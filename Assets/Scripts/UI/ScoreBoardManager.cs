using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab; // A prefab for a single row (with TMP components)
    [SerializeField] private Transform tableContainer; // The Panel with Vertical Layout Group

    public void PopulateScoreboard(List<GameData> gameDataList)
    {
        // Clear existing rows
        foreach (Transform child in tableContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate rows dynamically
        foreach (GameData data in gameDataList)
        {
            GameObject newRow = Instantiate(rowPrefab, tableContainer);

            TextMeshProUGUI[] columns = newRow.GetComponentsInChildren<TextMeshProUGUI>();
            columns[0].text = data.nickname; // First column: Nickname
            columns[1].text = data.money.ToString(); // Second column: Money
            columns[2].text = data.CardDictionaryCollected.keys.Count.ToString(); // Third column: Unlocked Cards
        }
    }
}