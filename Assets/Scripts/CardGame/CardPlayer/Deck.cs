using System.Collections.Generic;
using Cards.ExtenstionMethods;
using UnityEngine;

namespace Cards
{
    public class Deck : MonoBehaviour
    {
        public List<CardAsset> cards = new List<CardAsset>();

        void Awake()
        {
            // ShufflingExtention.Shuffle(cards);
        }
    }
}