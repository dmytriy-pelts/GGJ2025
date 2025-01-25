using Cysharp.Threading.Tasks;
using GumFly.Domain;
using GumFly.ScriptableObjects;
using GumFly.Utils;
using UnityEngine;

namespace GumFly.UI
{
    public class GasManager : MonoSingleton<GasManager>
    {
        public float Capacity { get; set; }
        
        [SerializeField]
        private RectTransform _container;
        private Inventory _inventory;

        public void Initialize(Inventory inventory)
        {
            Debug.Log("Initializing gas manager", this);
            _inventory = inventory;
        }

        public async UniTask PickGasesAsync(GumGasMixture mixture, float capacity)
        {
            Debug.Log("PICK GAS");
            await UniTask.Delay(5000);
        }
    }
}