using GumFly.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace GumFly.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Destroys all children of a transform.
        /// </summary>
        /// <param name="transform"></param>
        public static void DestroyChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(transform.GetChild(i).gameObject);
                }
                else
                {
                    Object.DestroyImmediate(transform.GetChild(i).gameObject);
                }
            }
        }

        public static TPrefab[] FillWithPrefabs<TPrefab, TInstance>(this Transform transform, TPrefab prefab, TInstance[] instances) where TPrefab : MonoBehaviour, IInitializable<TInstance>
        {
            // TODO: Could be optimized to reuse prefab instances 
            transform.DestroyChildren();

            var prefabs = new TPrefab[instances.Length];
            for (int i = 0; i < instances.Length; i++)
            {
                TInstance instance = instances[i];
                var prefabInstance = Object.Instantiate(prefab, transform);
                prefabInstance.Initialize(instance);
                
                prefabs[i] = prefabInstance;
            }

            return prefabs;
        }
        
        public static void FillWithPrefabs<TPrefab, TInstance>(this Transform transform, TPrefab prefab, TInstance instance, int count) where TPrefab : MonoBehaviour, IInitializable<TInstance>
        {
            // TODO: Could be optimized to reuse prefab instances 
            transform.DestroyChildren();

            for (int i = 0; i < count; i++)
            {
                var prefabInstance = Object.Instantiate(prefab, transform);
                prefabInstance.Initialize(instance);
            }
        }
    }
}