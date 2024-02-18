using System;
using System.Collections.Generic;
using System.Linq;
using PoolingSlider.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PoolingSlider
{
    [RequireComponent(typeof(RectTransform), typeof(ScrollRect))]
    public class PoolingSlider : MonoBehaviour
    {
        private const int MaxSlidesBuffer = 2;

        private enum Direction
        {
            Horizontal = 0,
            Vertical = 1
        }

        private ScrollRect _scrollCache;

        public ScrollRect Scroll
        {
            get
            {
                if (_scrollCache == null) _scrollCache = GetComponent<ScrollRect>();

                return _scrollCache;
            }
        }
        
        [SerializeField] private Direction _direction = Direction.Horizontal;
        [SerializeField] private float _spacing = 5;

        private int _maxSlides;

        private int _maxSlidesVisible;
        private float _contentHalfSize;
        private float _prefabSize;

        private Dictionary<int, int[]> _slideDictionary = new();
        private ObjectList<RectTransform> _slides = new();

        private int _maxSlidesItems;

        private Vector3 _startPosition;
        private Vector3 _offsetVector;

        private Action<int, int> _refreshSlide;

        private void OnEnable() => Scroll.onValueChanged.AddListener(UpdateScrollRect);
        private void OnDisable() => Scroll.onValueChanged.RemoveListener(UpdateScrollRect);

        public void Constructor<T>(PoolingSlide<T> prefab, IEnumerable<T> array, Action<T> onClick)
        {
            _slideDictionary.Clear();
            _slides.Clear();

            var contents = array.ToArray();
            _maxSlides = contents.Length;

            var originalSlides = new PoolingSlide<T>[_maxSlides];
            var rememberData = new T[_maxSlides];

            var content = Scroll.content;
            var view = Scroll.viewport;

            Vector2 prefabScale = prefab.RectTransform.rect.size;

            _prefabSize = (_direction == Direction.Horizontal ? prefabScale.x : prefabScale.y) + _spacing;

            content.sizeDelta = _direction == Direction.Horizontal
                ? new Vector2(_prefabSize * _maxSlides, prefabScale.y)
                : new Vector2(prefabScale.x, _prefabSize * _maxSlides);

            var rect = content.rect;
            _contentHalfSize = _direction == Direction.Horizontal ? rect.size.x * 0.5f : rect.size.y * 0.5f;

            var viewRect = view.rect;
            _maxSlidesVisible =
                Mathf.CeilToInt((_direction == Direction.Horizontal ? viewRect.size.x : viewRect.size.y) / _prefabSize);

            _offsetVector = _direction == Direction.Horizontal ? Vector3.right : Vector3.down;
            _startPosition = content.anchoredPosition3D - _offsetVector * _contentHalfSize + _offsetVector *
                ((_direction == Direction.Horizontal ? prefabScale.x : prefabScale.y) * 0.5f);

            _maxSlidesItems = Mathf.Min(_maxSlides, _maxSlidesVisible + MaxSlidesBuffer);

            for (int i = 0; i < _maxSlidesItems; i++)
            {
                int originalIndex = i;
                var data = contents[i];

                var slide = Instantiate(prefab, content.transform);

                slide.Constructor(data);
                slide.Constructor(() =>
                {
                    var real = rememberData[originalIndex];
                    onClick?.Invoke(real);
                });

                RectTransform rectTransform = slide.RectTransform;
                rectTransform.anchoredPosition3D = _startPosition + _offsetVector * i * _prefabSize;

                _slides.Add(slide.RectTransform);

                rememberData[i] = data;
                originalSlides[i] = slide;

                _slideDictionary.Add(rectTransform.GetInstanceID(), new[] { i, i });
            }

            _refreshSlide = (slideIndex, dataIndex) =>
            {
                var slide = originalSlides[slideIndex];
                var data = contents[dataIndex % contents.Length];

                rememberData[slideIndex] = data;
                slide.Constructor(data);
            };

            content.anchoredPosition3D += _offsetVector * (_contentHalfSize -
                                                           (_direction == Direction.Horizontal
                                                               ? viewRect.size.x
                                                               : viewRect.size.y) * 0.5f);

            var normalized = Scroll.normalizedPosition;
            float value = _direction == Direction.Horizontal ? normalized.x : normalized.y;

            ReorderItemsByPos(value);
            Canvas.ForceUpdateCanvases();
        }

        private void UpdateScrollRect(Vector2 values)
        {
            float value = _direction == Direction.Horizontal ? values.x : values.y;
            ReorderItemsByPos(value);
        }

        private void ReorderItemsByPos(float normPos)
        {
            if (_direction == Direction.Vertical) normPos = 1f - normPos;

            int maxSlidesOutOfView = Mathf.CeilToInt(normPos * (_maxSlides - _maxSlidesVisible));
            int firstIndex = Mathf.Max(0, maxSlidesOutOfView - MaxSlidesBuffer);
            int originalIndex = firstIndex % _maxSlidesItems;

            int newIndex = firstIndex;
            for (int i = originalIndex; i < _maxSlidesItems; i++)
            {
                var rectTransform = _slides[i];
                MoveItemByIndex(rectTransform, newIndex);

                _refreshSlide.Invoke(i, newIndex);
                newIndex++;
            }

            for (int i = 0; i < originalIndex; i++)
            {
                var rectTransform = _slides[i];
                MoveItemByIndex(rectTransform, newIndex);

                _refreshSlide.Invoke(i, newIndex);
                newIndex++;
            }
        }

        private void MoveItemByIndex(RectTransform item, int index)
        {
            int id = item.GetInstanceID();

            if (index < _maxSlides)
            {
                _slideDictionary[id][0] = index;
                item.anchoredPosition3D = _startPosition + _offsetVector * index * _prefabSize;
            }
        }
    }
}