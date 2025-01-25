﻿using GumFly.ScriptableObjects;
using System;
using UnityEngine;

namespace GumFly.Domain
{
    [Serializable]
    public class GasContainer
    {
        public Gas Gas;
        public float Fill;

        /// <summary>
        /// Pulls some gas from the gas container.
        /// </summary>
        /// <returns></returns>
        public float Pull(float remainingCapacity)
        {
            float pullAmount = Time.deltaTime;
            pullAmount = Mathf.Min(pullAmount, remainingCapacity);

            if (Fill >= pullAmount)
            {
                Fill -= pullAmount;
                return pullAmount;
            }

            pullAmount = Fill;
            Fill = 0.0f;

            return pullAmount;
        }
    }
}