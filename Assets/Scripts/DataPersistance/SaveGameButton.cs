using UnityEngine;

public class SaveGameButton : MonoBehaviour
{
    public void SaveGameManually()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGame();
            Debug.Log("Game saved manually via button.");
        }
        else
        {
            Debug.LogError("DataPersistenceManager instance not found. Ensure it's marked DontDestroyOnLoad.");
        }
    }
}