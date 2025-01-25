using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using GumFly.Domain;
using GumFly.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GumFly.UI
{
    public class AimManager : MonoSingleton<AimManager>
    {
        private AsyncReactiveProperty<AsyncUnit>
            _clickSignals = new AsyncReactiveProperty<AsyncUnit>(AsyncUnit.Default);

        public async UniTask AimAsync(GumGasMixture mixture)
        {
            // Placeholder to detect click
            await _clickSignals.Skip(1).FirstAsync();
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