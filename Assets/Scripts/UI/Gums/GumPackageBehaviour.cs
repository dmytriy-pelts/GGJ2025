using GumFly.Domain;
using GumFly.ScriptableObjects;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GumFly.UI.Gums
{
    public class GumPackageBehaviour : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private GumBehaviour _gumBehaviourPrefab;

        [SerializeField]
        private RectTransform _gumContainer;

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

            // Clean up
            for (int i = _gumContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(_gumContainer.GetChild(i).gameObject);
            }

            for (int i = 0; i < _package.Count; i++)
            {
                Instantiate(_gumBehaviourPrefab, _gumContainer);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_package.TryTakeGum(out var gum))
            {
                _gumContainer.GetChild(_package.Count)
                    .GetComponent<GumBehaviour>()
                    .Consume();
                
                GumPicked.Invoke(gum);
            }
        }
    }
}