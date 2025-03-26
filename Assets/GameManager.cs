using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame(int generations)
    {
        PlayerPrefs.SetInt("Generations", generations);
        SceneManager.LoadScene("save"); // Load the 'save' scene
        SceneManager.sceneLoaded += OnSceneLoaded; // Register for scene load event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "save")
        {
            // Find the GridManager and call its CreateGrid method
            GridManager gridManager = FindObjectOfType<GridManager>();
            if (gridManager != null)
            {
                gridManager.CreateGrid(); // Ensure the grid is created when the scene is ready
            }
        }
    }
}
