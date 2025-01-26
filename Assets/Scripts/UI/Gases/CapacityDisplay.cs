using System;
using UnityEngine;
using UnityEngine.UI;

namespace GumFly.UI.Gases
{
    public class CapacityDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private Image _remaining;

        public void Update()
        {
            _image.fillAmount = Mathf.Lerp(
                _image.fillAmount,
                GameManager.Instance.CurrentMixture.Capacity,
                Time.deltaTime * 6f);
            
            _remaining.fillAmount = GameManager.Instance.CurrentMixture.Capacity
                                    - GameManager.Instance.CurrentMixture.RemainingCapacity;
        }
    }
}