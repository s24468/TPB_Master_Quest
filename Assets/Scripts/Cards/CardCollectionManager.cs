using DataPersistance;
using DataPersistance.SerializeableTypes;
using UnityEngine;

public class CardCollectionManager : MonoBehaviour, IDataPersistence
{
    private SerializableDictionary<string, int> _cardCollection;

    private void Awake()
    {
        _cardCollection = new SerializableDictionary<string, int>();
    }

    public void AddCard(string cardId)
    {
        if (_cardCollection.ContainsKey(cardId))
        {
            _cardCollection[cardId]++;
        }
        else
        {
            _cardCollection[cardId] = 1;
        }
    }

    public void LoadData(GameData data)
    {
        _cardCollection = data.CardDictionaryCollected;
        Debug.Log("CardCollectionManager: Loaded card data.");
    }

    public void SaveData(ref GameData data)
    {
        data.CardDictionaryCollected = _cardCollection;
        Debug.Log("CardCollectionManager: Saved card data.");
    }

    public int GetCardCount(string cardId)
    {
        if (_cardCollection.ContainsKey(cardId))
        {
            return _cardCollection[cardId];
        }
        return 0;
    }
}