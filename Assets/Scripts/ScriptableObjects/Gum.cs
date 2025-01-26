using GumFly.Domain;
using UnityEngine;

namespace GumFly.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Objects/Gums/Simple Gum", fileName = "Gum", order = 0)]
    public class Gum : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; private set; }
        
        [field: SerializeField]
        public Rhythm Rhythm { get; private set; }

        [field: SerializeField]
        [field: Tooltip("The size of this gum.")]
        public int Radius { get; private set; } = 50;

        [field: SerializeField]
        [field: Tooltip("The base velocity of this gum.")]
        public float Velocity { get; private set; } = 10.0f;
        
        [field: SerializeField]
        public Color Color { get; private set; }

        [field: SerializeField]
        public float Weight { get; private set; }
    }
}