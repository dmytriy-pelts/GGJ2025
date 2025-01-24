using Cysharp.Threading.Tasks;
using GumFly.ScriptableObjects;
using GumFly.Utils;
using UnityEngine;

namespace GumFly.UI
{
    public class GumManager : MonoSingleton<GumManager>
    {
        private Inventory _inventory;
        public void Initialize(Inventory inventory)
        {
            Debug.Log("Initializing gas manager", this);
            _inventory = inventory;
        }

        public async UniTask<Gum> PickGumAsync()
        {
            return null;
        }
    }
}