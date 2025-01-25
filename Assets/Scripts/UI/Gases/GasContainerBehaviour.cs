using GumFly.Domain;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GumFly.UI.Gases
{
    public class GasContainerBehaviour : Selectable, IInitializable<GasContainer>
    {
        [field: SerializeField]
        public UnityEvent<GasContainer> StartPulling { get; private set; }

        [field: SerializeField]
        public UnityEvent<GasContainer> StopPulling { get; private set; }

        [SerializeField]
        private Image _fill;

        private GasContainer _gasContainer;

        public void Initialize(GasContainer gasContainer)
        {
            _gasContainer = gasContainer;
            _fill.color = gasContainer.Gas.Color;

            LMotion.Create(Vector3.zero, transform.localScale, 0.4f)
                .WithEase(Ease.InOutQuad)
                .WithDelay(transform.GetSiblingIndex() * 0.05f)
                .BindToLocalScale(transform);
        }

        private void Update()
        {
            if (_gasContainer == null) return; 
            _fill.fillAmount = _gasContainer.Fill;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (this.currentSelectionState == SelectionState.Pressed)
            {
                StartPulling.Invoke(_gasContainer);
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            
            if (this.currentSelectionState != SelectionState.Pressed)
            {
                StopPulling.Invoke(_gasContainer);
            }
        }

        public override void Select()
        {
            // Ignore select
        }
    }
}