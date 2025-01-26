using GumFly.Domain;
using GumFly.Extensions;
using GumFly.ScriptableObjects;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GumFly.UI.Gums
{
    public class GumPackageBehaviour : MonoBehaviour, IPointerClickHandler, IInitializable<GumPackage>
    {
        [SerializeField]
        private GumBehaviour _gumBehaviourPrefab;

        [SerializeField]
        private RectTransform _gumContainer;

        [SerializeField]
        private Image _sprite;

        [SerializeField]
        private Button _button;
        
        [field:SerializeField]
        public UnityEvent<Gum> GumPicked { get; private set; }

        private GumPackage _package;

        private void OnEnable()
        {
            _button.interactable = true;
        }

        private void OnDisable()
        {
            _button.interactable = false;
        }

        public void Initialize(GumPackage package)
        {
            _package = package;

            _gumContainer.FillWithPrefabs(_gumBehaviourPrefab, package.GumType, package.Count);
            _sprite.sprite = package.GumType.Sprite;
            
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(-10.0f, 10.0f));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_package.TryTakeGum(out var gum))
            {
                _gumContainer.GetChild(0)
                    .GetComponent<GumBehaviour>()
                    .Consume();
                
                GumPicked.Invoke(gum);
            }
        }
    }
}