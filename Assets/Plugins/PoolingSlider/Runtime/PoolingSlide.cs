using System;
using UnityEngine;
using UnityEngine.UI;

namespace PoolingSlider
{
    [RequireComponent(typeof(RectTransform), typeof(Button))]
    public abstract class PoolingSlide<T> : MonoBehaviour
    {
        private Button _buttonCache;
        private Button _button {
            get
            {
                if (_buttonCache == null)
                    _buttonCache = GetComponent<Button>();

                return _buttonCache;
            }
        }

        private RectTransform _rectTransformCache;
        public RectTransform RectTransform {
            get
            {
                if (_rectTransformCache == null)
                    _rectTransformCache = GetComponent<RectTransform>();

                return _rectTransformCache;
            }
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private Action _onClick;

        private void OnClick()
        {
            _onClick?.Invoke();
        }

        public void Constructor(Action action)
        {
            _onClick = action;
        }

        public abstract void Constructor(T value);
    }
}