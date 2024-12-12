using System.Collections.Generic;
using DataPersistance.SerializeableTypes;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int _money;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> CardDictionaryCollected;

    public GameData()
    {
        _money = 0;
        playerPosition = Vector3.zero;
        CardDictionaryCollected = new SerializableDictionary<string, bool>();
    }
}