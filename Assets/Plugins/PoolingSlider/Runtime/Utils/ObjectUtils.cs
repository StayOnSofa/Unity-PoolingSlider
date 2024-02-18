using System.Collections.Generic;
using UnityEngine;

namespace PoolingSlider.Utils
{
    public class ObjectList<T> : List<T> where T : Object
    {
        public new void Add(T item)
        {
            if (item != null)
            {
                base.Add(item);
            }
        }

        public new bool Remove(T item)
        {
            if (item != null && base.Remove(item))
            {
                Object.Destroy(item);
                return true;
            }

            return false;
        }

        public new void RemoveAt(int index)
        {
            if (index >= 0 && index < Count)
            {
                var item = this[index];

                if (item is Component)
                    Object.Destroy((item as Component).gameObject);
                else
                    Object.Destroy(item);

                base.RemoveAt(index);
            }
        }

        public new void Clear()
        {
            foreach (var item in this)
            {
                if (item != null)
                {
                    if (item is Component)
                        Object.Destroy((item as Component).gameObject);
                    else
                        Object.Destroy(item);
                }
            }

            base.Clear();
        }
    }
}