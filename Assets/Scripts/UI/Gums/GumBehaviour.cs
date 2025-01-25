using GumFly.Domain;
using GumFly.ScriptableObjects;
using UnityEngine;

namespace GumFly.UI.Gums
{
    public class GumBehaviour : MonoBehaviour, IInitializable<Gum>
    {
        public void Consume()
        {
            Destroy(gameObject);
        }

        public void Initialize(Gum instance)
        {
            
        }
    }
}