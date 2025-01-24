using GumFly.ScriptableObjects;
using System;

namespace GumFly.Domain
{
    /// <summary>
    /// A package of gum.
    /// </summary>
    [Serializable]
    public class GumPackage
    {
        public Gum GumType;
        public int Count;

        /// <summary>
        /// Tries to take out a gum from the package.
        /// </summary>
        /// <param name="gum"></param>
        /// <returns></returns>
        public bool TryTakeGum(out Gum gum)
        {
            if (Count > 0)
            {
                Count--;
                gum = GumType;

                return true;
            }

            gum = null;
            return false;
        }
    }
}