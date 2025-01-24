using GumFly.Domain;
using GumFly.Utils;

namespace GumFly
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private GumGasMixture _currentMixture = new GumGasMixture();
        
    }
}