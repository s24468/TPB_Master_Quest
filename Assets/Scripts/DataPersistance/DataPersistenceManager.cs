using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DataPersistance;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Configuration")] [SerializeField]
    private string fileName;

    private S3DataHandler s3DataHandler;
    public GameData gameData;


    private List<IDataPersistence> dataPersistenceObject;
    public static DataPersistenceManager instance { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Data Persistence Manager in the scene.");
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private async void Start()
    {
        // s3DataHandler = new S3DataHandler();
        await LoadGame();
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObject = GetAllDataPersistenceObjects();
        LoadGameInGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGameToMemory();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void SaveGameToMemory()
    {
        foreach (var dataPersistenceObj in dataPersistenceObject)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
    }

    public void LoadGameInGame()
    {
        foreach (var dataPersistenceObj in dataPersistenceObject)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public async Task LoadGame()
    {
        if (s3DataHandler == null)
        {
            Debug.Log("S3DataHandler not initialized.");
            return;
        }

        string uniqueID = SystemInfo.deviceUniqueIdentifier;
        // gameData = await s3DataHandler.LoadAsync(uniqueID);
        gameData = null;

        if (gameData == null)
        {
            Debug.Log("No Game Data found in S3. Creating a new game.");
            NewGame();
        }

        foreach (var dataPersistenceObj in dataPersistenceObject)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public async void SaveGame()
    {
        foreach (var dataPersistenceObj in dataPersistenceObject)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        if (s3DataHandler != null)
        {
            // string uniqueID = SystemInfo.deviceUniqueIdentifier;
            // await s3DataHandler.SaveAsync(uniqueID, gameData);
        }
        else
        {
            Debug.LogError("S3DataHandler not initialized. Cannot save data.");
        }
    }


    // private void OnApplicationQuit()
    // {
    //     SaveGame();
    // }

    private List<IDataPersistence> GetAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}