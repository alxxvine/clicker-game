using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusCounterUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI bonusCounterText;
    
    void Start()
    {
        Debug.Log($"BonusCounterUI Start called on {gameObject.name} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        
        // Проверяем что UI поля назначены
        if (bonusCounterText == null)
        {
            Debug.LogWarning("BonusCounterText not assigned! Please assign it in the inspector.");
        }
        
        // Обновляем отображение
        UpdateBonusDisplay();
    }
    
    void Update()
    {
        // Обновляем отображение каждый кадр (на случай изменения данных)
        UpdateBonusDisplay();
    }
    
    private void UpdateBonusDisplay()
    {
        // Создаем PlayerProgress если его нет
        if (PlayerProgress.Instance == null)
        {
            Debug.Log("PlayerProgress not found, creating one...");
            GameObject playerProgressGO = new GameObject("PlayerProgress");
            PlayerProgress playerProgress = playerProgressGO.AddComponent<PlayerProgress>();
        }
        
        // Отображаем количество накопленных бонусов
        if (bonusCounterText != null && PlayerProgress.Instance != null)
        {
            int totalBonuses = PlayerProgress.Instance.GetTotalBonusesEarned();
            bonusCounterText.text = $"Bonus points: +{totalBonuses}";
            Debug.Log($"BonusCounterUI: Displaying {totalBonuses} bonus points");
        }
        else
        {
            Debug.LogWarning("BonusCounterUI: Cannot update display - missing references");
        }
    }
    
    // Метод для принудительного обновления отображения
    public void RefreshDisplay()
    {
        UpdateBonusDisplay();
    }
} 