using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GumFly.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Objects/Flies/Simple Fly", fileName = "Fly", order = 0)]
    public class Fly : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public float WeightThreshold { get; private set; }

        [field: SerializeField]
        public float MaxSpeed { get; private set; }

        [field: SerializeField]
        public bool IsFriendly { get; private set; }

        [field: SerializeField]
        public bool IsAbleToEvade { get; private set; }

        [field: SerializeField]
        public float DetectionRadius { get; private set; }

        [field: SerializeField]
        public float EvasionRadius { get; private set; }

        [field: SerializeField]
        public float MaxEvasionSpeed { get; private set; }

        [field: SerializeField]
        public float EvasionTotalCooldownInSec { get; private set; }
    }

}