using UnityEngine;

namespace GumFly.UI.Gums
{
    public class GumBehaviour : MonoBehaviour
    {
        public void Consume()
        {
            Destroy(gameObject);
        }
    }
}