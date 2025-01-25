using GumFly.ScriptableObjects;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace GumFly.Domain
{
    public class GumGasMixture
    {
        [CanBeNull]
        public Gum Gum;

        public float Capacity;

        public float RemainingCapacity
        {
            get
            {
                float caps = Capacity;
                foreach (var amount in GasAmounts)
                {
                    caps -= amount.Amount;
                }

                return Mathf.Max(0.0f, caps);
            }
        }
        
        public List<GasAndAmount> GasAmounts = new List<GasAndAmount>();

        public void Add(Gas gas, float amount)
        {
            for (int i = 0; i < GasAmounts.Count; i++)
            {
                GasAndAmount gasAndAmount = GasAmounts[i];
                if (gasAndAmount.Gas == gas)
                {
                    gasAndAmount.Amount += amount;
                    GasAmounts[i] = gasAndAmount;
                    return;
                }
            }

            GasAmounts.Add(new GasAndAmount { Gas = gas, Amount = amount });
        }

        public struct GasAndAmount
        {
            public Gas Gas;
            public float Amount;
        }
    }
}