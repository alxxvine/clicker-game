using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int clickZoneRadiusBonus = 0;
    [SerializeField] private int maxClickZoneRadiusBonus = 50; // Максимальный бонус
    [SerializeField] private int totalBonusesEarned = 0; // Общее количество полученных бонусов
    [SerializeField] private int currentSessionBonuses = 0; // Бонусы в текущей сессии
    
    // Singleton pattern
    public static PlayerProgress Instance { get; private set; }
    
    void Awake()
    {
        Debug.Log($"PlayerProgress Awake called on {gameObject.name}");
        
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PlayerProgress singleton created and set to DontDestroyOnLoad");
        }
        else
        {
            Debug.Log("PlayerProgress already exists, destroying duplicate");
            Destroy(gameObject);
        }
    }
    
    // Добавить бонусные очки при проигрыше
    public void AddClickZoneRadiusBonus(int bonus)
    {
        clickZoneRadiusBonus = Mathf.Min(clickZoneRadiusBonus + bonus, maxClickZoneRadiusBonus);
        totalBonusesEarned += bonus;
        currentSessionBonuses += bonus;
        Debug.Log($"Added {bonus} click zone radius bonus. Total: {clickZoneRadiusBonus}, Session: {currentSessionBonuses}");
    }
    
    // Получить текущий бонус
    public int GetClickZoneRadiusBonus()
    {
        return clickZoneRadiusBonus;
    }
    
    // Сбросить прогресс (для новой игры)
    public void ResetProgress()
    {
        clickZoneRadiusBonus = 0;
        totalBonusesEarned = 0;
        currentSessionBonuses = 0;
        Debug.Log("Player progress reset");
    }
    
    // Получить количество бонусов в текущей сессии
    public int GetCurrentSessionBonuses()
    {
        return currentSessionBonuses;
    }
    
    // Получить общее количество полученных бонусов
    public int GetTotalBonusesEarned()
    {
        return totalBonusesEarned;
    }
    
    // Получить размер коллайдера с учетом бонуса
    public float GetClickZoneSize()
    {
        // Базовый размер + бонус (в процентах)
        float baseSize = 1f;
        float bonusMultiplier = 1f + (clickZoneRadiusBonus / 100f);
        return baseSize * bonusMultiplier;
    }
} 