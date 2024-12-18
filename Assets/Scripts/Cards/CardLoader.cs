using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UI;

public class CardLoader : MonoBehaviour
{
    public GameObject cardPrefab;   // Assign your card prefab in the Inspector
    public Transform cardParent;    // Assign the parent transform (e.g., the content object in your scrollable grid)
    public Vector3  localCardTransform= new Vector3(4f, 4f, 1f);    // Assign the parent transform (e.g., the content object in your scrollable grid)
    public Sprite DefaultSprite; // Assign a placeholder sprite in the inspector

    private List<Card> creatureCards = new List<Card>();
    private List<int> creatureCardIDs = new List<int>();
    private List<string> creatureCardNames = new List<string>();

    void Awake()
    {
        LoadCardsFromCSV();
        InstantiateCreatureCards();

    }
    void LoadCardsFromCSV()
    {
        TextAsset csvData = Resources.Load<TextAsset>("cards"); // Load CSV
        if (csvData == null)
        {
            Debug.LogError("CSV file not found in Resources folder!");
            return;
        }

        StringReader reader = new StringReader(csvData.text);
        string headerLine = reader.ReadLine(); // Skip the header

        while (true)
        {
            string line = reader.ReadLine();
            if (line == null) break;

            // string[] fields = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            string[] fields = line.Split(','); // Change '\t' to ',' if using commas

            if (fields.Length < 9) continue;
            Card card = new Card
            {
                ID = int.TryParse(fields[0], out int id) ? id : 0,
                Name = fields[1],
                Type = fields[2],
                Mana = int.TryParse(fields[3], out int mana) ? mana : 0,
                TPower = int.TryParse(fields[4], out int tPower) ? tPower : 0,
                PPower = int.TryParse(fields[5], out int pPower) ? pPower : 0,
                BPower = int.TryParse(fields[6], out int bPower) ? bPower : 0,
                CasualPower = int.TryParse(fields[7], out int casualPower) ? casualPower : 0,
                Description = fields[8],
                CardSprite = DefaultSprite // Use the placeholder sprite if Resources.Load fails
            };

            // Load the sprite dynamically using ID + Name
            string spritePath = $"Sprites/Cards/Creatures/{card.ID} {card.Name}";
            print(spritePath);
            card.CardSprite = Resources.Load<Sprite>(spritePath);

            if (card.Type.ToLower() == "creature")
            {
                creatureCards.Add(card);
            }
        }

        reader.Close();
    }

    public void InstantiateCreatureCards()
    {
        foreach (Card card in creatureCards)
        {
            GameObject newCard = Instantiate(cardPrefab, cardParent);
            // Assume the card prefab has a script named CardDisplay to handle displaying the card's data

            newCard.transform.localScale = localCardTransform;

            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.Setup(card);
            }
            else
            {
                Debug.LogWarning("Card prefab does not have a CardDisplay component.");
            }
        }
    }

}
