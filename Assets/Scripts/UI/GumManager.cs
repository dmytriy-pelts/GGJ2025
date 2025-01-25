using Cysharp.Threading.Tasks;
using GumFly.Domain;
using GumFly.ScriptableObjects;
using GumFly.UI.Gums;
using GumFly.Utils;
using System;
using System.Collections.Generic;
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
        
        private List<GumPackageBehaviour> _gumPackages = new List<GumPackageBehaviour>();

        private UniTaskCompletionSource<Gum> _selectedGum = new UniTaskCompletionSource<Gum>();

        public void Initialize(Inventory inventory)
        {
            Debug.Log("Initializing gas manager", this);
            _inventory = inventory;
            
            // Clean
            for (int i = _gumPackageContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(_gumPackageContainer.GetChild(i));
            }
            _gumPackages.Clear();

            foreach (var package in inventory.Gums)
            {
                var instance = Instantiate(_gumPackagePrefab, _gumPackageContainer);
                instance.Initialize(package);
                
                instance.GumPicked.AddListener((gum) =>
                {
                    _selectedGum.TrySetResult(gum);
                });
                
                _gumPackages.Add(instance);
            }
        }

        public async UniTask<Gum> PickGumAsync()
        {
            foreach (var package in _gumPackages)
            {
                package.enabled = true;
            }
            

            var gum = await _selectedGum.Task;
            _selectedGum = new UniTaskCompletionSource<Gum>();
            
            foreach (var package in _gumPackages)
            {
                package.enabled = false;
            }
            
            return gum;
        }
    }
}