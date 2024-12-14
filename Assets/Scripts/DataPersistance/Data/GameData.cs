using System.Collections.Generic;
using DataPersistance.SerializeableTypes;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    [FormerlySerializedAs("_money")] public int money;
    public string nickname;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> CardDictionaryCollected;

    public GameData()
    {
        money = 0;
        nickname = "";
        playerPosition = Vector3.zero;
        CardDictionaryCollected = new SerializableDictionary<string, bool>();
    }
}