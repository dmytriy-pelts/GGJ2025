using Cysharp.Threading.Tasks;
using GumFly.ScriptableObjects;
using GumFly.Utils;
using UnityEngine;

namespace GumFly.UI
{
    public class RhyhmManager : MonoSingleton<RhyhmManager>
    {
        public float Capacity { get; private set; }

        public async UniTask<float> ChewAsync(Gum gum)
        {
            Debug.Log("CHEW CHEW");
            await UniTask.Delay(5000);
            return 1.0f;
        }
        
    }
}