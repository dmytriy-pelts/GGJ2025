﻿using Cysharp.Threading.Tasks;
using GumFly.Domain;
using GumFly.Extensions;
using GumFly.ScriptableObjects;
using GumFly.UI.Gums;
using GumFly.Utils;
using System;
using System.Collections.Generic;
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

        private UniTaskCompletionSource<Gum> _selectedGum = new UniTaskCompletionSource<Gum>();
        private CanvasGroup _canvasGroup;

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
                    _selectedGum.TrySetResult(gum);
                });
                
                instance.enabled = false;
                _gumPackages.Add(instance);
            }
        }

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0.5f;
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
            _canvasGroup.alpha = e.NewState == GameState.PickingGum ? 1.0f : 0.5f;
        }

        public async UniTask<Gum> PickGumAsync()
        {
            await FaceManager.Instance.MoveToGums();
            foreach (var package in _gumPackages)
            {
                package.enabled = true;
            }

            var gum = await _selectedGum.Task;
            _selectedGum = new UniTaskCompletionSource<Gum>();

            await UniTask.Delay(1000);
            
            foreach (var package in _gumPackages)
            {
                package.enabled = false;
            }
            
            return gum;
        }
    }
}