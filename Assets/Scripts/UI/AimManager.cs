using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using GumFly.Domain;
using GumFly.Utils;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GumFly.UI
{
    public class AimManager : MonoSingleton<AimManager>
    {
        [SerializeField]
        private BubbleFlightPath _flightController;

        private AsyncReactiveProperty<AsyncUnit>
            _clickSignals = new AsyncReactiveProperty<AsyncUnit>(AsyncUnit.Default);

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
        }

        public async UniTask AimAsync(GumGasMixture mixture, CancellationToken cancellation)
        {
            _flightController.gameObject.SetActive(true);

            // Placeholder to detect click
            await _clickSignals.Skip(1).FirstAsync(cancellationToken: cancellation);
            
            _flightController.Shoot();

            await UniTask.Delay(1000);

            _flightController.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _clickSignals.Value = AsyncUnit.Default;
            }
        }
    }
}