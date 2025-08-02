using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LevelTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float levelDuration = 30f; // Длительность уровня в секундах
    [SerializeField] private bool showTimer = true;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    
    private float currentTime;
    private bool isTimerRunning = false;
    
    void Start()
    {
        Debug.Log($"LevelTimer Start called on {gameObject.name} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        
        currentTime = levelDuration;
        isTimerRunning = true;
        
        // Создаем UI для таймера если его нет
        if (showTimer && timerText == null)
        {
            CreateTimerUI();
        }
    }
    
    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            
            // Обновляем UI
            if (showTimer && timerText != null)
            {
                UpdateTimerDisplay();
            }
            
            // Проверяем окончание времени
            if (currentTime <= 0)
            {
                OnTimeUp();
            }
        }
    }
    
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    private void CreateTimerUI()
    {
        // Создаем Canvas если его нет
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("TimerCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }
        
        // Создаем текст для таймера
        GameObject timerGO = new GameObject("TimerText");
        timerGO.transform.SetParent(canvas.transform, false);
        
        timerText = timerGO.AddComponent<TextMeshProUGUI>();
        timerText.text = "30:00";
        timerText.fontSize = 36;
        timerText.color = Color.white;
        timerText.alignment = TextAlignmentOptions.Center;
        
        // Позиционируем в правом верхнем углу
        RectTransform rectTransform = timerText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1);
        rectTransform.anchoredPosition = new Vector2(-20, -20);
        rectTransform.sizeDelta = new Vector2(200, 50);
    }
    
    private void OnTimeUp()
    {
        isTimerRunning = false;
        Debug.Log("Time's up! Level failed.");
        
        // Вызываем проигрыш через GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelFailed();
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }
    }
    
    // Метод для остановки таймера (при победе)
    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Timer stopped");
    }
    
    // Метод для получения оставшегося времени
    public float GetRemainingTime()
    {
        return currentTime;
    }
} 