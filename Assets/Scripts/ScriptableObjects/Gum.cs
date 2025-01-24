using UnityEngine;

namespace GumFly.ScriptableObjects
{
    
    [CreateAssetMenu(menuName = "Objects/Gums/Simple Gum", fileName = "Gum", order = 0)]
    public class Gum : ScriptableObject
    {
        public float Size;
    }
}