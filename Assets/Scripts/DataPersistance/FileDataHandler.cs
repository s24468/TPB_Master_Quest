using UnityEngine;
using System;
using System.IO;

namespace DataPersistance
{
    
    public class FileDataHandler
    {
        private string dataDirectoryPath = "";
        private string dataFileName = "";

        public FileDataHandler(string dataDirectoryPath, string dataFileName)
        {
            this.dataDirectoryPath = dataDirectoryPath;
            this.dataFileName = dataFileName;
        }

        public GameData Load()
        {
            
            string fullPath = Path.Combine(this.dataDirectoryPath, dataFileName);
            GameData loadedData = null;
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream fs = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            dataToLoad = sr.ReadToEnd();
                            Debug.Log(dataToLoad);
                        }
                    }
                    
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
            return loadedData;
        }

        public void Save(GameData data)
        {
            string fullPath = Path.Combine(this.dataDirectoryPath, dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(data,true);
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to save data to file: " + fullPath + "\n" + e);
            }
        }
    }
}