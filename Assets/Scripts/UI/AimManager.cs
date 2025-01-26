using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using GumFly.Domain;
using GumFly.Utils;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GumFly.UI
{
    enum Signal
    {
        None,
        Click,
        Burst,
        Gulp
    }
    
    public class AimManager : MonoSingleton<AimManager>
    {
        [SerializeField]
        private BubbleFlightPath _flightController;
        
        private AsyncReactiveProperty<Signal>
            _clickSignals = new AsyncReactiveProperty<Signal>(Signal.None);

        public async UniTask WaitUntilBubblesAreGone()
        {
            while (AnyBubblesAirborne())
            {
                await UniTask.Delay(1000);
            }
        }

        private bool AnyBubblesAirborne()
        {
            return FindObjectsOfType<BubbleBehaviour>().Any(it => it.IsReleased);
        }

        private void Start()
        {
            _flightController.gameObject.SetActive(false);
            
            GasManager.Instance.OnBurst.AddListener(OnBurst);
        }

        private void OnBurst()
        {
            _clickSignals.Value = Signal.Burst;
        }

        public async UniTask AimAsync(GumGasMixture mixture, CancellationToken cancellation)
        {
            _flightController.gameObject.SetActive(true);

            // Placeholder to detect click
            var evt = await _clickSignals.Skip(1).FirstAsync(cancellationToken: cancellation);

            switch (evt)
            {
                case Signal.None:
                    Debug.LogWarning("Got none signal?!");
                    break;
                case Signal.Click:
                    _flightController.Shoot();
                    break;
                case Signal.Burst:
                    SoundManager.Instance.PlaySplat(transform.position);
                    break;
                case Signal.Gulp:
                    SoundManager.Instance.PlayGulp();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            _flightController.gameObject.SetActive(false);
            await UniTask.Delay(1000);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (GameManager.Instance.CurrentMixture.GasAmounts.Count == 0)
                {
                    // Ignore
                 //   _clickSignals.Value = Signal.Gulp;
                }
                else
                {
                    _clickSignals.Value = Signal.Click;
                }
            }
        }
    }
}