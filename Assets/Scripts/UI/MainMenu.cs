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

    public void StartNewGame()
    {
        SceneManager.LoadScene(startGameGameScene.ToString());
        // SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
        // SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
    }
}