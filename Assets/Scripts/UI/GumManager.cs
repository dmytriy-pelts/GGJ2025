using Cysharp.Threading.Tasks;
using GumFly.Domain;
using GumFly.ScriptableObjects;
using GumFly.UI.Gums;
using GumFly.Utils;
using System;
using UnityEngine;

namespace GumFly.UI
{
    public class GumManager : MonoSingleton<GumManager>
    {
        private Inventory _inventory;

        [SerializeField]
        private GumPackageBehaviour _gumPackagePrefab;

        [SerializeField]
        private RectTransform _gumPackageContainer;

        public void Initialize(Inventory inventory)
        {
            Debug.Log("Initializing gas manager", this);
            _inventory = inventory;

            // Clean
            for (int i = _gumPackageContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(_gumPackageContainer.GetChild(i));
            }

            foreach (var package in inventory.Gums)
            {
                var instance = Instantiate(_gumPackagePrefab, _gumPackageContainer);
                instance.Initialize(package);
            }
        }

        public async UniTask<Gum> PickGumAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(10));
            return null;
        }
    }
}