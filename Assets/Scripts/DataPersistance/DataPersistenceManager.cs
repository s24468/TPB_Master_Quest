using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DataPersistance;
using NUnit.Framework;

public class DataPersistenceManager : MonoBehaviour
{
    
    [Header("File Storage Configuration")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption = false;

    
    public GameData gameData;
    private List<IDataPersistence> dataPersistenceObject;
    public static DataPersistenceManager instance { get; private set; }
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }

        instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObject = GetAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No Game Data loaded.");
            NewGame();
        }

        foreach (var dataPersistenceObj in dataPersistenceObject)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log("Game Data loaded." + gameData._money);
    }

    public void SaveGame()
    {
        foreach (var dataPersistenceObj in dataPersistenceObject)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        Debug.Log("Game Data saved." + gameData._money);
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> GetAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        // return dataPersistenceObjects.ToList();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}