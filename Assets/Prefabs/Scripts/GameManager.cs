using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    
    [Header("Level Settings")]
    [SerializeField] private int startingScore = 0;
    [SerializeField] private int targetScore = 50;
    [SerializeField] private VictoryScene victoryScene = VictoryScene.Win;
    
    public enum VictoryScene
    {
        Win,        // Сцена победы
        Menu,       // Главное меню
        Level1,     // Повторить уровень
        NextLevel   // Следующий уровень (пока что тоже Level1)
    }
    
    private int currentScore = 0;
    private bool levelCompleted = false;
    
    // Singleton pattern
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        Debug.Log($"GameManager Awake called on {gameObject.name} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        Debug.Log($"GameManager stack trace: {System.Environment.StackTrace}");
        
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            // Убираем DontDestroyOnLoad - GameManager должен пересоздаваться для каждого уровня
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Сбрасываем состояние при каждом запуске уровня
        ResetLevel();
        
        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        Debug.Log($"GameManager started on scene: {SceneManager.GetActiveScene().name}");
    }
    
    void OnDestroy()
    {
        // Отписываемся от события
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("GameManager destroyed");
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        // Если это уровень, сбрасываем состояние
        if (scene.name == "Level 1")
        {
            // Небольшая задержка для гарантии, что все компоненты инициализированы
            Invoke(nameof(ResetLevel), 0.1f);
        }
    }
    
    public void AddPoints(int points)
    {
        if (levelCompleted) return; // Не начисляем очки если уровень завершен
        
        currentScore += points;
        UpdateScoreDisplay();
        
        // Проверяем завершение уровня
        if (currentScore >= targetScore && !levelCompleted)
        {
            levelCompleted = true;
            OnLevelCompleted();
        }
    }
    
    public void ResetLevel()
    {
        Debug.Log($"Resetting level. Old score: {currentScore}/{targetScore}");
        currentScore = startingScore;
        levelCompleted = false;
        UpdateScoreDisplay();
        Debug.Log($"Level reset complete. New score: {currentScore}/{targetScore}");
    }
    
    public void SetTargetScore(int target)
    {
        targetScore = target;
        UpdateScoreDisplay();
    }
    
    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    public int GetTargetScore()
    {
        return targetScore;
    }
    
    public bool IsLevelCompleted()
    {
        return levelCompleted;
    }
    
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{currentScore}/{targetScore}";
        }
    }
    
    private void OnLevelCompleted()
    {
        Debug.Log("Level completed! Score: " + currentScore + "/" + targetScore);
        
        // Останавливаем таймер при победе
        LevelTimer levelTimer = FindObjectOfType<LevelTimer>();
        if (levelTimer != null)
        {
            levelTimer.StopTimer();
        }
        
        // Проверяем и создаем SceneController если его нет
        if (SceneController.Instance == null)
        {
            Debug.Log("SceneController not found, creating one...");
            GameObject sceneControllerGO = new GameObject("SceneController");
            SceneController sceneController = sceneControllerGO.AddComponent<SceneController>();
        }
        
        // Переходим к выбранной сцене
        if (SceneController.Instance != null)
        {
            switch (victoryScene)
            {
                case VictoryScene.Win:
                    SceneController.Instance.LoadWinScene();
                    break;
                case VictoryScene.Menu:
                    SceneController.Instance.LoadMenu();
                    break;
                case VictoryScene.Level1:
                    SceneController.Instance.LoadLevel1();
                    break;
                case VictoryScene.NextLevel:
                    // Пока что тоже Level1, но в будущем можно расширить
                    SceneController.Instance.LoadLevel1();
                    break;
            }
        }
        else
        {
            Debug.LogError("Failed to create SceneController!");
        }
    }
    
    // Метод для обработки проигрыша
    public void OnLevelFailed()
    {
        Debug.Log("Level failed! Adding click zone radius bonus...");
        
        // Добавляем бонусные очки при проигрыше
        if (PlayerProgress.Instance != null)
        {
            PlayerProgress.Instance.AddClickZoneRadiusBonus(50);
        }
        else
        {
            // Создаем PlayerProgress если его нет
            Debug.Log("PlayerProgress not found, creating one...");
            GameObject playerProgressGO = new GameObject("PlayerProgress");
            PlayerProgress playerProgress = playerProgressGO.AddComponent<PlayerProgress>();
            playerProgress.AddClickZoneRadiusBonus(50);
        }
        
        // Переходим к сцене проигрыша
        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadLoseScene();
        }
        else
        {
            Debug.LogError("SceneController not found!");
        }
    }
    
    // Методы для установки UI элементов (если они создаются динамически)
    public void SetScoreText(TextMeshProUGUI text)
    {
        scoreText = text;
        UpdateScoreDisplay();
    }
    
    public void SetLevelText(TextMeshProUGUI text)
    {
        levelText = text;
        UpdateScoreDisplay();
    }
    
    public bool HasUIElements()
    {
        return scoreText != null || levelText != null;
    }
}