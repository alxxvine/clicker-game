using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Auto Setup")]
    [SerializeField] private bool autoCreateSceneController = true;
    [SerializeField] private bool autoCreateGameManager = true;
    
    void Awake()
    {
        Debug.Log($"GameInitializer Awake called on {gameObject.name} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        
        // Автоматически создаем SceneController если его нет
        if (autoCreateSceneController && SceneController.Instance == null)
        {
            Debug.Log("Creating SceneController...");
            GameObject sceneControllerGO = new GameObject("SceneController");
            SceneController sceneController = sceneControllerGO.AddComponent<SceneController>();
        }
        
        // Автоматически создаем GameManager если его нет
        if (autoCreateGameManager && GameManager.Instance == null)
        {
            Debug.Log("Creating GameManager...");
            GameObject gameManagerGO = new GameObject("GameManager");
            GameManager gameManager = gameManagerGO.AddComponent<GameManager>();
        }
    }
} 