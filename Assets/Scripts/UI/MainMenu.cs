using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameScene startGameGameScene;

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

    public void StartNewGame()
    {
        DataPersistenceManager.instance.NewGame();

        SceneManager.LoadSceneAsync(startGameGameScene.ToString());
    }
}
// [SerializeField] string nameEssentialScene;
// [SerializeField] string nameNewGameStartScene;
// SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
// SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);