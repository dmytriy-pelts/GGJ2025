using GumFly.ScriptableObjects;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GumFly.Domain
{
    public class GumGasMixture
    {
        public Gum Gum;
        private List<GasAndAmount> _amounts = new List<GasAndAmount>();

        public void Add(Gas gas, float amount)
        {
            for (int i = 0; i < _amounts.Count; i++)
            {
                GasAndAmount gasAndAmount = _amounts[i];
                if (gasAndAmount.Gas == gas)
                {
                    gasAndAmount.Amount += amount;
                    _amounts[i] = gasAndAmount;
                    return;
                }
            }

            _amounts.Add(new GasAndAmount { Gas = gas, Amount = amount });
        }

        private struct GasAndAmount
        {
            public Gas Gas;
            public float Amount;
        }
    }
}