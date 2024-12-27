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


    public void LoadData(GameData data)
    {
        _cardCollection = data.CardDictionaryCollected;
    }

    public void SaveData(ref GameData data)
    {
        data.CardDictionaryCollected = _cardCollection;
    }
}