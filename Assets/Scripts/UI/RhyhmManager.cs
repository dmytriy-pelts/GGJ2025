using Cysharp.Threading.Tasks;
using GumFly.ScriptableObjects;
using GumFly.Utils;
using LitMotion;
using UnityEngine;

namespace GumFly.UI
{
    public class RhyhmManager : MonoSingleton<RhyhmManager>
    {
        public async UniTask<float> ChewAsync(Gum gum)
        {
            await LMotion.Create(0.0f, 1.0f, 3.0f).WithEase(Ease.InOutSine).Bind((capacity) =>
            {
                GasManager.Instance.Capacity = capacity;
            });
            
            return 1.0f;
        }
        
    }
}