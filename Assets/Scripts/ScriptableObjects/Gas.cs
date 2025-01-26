using UnityEngine;

namespace GumFly.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Objects/Gases/Simple Gas", fileName = "Gas", order = 0)]
    public class Gas : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; private set; }
        
        [field: SerializeField]
        public Color Color { get; private set; }

        [field: SerializeField]
        public float VelocityScale { get; private set; } = 1.0f;
        
        [field: SerializeField]
        public float SizeScale { get; private set; } = 0.0f;
        
        [field: SerializeField]
        public float GravityDecay { get; private set; } = 0.0f;

        [field: SerializeField]
        public float PullSpeed { get; private set; } = 1.0f;
    }
}