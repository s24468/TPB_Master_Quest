using UnityEngine;

namespace UI
{
    public class QuitGameButton: MonoBehaviour
    {
      public  void Quit()
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
}