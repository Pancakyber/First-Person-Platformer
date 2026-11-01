using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonManager : MonoBehaviour
{
    // Singleton Instance
    public static buttonManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure only one instance exists in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Optional: Keep this manager alive across scenes
        // DontDestroyOnLoad(gameObject); 
    }

    // --- Action Methods ---

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void ToggleGameObject(GameObject target)
    {
        if (target != null)
        {
            target.SetActive(!target.activeSelf);
        }
    }
    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
