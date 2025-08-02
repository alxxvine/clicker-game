using UnityEngine;
using UnityEngine.UI;

public class Head : MonoBehaviour
{
    [Header("Click Settings")]
    [SerializeField] private int pointsPerClick = 1;
    [SerializeField] private float clickAnimationDuration = 0.2f;
    [SerializeField] private float scaleMultiplier = 1.2f;
    
    [Header("Visual Feedback")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color clickColor = Color.yellow;
    
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private Color originalColor;
    private bool isAnimating = false;
    private bool clicked = false; // Флаг для предотвращения множественных кликов
    
    void Start()
    {
        Debug.Log($"Head Start called on {gameObject.name} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on head!");
            return;
        }
        
        originalScale = transform.localScale;
        originalColor = spriteRenderer.color;
        
        // Добавляем коллайдер для кликов, если его нет
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
        
        // Применяем бонусный размер click zone
        ApplyClickZoneBonus();
    }
    
    private void ApplyClickZoneBonus()
    {
        if (PlayerProgress.Instance != null)
        {
            float bonusSize = PlayerProgress.Instance.GetClickZoneSize();
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size *= bonusSize;
                Debug.Log($"Applied click zone bonus: {bonusSize}x size");
            }
        }
    }
    
    void OnMouseDown()
    {
        if (!isAnimating && !clicked)
        {
            OnHeadClicked();
        }
    }
    
    public void OnHeadClicked()
    {
        Debug.Log($"Head clicked! Trying to access GameManager.Instance...");
        
        // Устанавливаем флаг клика
        clicked = true;
        
        // Начисляем очки
        GameManager.Instance.AddPoints(pointsPerClick);
        
        // Запускаем анимацию клика
        StartCoroutine(ClickAnimation());
    }
    
    private System.Collections.IEnumerator ClickAnimation()
    {
        isAnimating = true;
        
        // Анимация увеличения
        float elapsedTime = 0f;
        Vector3 targetScale = originalScale * scaleMultiplier;
        
        while (elapsedTime < clickAnimationDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (clickAnimationDuration / 2f);
            transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            spriteRenderer.color = Color.Lerp(originalColor, clickColor, progress);
            yield return null;
        }
        
        // Анимация уменьшения
        elapsedTime = 0f;
        while (elapsedTime < clickAnimationDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (clickAnimationDuration / 2f);
            transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
            spriteRenderer.color = Color.Lerp(clickColor, originalColor, progress);
            yield return null;
        }
        
        // Возвращаем к исходному состоянию
        transform.localScale = originalScale;
        spriteRenderer.color = originalColor;
        isAnimating = false;
        
        // Голова остается кликнутой - она больше не может быть кликнута
        // Это предотвращает множественные клики
    }
    
    public void SetPointsPerClick(int points)
    {
        pointsPerClick = points;
    }
    
    public int GetPointsPerClick()
    {
        return pointsPerClick;
    }
} 