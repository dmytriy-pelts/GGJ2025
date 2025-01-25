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

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _gasClip;
        
        [SerializeField]
        private AudioClip _noGasClip;


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

        private void Start()
        {
        }

        private void OnEnable()
        {
            GameManager.Instance.StateChanged.AddListener(OnStateChanged);
        }

        private void OnDisable()
        {
            GameManager.Instance.StateChanged.RemoveListener(OnStateChanged);
        }

        private void OnStateChanged(StateChangeEvent e)
        {
            if (e.NewState == GameState.Aiming)
            {
                foreach (var container in _containers)
                {
                    container.interactable = true;
                }

                _mixture = GameManager.Instance.CurrentMixture;
            } else if (e.OldState == GameState.Aiming)
            {
                _mixture = null;
            
                foreach (var container in _containers)
                {
                    container.interactable = false;
                }
            }
        }

        private void OnStartPulling(GasContainer gasContainer)
        {
            if (_mixture == null) return;

            _pulling = gasContainer;
            _audioSource.Play();
        }

        private void OnStopPulling(GasContainer gasContainer)
        {
            _audioSource.Stop();
            _pulling = null;
        }


        private void Update()
        {
            if(_pulling == null || _mixture == null) return;
            
            float amount = _pulling.Pull(_mixture.RemainingCapacity);
            _mixture.Add(_pulling.Gas, amount);
            
            _audioSource.clip = amount > 0.0f ? _gasClip : _noGasClip;
        }
    }
}