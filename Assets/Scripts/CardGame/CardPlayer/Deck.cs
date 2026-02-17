using System;
using System.Collections.Generic;
using System.Linq;
using Cards.ExtenstionMethods;
using UnityEngine;

namespace Cards
{
    public class Deck : MonoBehaviour
    {
        public List<CardAsset> cards = new List<CardAsset>();

        private void Awake()
        {
            setDeck();
            ShufflingExtention.Shuffle(cards);
        }

        public void setDeck()
        {
            var cardDictionaryCollected = DataPersistenceManager.instance.gameData.CardDictionaryCollected;

            var cards = CardDataLoader.Instance.GetCreatureCards();
            cards.AddRange(CardDataLoader.Instance.GetSpellCards());


            foreach (var cardDic in cardDictionaryCollected)
            {
                int key = Convert.ToInt32(cardDic.Key);
                int count = cardDic.Value;

                var card = cards.FirstOrDefault(c => c.ID == key);
                if (card == null)
                {
                    Debug.LogWarning($"setDeck(): Nie znaleziono karty o ID={key}");
                    continue;
                }

                for (int i = 0; i < count; i++)
                {
                    // TWORZYSZ NOWĄ INSTANCJĘ dla każdej kopii karty
                    CardAsset cardAsset = ScriptableObject.CreateInstance<CardAsset>();

                    cardAsset.Id = card.ID.ToString();
                    cardAsset.Name = card.Name;
                    cardAsset.CardImage = card.CardSprite;
                    cardAsset.ManaCost = card.Mana;
                    cardAsset.IsCreature = card.Type.ToLower() == "creature";
                    cardAsset.Description = card.Description;
                    cardAsset.CasualPower = card.CasualPower;
                    cardAsset.TPower = card.TPower;
                    cardAsset.PPower = card.PPower;
                    cardAsset.BPower = card.BPower;
                    cardAsset.TriggerAbilities = card.TriggerAbilities;
                    cardAsset.PassiveAbilities = card.PassiveAbilities;

                    this.cards.Add(cardAsset);
                }
            }
        }

        public CardAsset getRandomCardFromDeck()
        {
            CardAsset c = cards[0];
            cards.RemoveAt(0);
            return c;
        }
    }
}