using System.Collections.Generic;
using UnityEngine;

namespace PoolingSlider.Example
{
    public class ExampleSetup : MonoBehaviour
    {
        [SerializeField] private PoolingSlider _poolingSlider;
        
        //Your pooling slide prefab to use
        [SerializeField] private ExampleSlide _prefab;
        [Space] 
        [SerializeField] private Sprite _example1;
        [SerializeField] private Sprite _example2;

        private void Start()
        {
            //fill up List/Array
            List<Example> examples = new();
            for (int i = 0; i < 128; i++)
            {
                examples.Add(new Example(Color.red, _example1));
                examples.Add(new Example(Color.yellow, _example2));
                examples.Add(new Example(Color.green, _example1));
                examples.Add(new Example(Color.cyan, _example2));
                examples.Add(new Example(Color.blue, _example1));
                examples.Add(new Example(Color.magenta, _example2));
                examples.Add(new Example(Color.white, _example1));
                examples.Add(new Example(Color.red, _example2));
                examples.Add(new Example(Color.yellow, _example1));
                examples.Add(new Example(Color.green, _example2));
                examples.Add(new Example(Color.cyan, _example1));
                examples.Add(new Example(Color.blue, _example2));
                examples.Add(new Example(Color.magenta, _example1));
                examples.Add(new Example(Color.white, _example2));
            }

            _poolingSlider.Constructor(_prefab, _examples, clickedExample =>
            {
                //Action where you click on slide
            });
        }
    }
}