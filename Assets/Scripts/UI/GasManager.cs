using Cysharp.Threading.Tasks;
using GumFly.Domain;
using GumFly.Extensions;
using GumFly.ScriptableObjects;
using GumFly.UI.Gases;
using GumFly.Utils;
using UnityEngine;

namespace GumFly.UI
{
    public class GasManager : MonoSingleton<GasManager>
    {
        public float Capacity { get; set; }
        
        [SerializeField]
        private RectTransform _container;

        [SerializeField]
        private GasContainerBehaviour _gasContainerPrefab;
        
        private Inventory _inventory;

        public void Initialize(Inventory inventory)
        {
            Debug.Log("Initializing gas manager", this);
            _inventory = inventory;

            _container.FillWithPrefabs(_gasContainerPrefab, _inventory.Gases);
        }

        public async UniTask PickGasesAsync(GumGasMixture mixture, float capacity)
        {
            Debug.Log("PICK GAS");
            await UniTask.Delay(5000);
        }
    }
}