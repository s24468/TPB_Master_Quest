using UnityEngine;

public class QuitGame : MonoBehaviour
{
    private static QuitGame instance;

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Check if the Q key is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Quit();
        }
    }

    void Quit()
    {
        Debug.Log("Quitting the game...");
        
        // Quit the application
        Application.Quit();

        // Note: This won't work in the editor. To test in the editor:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}