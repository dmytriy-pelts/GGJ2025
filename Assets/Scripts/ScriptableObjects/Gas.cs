using UnityEngine;

namespace GumFly.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Objects/Gases/Simple Gas", fileName = "Gas", order = 0)]
    public class Gas : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; private set; }
    }
}