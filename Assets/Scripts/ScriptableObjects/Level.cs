using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GumFly.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Objects/Levels/Level", fileName = "Fly Amount in Level", order = 0)]
    public class Level : ScriptableObject
    {
        [Serializable]
        public struct FlyAmount
        {
            public int amount;
            public GameObject reference;
        }

        [field: SerializeField]
        public FlyAmount[] flyTypesInLevel { get; private set; }
    }
}
