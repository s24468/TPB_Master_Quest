using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneManager : MonoBehaviour
{
    [FormerlySerializedAs("startGameGameScene")] [SerializeField]
    GameScene gameScene;
    // [SerializeField] private string gameSceneName; // Assign the game scene name in Unity

    public void ExitGame()
    {
        Application.Quit();
        // Note: This won't work in the editor. To test in the editor:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void LoadSave()
    {
        // Save game to memory before loading a new scene
        DataPersistenceManager.instance.SaveGameToMemory();
        // Load the saved game scene
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameScene.ToString());
    }

    public void StartNewGame()
    {
        DataPersistenceManager.instance.NewGame();

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameScene.ToString());
    }

    public void ChangeScene(string sceneName)
    {
        // Save game to memory before changing scenes
        DataPersistenceManager.instance.SaveGameToMemory();

        // Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        Debug.Log($"Changing scene to: {sceneName}");
    }
}