using GumFly.Domain;
using System.Linq;
using UnityEngine;

namespace GumFly.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Objects/Inventory", fileName = "Inventory", order = 0)]
    public class Inventory : ScriptableObject
    {
        [field:SerializeField]
        public GumPackage[] Gums { get; private set; }
        
        [field:SerializeField]
        public GasContainer[] Gases { get; private set; }

        public bool HasAnyGumsLeft => Gums.Any(it => it.Count > 0);
        
        public bool HasAnyGasLeft => Gases.Any(it => it.Fill > 0.0f);
    }
}