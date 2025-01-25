using Cysharp.Threading.Tasks;
using GumFly.Domain;
using GumFly.Extensions;
using GumFly.ScriptableObjects;
using GumFly.UI.Gases;
using GumFly.Utils;
using System;
using UnityEngine;

namespace GumFly.UI
{
    public class GasManager : MonoSingleton<GasManager>
    {
        [SerializeField]
        private RectTransform _container;

        [SerializeField]
        private GasContainerBehaviour _gasContainerPrefab;

        private Inventory _inventory;
        private GumGasMixture _mixture;


        private GasContainer _pulling;
        private GasContainerBehaviour[] _containers;

        public void Initialize(Inventory inventory)
        {
            Debug.Log("Initializing gas manager", this);
            _inventory = inventory;

            _containers = _container.FillWithPrefabs(_gasContainerPrefab, _inventory.Gases);
            foreach (var instance in _containers)
            {
                instance.StartPulling.AddListener(OnStartPulling);
                instance.StopPulling.AddListener(OnStopPulling);
                instance.interactable = false;
            }
        }

        private void OnStartPulling(GasContainer gasContainer)
        {
            if (_mixture == null) return;

            _pulling = gasContainer;
        }

        private void OnStopPulling(GasContainer gasContainer)
        {
            _pulling = null;
        }

        public async UniTask PickGasesAsync(GumGasMixture mixture, float capacity)
        {
            foreach (var container in _containers)
            {
                container.interactable = true;
            }

            _mixture = mixture;

            await UniTask.Delay(5000);
            while (_pulling != null)
            {
                await UniTask.Yield();
            }
            
            _mixture = null;
            
            foreach (var container in _containers)
            {
                container.interactable = false;
            }
        }

        private void Update()
        {
            if(_pulling == null || _mixture == null) return;
            
            float amount = _pulling.Pull(_mixture.RemainingCapacity);
            _mixture.Add(_pulling.Gas, amount);
        }
    }
}