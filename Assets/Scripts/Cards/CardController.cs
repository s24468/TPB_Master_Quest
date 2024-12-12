using System;
using DataPersistance;
using UnityEngine;

namespace Cards
{
    public class CardController: MonoBehaviour, IDataPersistence
    {
        [SerializeField] private string id;
        private bool owned = false;

        //If you want to create unique id, in editor u need to click an instance of a class (Object) and click RMB on id
        [ContextMenu("Generate guid for id")]
        private void GenerateGuid()
        {
            id = Guid.NewGuid().ToString();
        }

        public void LoadData(GameData data)
        {
            data.CardDictionaryCollected.TryGetValue(id, out owned);
            if (owned)
            {
                //make sth that means player owns the card
            }
        }

        public void SaveData(ref GameData data)
        {
            if (data.CardDictionaryCollected.ContainsKey(id))
            {
                data.CardDictionaryCollected.Remove(id);
            }
            data.CardDictionaryCollected.Add(id, owned);
        }
    }
}