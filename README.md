# Unity-Pooling Slider UI
![](https://github.com/StayOnSofa/Unity-PoolingSlider/blob/main/Example.gif?raw=true)
## Overview
The Unity Pooling Slider System introduces a design pattern and implementation focused on enhancing the performance and efficiency of UI sliders.
### Setting up
To get started, follow these steps:
1. Create a Scroll Rect and populate the View and Content fields.
2. Add the PoolingSlider class to the Scroll Rect.

![](https://github.com/StayOnSofa/Unity-PoolingSlider/blob/main/Example.png?raw=true)
Next, create a class model from which the UI sliders will retrieve information:
```C#
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
```
Fills up your UI slides, use PoolingSlide<YourModel>:
```C#
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
```

Initialize pooling slider with your data in a setup class:
```C#
//View layer
 public class ExampleSetup : MonoBehaviour
 {
    [SerializeField] private PoolingSlider _poolingSlider;
        
    // Your pooling slide prefab to use
    [SerializeField] private ExampleSlide _prefab;
    
    [Space] 
    [SerializeField] private Sprite _example1;
    [SerializeField] private Sprite _example2;

    private void Start()
    {
        // Fill up List/Array
        List<Example> examples = new List<Example>();
        for (int i = 0; i < 128; i++)
        {
            examples.Add(new Example(Color.red, _example1));
            // ...
        }

        _poolingSlider.Constructor(_prefab, examples, clickedExample =>
        {
            // Action to perform when a slide is clicked
        });
    }
}
```