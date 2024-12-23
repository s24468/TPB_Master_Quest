using System;
using Amazon.S3;
using Amazon.S3.Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class S3BucketAccess : MonoBehaviour
{
    private string bucketName = "mygame-cards-storage";
    private AmazonS3Client s3Client;

    private string combinedJsonData = ""; // To store all JSON data

    [SerializeField] private TextMeshProUGUI Column1; // Reference to TextMeshPro object
    [SerializeField] private TextMeshProUGUI Column2; // Reference to TextMeshPro object
    [SerializeField] private TextMeshProUGUI Column3; // Reference to TextMeshPro object

    void Start()
    {
        string accessKey = "AKIAVYV52E7EM2ERQJBQ";
        string secretKey = "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";

        s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.EUNorth1);
        StartCoroutine(ListObjectsInBucket());
        // StartCoroutine(SaveFormattedGameData());
    }

    private IEnumerator ListObjectsInBucket()
    {
        ListObjectsV2Request request = new ListObjectsV2Request
        {
            BucketName = bucketName
        };

        var responseTask = s3Client.ListObjectsV2Async(request);
        yield return new WaitUntil(() => responseTask.IsCompleted);

        if (responseTask.Exception != null)
        {
            Debug.LogError("Error listing objects: " + responseTask.Exception.Message);
            yield break;
        }

        ListObjectsV2Response response = responseTask.Result;

        foreach (S3Object entry in response.S3Objects)
        {
            Debug.Log("Key: " + entry.Key);

            // Fetch file content if JSON
            if (entry.Key.EndsWith(".json"))
            {
                yield return StartCoroutine(FetchFileContent(entry.Key));
            }
        }

        setScoreBoard(combinedJsonData);
    }

    private void setScoreBoard(string combinedJsonData)
    {
        string jsonArray = $"[{combinedJsonData.Replace("}\n\n{", "},{")}]";
        GameData[] gameDataArray = JsonUtility.FromJson<GameDataArrayWrapper>($"{{\"gameDataArray\":{jsonArray}}}")
            .gameDataArray;
        Column1.text = "<color=red><size=30>Nickname</size></color>\n\n";
        Column2.text = "<color=red><size=30>Money</size></color>\n\n";
        Column3.text = "<color=red><size=30>Cards</size></color>\n\n";
        // Build the table rows
        foreach (GameData jsonData in gameDataArray)
        {
            string randomColor = GetRandomColor();
            string nickname = jsonData.nickname;
            int money = jsonData.money;
            int unlockedCards = jsonData.CardDictionaryCollected.keys.Count;
            Column1.text += $"<color={randomColor}>{nickname}</color>\n";
            Column2.text += $"<color={randomColor}>{money}</color>\n";
            Column3.text += $"<color={randomColor}>{unlockedCards}</color>\n";
        }
    }
    string GetRandomColor()
    {
        // Generate random RGB values
        int r = Random.Range(0, 256);
        int g = Random.Range(0, 256);
        int b = Random.Range(0, 256);
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    private IEnumerator FetchFileContent(string fileKey)
    {
        GetObjectRequest getRequest = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = fileKey
        };

        var getResponseTask = s3Client.GetObjectAsync(getRequest);
        yield return new WaitUntil(() => getResponseTask.IsCompleted);

        if (getResponseTask.Exception != null)
        {
            Debug.LogError("Error fetching file content: " + getResponseTask.Exception.Message);
            yield break;
        }

        GetObjectResponse getResponse = getResponseTask.Result;
        using (StreamReader reader = new StreamReader(getResponse.ResponseStream))
        {
            string content = reader.ReadToEnd();
            combinedJsonData += content + "\n\n"; // Concatenate each JSON file's content
        }
    }

    private IEnumerator SaveDataToS3(string fileName, string content)
    {
        PutObjectRequest request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileName,
            ContentBody = content,
            ContentType = "application/json",
            CannedACL = S3CannedACL.BucketOwnerFullControl // Ensures bucket owner control
        };

        var putResponseTask = s3Client.PutObjectAsync(request);
        yield return new WaitUntil(() => putResponseTask.IsCompleted);

        if (putResponseTask.Exception != null)
        {
            Debug.LogError($"Error saving file: {fileName}. Exception: {putResponseTask.Exception.Message}");
            yield break;
        }

        Debug.Log($"Successfully saved file: {fileName}");
    }

    private IEnumerator SaveFormattedGameData()
    {
        string fileName = "ExampleGameData.json";

        // Construct example game data
        var gameData = new ExampleGameData
        {
            money = 1000,
            nickname = "ExamplePlayer",
            playerPosition = new ExampleGameData.PlayerPosition { x = 10.5f, y = 20.3f, z = 5.7f },
            CardDictionaryCollected = new ExampleGameData.CardDictionary
            {
                keys = new string[] { "12", "45", "67", "89", "101" },
                values = new int[] { 3, 2, 5, 1, 10 }
            }
        };

        // Serialize to JSON
        string content = JsonUtility.ToJson(gameData, true);

        Debug.Log($"Formatted JSON to save:\n{content}");

        // Save to S3
        yield return StartCoroutine(SaveDataToS3(fileName, content));
    }
}

[System.Serializable]
public class ExampleGameData
{
    public int money;
    public string nickname;
    public PlayerPosition playerPosition;
    public CardDictionary CardDictionaryCollected;

    [System.Serializable]
    public class PlayerPosition
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class CardDictionary
    {
        public string[] keys;
        public int[] values;
    }
}

[System.Serializable]
public class GameDataArrayWrapper
{
    public GameData[] gameDataArray;
}