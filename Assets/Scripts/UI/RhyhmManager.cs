using Cysharp.Threading.Tasks;
using GumFly.ScriptableObjects;
using GumFly.Utils;

namespace GumFly.UI
{
    public class RhyhmManager : MonoSingleton<RhyhmManager>
    {
        public float Capacity { get; private set; }

        public async UniTask<float> ChewAsync(Gum gum)
        {
            return 1.0f;
        }
        
    }
}