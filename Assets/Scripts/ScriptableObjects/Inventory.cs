using GumFly.Domain;
using UnityEngine;

namespace GumFly.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Objects/Inventory", fileName = "Inventory", order = 0)]
    public class Inventory : ScriptableObject
    {
        [SerializeField]
        GumPackage[] Gums;
        
        [SerializeField]
        GasContainer[] Gases;
    }
}