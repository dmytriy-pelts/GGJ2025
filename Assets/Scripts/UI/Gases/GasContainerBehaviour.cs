using GumFly.Domain;
using GumFly.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace GumFly.UI.Gases
{
    public class GasContainerBehaviour : MonoBehaviour, IInitializable<GasContainer>
    {
        private GasContainer _gasContainer;

        [field:SerializeField]
        public UnityEvent<GasContainer> StartPulling { get; private set; }
        
        [field:SerializeField]
        public UnityEvent<GasContainer> StopPulling { get; private set; }

        public void Initialize(GasContainer gasContainer)
        {
            _gasContainer = gasContainer;
        }
    }
}