using System;
using UnityEngine;

namespace GumFly.ScriptableObjects
{
    [Serializable]
    public struct FlyAmount
    {
        public int amount;
        public GameObject reference;
    }

    [CreateAssetMenu(menuName = "Objects/Levels/Level", fileName = "Fly Amount in Level", order = 0)]
    public class Level : ScriptableObject
    {
        [field: SerializeField]
        public FlyAmount[] flyTypesInLevel { get; private set; }
    }
}
