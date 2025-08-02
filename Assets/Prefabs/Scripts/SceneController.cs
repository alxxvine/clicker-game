using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string menuSceneName = "Menu";
    [SerializeField] private string level1SceneName = "Level 1";
    [SerializeField] private string winSceneName = "Win";
    [SerializeField] private string loseSceneName = "Lose";
    
    // Singleton pattern
    public static SceneController Instance { get; private set; }
    
    void Awake()
    {
        Debug.Log($"SceneController Awake called on {gameObject.name} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        Debug.Log($"SceneController stack trace: {System.Environment.StackTrace}");
        
        // Singleton setup
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
    
    void Start()
    {
        // Если это первая сцена (Menu), то не уничтожаем SceneController
        if (SceneManager.GetActiveScene().name == menuSceneName)
        {
            DontDestroyOnLoad(gameObject);
        }
        
        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDestroy()
    {
        // Отписываемся от события
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Очищаем старые UI элементы при загрузке новой сцены
        CleanupOldUI();
    }
    
    void CleanupOldUI()
    {
        // Удаляем старые GameManager объекты (кроме текущего)
        GameManager[] gameManagers = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
        if (gameManagers.Length > 1)
        {
            for (int i = 1; i < gameManagers.Length; i++)
            {
                DestroyImmediate(gameManagers[i].gameObject);
            }
        }
        // Блок с UIManager удалён
    }
    
    // Переход в меню
    public void LoadMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
    
    // Переход к первому уровню
    public void LoadLevel1()
    {
        SceneManager.LoadScene(level1SceneName);
    }
    
    // Переход к сцене победы
    public void LoadWinScene()
    {
        SceneManager.LoadScene(winSceneName);
    }
    
    // Переход к сцене проигрыша
    public void LoadLoseScene()
    {
        SceneManager.LoadScene(loseSceneName);
    }
    
    // Перезапуск текущего уровня
    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Загрузка кастомной сцены по названию
    public void LoadCustomScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    // Выход из игры
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 