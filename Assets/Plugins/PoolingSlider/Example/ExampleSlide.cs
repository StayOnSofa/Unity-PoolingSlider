using UnityEngine;
using UnityEngine.UI;

namespace PoolingSlider.Example
{

    //Model layer
    public class Example
    {
        public Color Color { private set; get; }
        public Sprite Sprite { private set; get; }
        
        public Example(Color color, Sprite sprite)
        {
            Color = color;
            Sprite = sprite;
        }
    }

    //View layer
    public class ExampleSlide : PoolingSlide<Example>
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _image;

        public override void Constructor(Example value)
        {
            _background.color = value.Color;
            _image.sprite = value.Sprite;
        }
    }
}