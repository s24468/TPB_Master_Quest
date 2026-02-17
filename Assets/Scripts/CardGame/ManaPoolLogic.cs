using System;
using UnityEngine;

namespace Cards
{
    [Serializable]
    public class ManaPoolLogic
    {
        [SerializeField] private int maxCrystals;
        [SerializeField] private int availableCrystals;

        public int MaxCrystals => maxCrystals;
        public int AvailableCrystals => availableCrystals;

        /// <summary>
        /// AvailableCrystals
        /// </summary>
        public void SubtractAvailableCrystals(int value)
        {
            availableCrystals = Mathf.Clamp(availableCrystals - value, 0, maxCrystals);
        }

        public void AddMaxCrystals(int value)
        {
            maxCrystals+=value;
            availableCrystals += value;
        }

        public void RefillToMax()
        {
            availableCrystals = maxCrystals;
        }

        public void AddAvailable(int amount)
        {
            if (amount <= 0) return;
            availableCrystals = Mathf.Clamp(availableCrystals + amount, 0, maxCrystals);
        }

    }
}
