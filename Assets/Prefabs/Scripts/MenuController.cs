using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Menu Settings")]
    [SerializeField] private bool clickAnywhereToStart = true;
    [SerializeField] private SceneName targetScene = SceneName.Menu;
    
    public enum SceneName
    {
        Menu,
        Level1,
        Win,
        Lose
    }
    
    void Start()
    {
        Debug.Log($"MenuController Start called on {gameObject.name} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        
        // Убеждаемся, что SceneController существует
        if (SceneController.Instance == null)
        {
            Debug.Log("Creating SceneController...");
            GameObject sceneControllerGO = new GameObject("SceneController");
            sceneControllerGO.AddComponent<SceneController>();
        }
        else
        {
            Debug.Log("SceneController already exists");
        }
    }
    
    void Update()
    {
        // Обработка клика в любом месте экрана
        if (clickAnywhereToStart && Input.GetMouseButtonDown(0))
        {
            LoadTargetScene();
        }
    }
    
    // Метод для загрузки целевой сцены
    public void LoadTargetScene()
    {
        // Отключаем автоматический переход если он активен
        DisableAutoTransition();
        
        if (SceneController.Instance != null)
        {
            switch (targetScene)
            {
                case SceneName.Menu:
                    SceneController.Instance.LoadMenu();
                    break;
                case SceneName.Level1:
                    SceneController.Instance.LoadLevel1();
                    break;
                case SceneName.Win:
                    SceneController.Instance.LoadWinScene();
                    break;
                case SceneName.Lose:
                    SceneController.Instance.LoadLoseScene();
                    break;
            }
        }
        else
        {
            Debug.LogError("SceneController not found!");
        }
    }
    
    private void DisableAutoTransition()
    {
        AutoMenuTransition autoTransition = FindObjectOfType<AutoMenuTransition>();
        if (autoTransition != null)
        {
            autoTransition.DisableAutoTransition();
        }
    }
    
    // Метод для кнопки "Выход"
    public void QuitGame()
    {
        // Отключаем автоматический переход если он активен
        DisableAutoTransition();
        
        if (SceneController.Instance != null)
        {
            SceneController.Instance.QuitGame();
        }
        else
        {
            Debug.LogError("SceneController not found!");
        }
    }
    
    // Метод для кнопки "Повторить" (для сцены Lose)
    public void RestartLevel()
    {
        // Отключаем автоматический переход если он активен
        DisableAutoTransition();
        
        if (SceneController.Instance != null)
        {
            SceneController.Instance.RestartCurrentLevel();
        }
        else
        {
            Debug.LogError("SceneController not found!");
        }
    }
} 