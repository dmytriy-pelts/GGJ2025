using GumFly.Domain;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GumFly.UI.Gums
{
    public class GumPackageBehaviour : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private GumBehaviour _gumBehaviourPrefab;

        [SerializeField]
        private RectTransform _gumContainer;

        private GumPackage _package;

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
            }
        }
    }
}