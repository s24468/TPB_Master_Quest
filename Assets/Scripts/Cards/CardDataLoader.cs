using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI;

public class CardDataLoader : MonoBehaviour
{
    public static CardDataLoader Instance; // Singleton do łatwego dostępu do danych
    private List<Card> creatureCards = new List<Card>();

    private string s3CsvUrl = "https://mygame-cards-storage.s3.eu-north-1.amazonaws.com/DatabaseCards.csv";

    public Sprite DefaultSprite; // Assign a placeholder sprite in the inspector


    void Awake()
    {
        // Ustawienie singletona
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Utrzymuje obiekt między scenami
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Załaduj dane na początku działania aplikacji
        StartCoroutine(DownloadCSVFromS3());
    }

    IEnumerator DownloadCSVFromS3()
    {
        UnityWebRequest request = UnityWebRequest.Get(s3CsvUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string csvData = request.downloadHandler.text;
            Debug.Log("Pobrano dane CSV z S3:\n" + csvData);
            LoadCardsFromCSV(csvData);
        }
        else
        {
            Debug.LogError("Błąd pobierania pliku z S3: " + request.error);
        }
    }

    void LoadCardsFromCSV(string csvContent)
    {
        StringReader reader = new StringReader(csvContent);
        string headerLine = reader.ReadLine(); // Skip header

        while (true)
        {
            string line = reader.ReadLine();
            if (line == null) break;

            // string[] fields = line.Split(',');
            string[] fields = ParseCSVLine(line);
            ;
            Debug.Log(fields[0] + fields[1] + fields[2] + fields[3] + fields[4] + fields[5] + fields[6] + fields[7] +
                      fields[8] + fields[9] + fields[10]);

            // if (fields.Length < 9) continue;
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
                Cost = int.TryParse(fields[9], out int cost) ? cost : 0,
                Abilities = fields[10],
                CardSprite = DefaultSprite
            };
// Wczytaj sprite z katalogu Resources, jeśli istnieje
            string spritePath = $"Sprites/Cards/Creatures/{card.ID} {card.Name}";
            Sprite loadedSprite = Resources.Load<Sprite>(spritePath);
            if (loadedSprite != null) card.CardSprite = loadedSprite;

            if (card.Type.ToLower() == "creature")
            {
                creatureCards.Add(card);
            }
        }

        reader.Close();
        Debug.Log("Załadowano karty: " + creatureCards.Count);
    }

    private string[] ParseCSVLine(string line)
    {
        if (!line.Contains("\""))
        {
            return line.Split(','); // Simple split for lines without quotes
        }

        // Parse the line while respecting quotes
        // List<string> fields = new List<string>();
        //          bool insideQuote = false;
        //          string currentField = "";
        //  
        //          foreach (char c in line)
        //          {
        //              if (c == '"' && insideQuote)
        //              {
        //                  insideQuote = false; // Close quote
        //              }
        //              else if (c == '"' && !insideQuote)
        //              {
        //                  insideQuote = true; // Open quote
        //              }
        //              else if (c == ',' && !insideQuote)
        //              {
        //                  fields.Add(currentField);
        //                  currentField = "";
        //              }
        //              else
        //              {
        //                  currentField += c;
        //              }
        //          }
        // fields.Add(currentField); // Add the last field

        List<string> fields = new List<string>();
        bool insideQuote = false;
        var currentField = new System.Text.StringBuilder();

        foreach (char c in line)
        {
            switch (c)
            {
                //xyz,"abc",123
                case '"':
                    insideQuote = !insideQuote; // Toggle the quote state
                    break;

                case ',' when !insideQuote:
                    fields.Add(currentField.ToString().Trim()); // Add the field
                    currentField.Clear(); // Reset the field
                    break;

                default:
                    currentField.Append(c); // Add the character to the current field
                    break;
            }
        }

        // Add the last field
        fields.Add(currentField.ToString().Trim());

        return fields.ToArray();
    }

    // Publiczny dostęp do danych kart
    public List<Card> GetCreatureCards()
    {
        return creatureCards;
    }
}