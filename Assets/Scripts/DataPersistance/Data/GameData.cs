using DataPersistance.SerializeableTypes;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    public int money;
    public string nickname;
    public Vector3 playerPosition;
    public SerializableDictionary<string, int> CardDictionaryCollected;

    public GameData()
    {
        money = 600;
        nickname = "";
        playerPosition = Vector3.zero;
        // CardDictionaryCollected = new SerializableDictionary<string, int>();
        CardDictionaryCollected = new SerializableDictionary<string, int>
        {
            { "32", 4 },
            { "22", 2 },
            { "33", 2 },
            { "37", 3 },
            { "12", 2 },
            { "19", 2 },
            { "43", 2 },
            { "35", 4 },
            { "15", 1 },
            { "28", 1 },
            { "20", 2 },
            { "3", 1 },
            { "11", 2 },
            { "42", 2 },
            { "14", 3 },
            { "2", 2 },
            { "36", 1 },
            { "7", 1 },
            { "23", 1 },
            { "5", 1 },
            { "18", 1 }
        };
    }
    /// <summary>
    /// Get the number of owned cards for the given card ID.
    /// </summary>
    /// <param name="cardId">The ID of the card as a string.</param>
    /// <returns>The number of owned cards, or 0 if the card is not found.</returns>
    public int GetNumberOfOwnedCards(string cardId)
    {
        if (CardDictionaryCollected.ContainsKey(cardId))
        {
            return CardDictionaryCollected[cardId];
        }

        return 0; // Default to 0 if the card ID doesn't exist in the dictionary
    }
}