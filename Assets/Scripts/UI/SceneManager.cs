using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneManager : MonoBehaviour
{
    [FormerlySerializedAs("startGameGameScene")] [SerializeField] GameScene gameScene;

    public void ExitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void LoadSave()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameScene.ToString());
        Debug.Log("Loading save file...");
    }

    public void StartNewGame()
    {
        DataPersistenceManager.instance.NewGame();

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameScene.ToString());
    }
}
// [SerializeField] string nameEssentialScene;
// [SerializeField] string nameNewGameStartScene;
// SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
// SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);