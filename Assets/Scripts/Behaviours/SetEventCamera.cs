using System;
using UnityEngine;

namespace GumFly.Behaviours
{
    [RequireComponent(typeof(Canvas))]
    public class SetEventCamera : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}