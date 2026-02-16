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
        /// Ustawia max many (liczbę slotów). available jest automatycznie przycinane do max.
        /// </summary>
        public void SetMax(int value, int hardCap)
        {
            maxCrystals = Mathf.Clamp(value, 0, hardCap);
            availableCrystals = Mathf.Clamp(availableCrystals, 0, maxCrystals);
        }

        /// <summary>
        /// Ustawia dostępną manę. Przycinane do [0, maxCrystals].
        /// </summary>
        public void SetAvailable(int value)
        {
            availableCrystals = Mathf.Clamp(value, 0, maxCrystals);
        }

        /// <summary>
        /// Reset dostępnej many na start tury (typowo = max).
        /// </summary>
        public void RefillToMax()
        {
            availableCrystals = maxCrystals;
        }

        /// <summary>
        /// Próbuje wydać manę. Zwraca true jeśli się udało.
        /// </summary>
        public bool TrySpend(int amount)
        {
            if (amount <= 0) return true;
            if (availableCrystals < amount) return false;

            availableCrystals -= amount;
            return true;
        }

        /// <summary>
        /// Dodaje manę (np. efekt karty), ale nie przekracza max.
        /// </summary>
        public void AddAvailable(int amount)
        {
            if (amount <= 0) return;
            availableCrystals = Mathf.Clamp(availableCrystals + amount, 0, maxCrystals);
        }

        /// <summary>
        /// Pomocnicze: ustawia oba naraz (np. init).
        /// </summary>
        public void SetState(int newMax, int newAvailable, int hardCap)
        {
            maxCrystals = Mathf.Clamp(newMax, 0, hardCap);
            availableCrystals = Mathf.Clamp(newAvailable, 0, maxCrystals);
        }
    }
}
