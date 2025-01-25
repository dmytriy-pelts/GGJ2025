using System;
using UnityEngine;
using UnityEngine.UI;

namespace GumFly.UI.Gases
{
    public class CapacityDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image _image;
        
        public void Update()
        {
            _image.fillAmount = GasManager.Instance.Capacity;
        }
    }
}