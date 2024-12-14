using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameScene startGameGameScene;
    // [SerializeField] string nameEssentialScene;
    // [SerializeField] string nameNewGameStartScene;

    public void ExitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void LoadSave()
    {
        SceneManager.LoadSceneAsync(startGameGameScene.ToString());
        Debug.Log("Loading save file...");
    }

    public void StartNewSave()
    {
        // SceneManager.LoadSceneAsync()
        // DataPersistenceManager.instance.NewGame();
        //
        // SceneManager.LoadSceneAsync(startGameGameScene.ToString());
    }


    public void StartNewGame()
    {
        DataPersistenceManager.instance.NewGame();

        SceneManager.LoadSceneAsync(startGameGameScene.ToString());
        // SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
        // SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
    }
}