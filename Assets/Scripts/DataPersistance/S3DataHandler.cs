using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace DataPersistance
{
    public class S3DataHandler
    {
        private readonly string bucketName = "mygame-cards-storage";
        private readonly AmazonS3Client s3Client;

        public S3DataHandler()
        {
            string accessKey = "AKIAVYV52E7EM2ERQJBQ";
            string secretKey = "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";
            var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            var config = new AmazonS3Config { RegionEndpoint = Amazon.RegionEndpoint.EUNorth1 };
            s3Client = new AmazonS3Client(credentials, config);
        }

        public async Task<GameData> LoadAsync(string uniqueID)
        {
            string fileName = $"PlayersData{uniqueID}.json";
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                var response = await s3Client.GetObjectAsync(request);

                using (var reader = new StreamReader(response.ResponseStream))
                {
                    string json = await reader.ReadToEndAsync();
                    return JsonUtility.FromJson<GameData>(json);
                }
            }
            catch (AmazonS3Exception ex)
            {
                Debug.LogError($"Error fetching data from S3: {ex.Message}");
                return null;
            }
        }

        public async Task SaveAsync(string uniqueID, GameData data)
        {
            string fileName = $"PlayersData{uniqueID}.json";
            string json = JsonUtility.ToJson(data, true);
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    ContentBody = json,
                    ContentType = "application/json"
                };
                var response = await s3Client.PutObjectAsync(request);
            }
            catch (AmazonS3Exception ex)
            {
                Debug.LogError($"Error saving data to S3: {ex.Message}");
            }
        }
    }
}