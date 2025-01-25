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
                    Object.Destroy(transform.GetChild(i));
                }
                else
                {
                    Object.DestroyImmediate(transform.GetChild(i));
                }
            }
        }

        public static void FillWithPrefabs<TPrefab, TInstance>(this Transform transform, TPrefab prefab, IEnumerable<TInstance> instances) where TPrefab : MonoBehaviour, IInitializable<TInstance>
        {
            // TODO: Could be optimized to reuse prefab instances 
            transform.DestroyChildren();

            foreach (var instance in instances)
            {
                var prefabInstance = Object.Instantiate(prefab, transform);
                prefabInstance.Initialize(instance);
            }
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