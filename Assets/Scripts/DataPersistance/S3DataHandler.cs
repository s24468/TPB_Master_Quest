using System;
using System.IO;
using UnityEngine;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

// namespace DataPersistance;

namespace DataPersistance
{
   
    public class S3DataHandler
    {
        private string s3Url = "https://mygame-cards-storage.s3.eu-north-1.amazonaws.com/PlayersData.json";

        public async Task<GameData> LoadAsync()
        {
            Debug.Log($"Attempting to load data from S3: {s3Url}");
            GameData loadedData = null;

            using (UnityWebRequest request = UnityWebRequest.Get(s3Url))
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Successfully fetched JSON data.");
                    string json = request.downloadHandler.text;
                    loadedData = JsonUtility.FromJson<GameData>(json);
                }
                else
                {
                    Debug.LogError($"Error fetching data from S3: {request.error}");
                }
            }

            return loadedData;
        }

        public async Task SaveAsync(GameData data)
        {
            Debug.Log("Saving data to S3...");
            string json = JsonUtility.ToJson(data, true);

            using (UnityWebRequest request = UnityWebRequest.Put(s3Url, json))
            {
                request.method = "PUT";
                request.SetRequestHeader("Content-Type", "application/json");

                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Successfully saved data to S3.");
                }
                else
                {
                    Debug.LogError($"Error saving data to S3: {request.error}");
                }
            }
        }
    }
}