using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using GumFly.Extensions;
using GumFly.ScriptableObjects;
using GumFly.UI.Gums;
using GumFly.Utils;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GumFly.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GumManager : MonoSingleton<GumManager>
    {
        private Inventory _inventory;

        [SerializeField]
        private GumPackageBehaviour _gumPackagePrefab;

        [SerializeField]
        private RectTransform _gumPackageContainer;

        [SerializeField]
        public RectTransform RectTransform => transform as RectTransform;

        private List<GumPackageBehaviour> _gumPackages = new List<GumPackageBehaviour>();

        private CanvasGroup _canvasGroup;

        private AsyncReactiveProperty<Gum> _selectedGum = new AsyncReactiveProperty<Gum>(null);
        public void Initialize(Inventory inventory)
        {
            Debug.Log("Initializing gas manager", this);
            _inventory = inventory;

            // Clean
            _gumPackageContainer.DestroyChildren();
            _gumPackages.Clear();

            foreach (var package in inventory.Gums)
            {
                var instance = Instantiate(_gumPackagePrefab, _gumPackageContainer);
                instance.Initialize(package);

                instance.GumPicked.AddListener((gum) =>
                {
                    _selectedGum.Value = gum;
                });

                instance.enabled = false;
                _gumPackages.Add(instance);
            }
        }

        private void Start()
        {
            GameManager.Instance.StateChanged.AddListener(OnStateChanged);
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0.5f;
        }

        private void OnStateChanged(StateChangeEvent e)
        {
            Debug.Log(e.NewState);
            _canvasGroup.alpha = e.NewState == GameState.PickingGum ? 1.0f : 0.5f;
        }

        public async UniTask<Gum> PickGumAsync(CancellationToken cancellation)
        {
            await FaceManager.Instance.MoveToGums();
            foreach (var package in _gumPackages)
            {
                package.enabled = true;
            }

            var gum = await _selectedGum.FirstAsync(it => it != null, cancellation);
            
            await UniTask.Delay(1000, cancellationToken: cancellation);

            foreach (var package in _gumPackages)
            {
                package.enabled = false;
            }

            return gum;
        }
    }
}