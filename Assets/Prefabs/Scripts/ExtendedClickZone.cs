using UnityEngine;

public class ExtendedClickZone : MonoBehaviour
{
    [Header("Click Zone Settings")]
    [SerializeField] private float clickZoneRadius = 20f; // Радиус зоны клика в пикселях
    [SerializeField] private bool showDebugZone = false; // Показывать зону клика в редакторе
    
    private Vector3 originalScale;
    private bool isHovered = false;
    
    void Start()
    {
        originalScale = transform.localScale;
    }
    
    void Update()
    {
        // Проверяем расстояние от мыши до объекта
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z;
        
        float distance = Vector3.Distance(transform.position, mouseWorldPos);
        
        // Конвертируем расстояние в пиксели
        float distanceInPixels = distance * Screen.height / (2f * Camera.main.orthographicSize);
        
        bool wasHovered = isHovered;
        isHovered = distanceInPixels <= clickZoneRadius;
        
        // Если состояние изменилось, вызываем события
        if (isHovered && !wasHovered)
        {
            OnMouseEnter();
        }
        else if (!isHovered && wasHovered)
        {
            OnMouseExit();
        }
        
        // Проверяем клик
        if (isHovered && Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
    }
    
    void OnMouseEnter()
    {
        // Вызываем стандартные события Unity
        SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
        
        // Можно добавить визуальный эффект
        transform.localScale = originalScale * 1.1f;
    }
    
    void OnMouseExit()
    {
        // Вызываем стандартные события Unity
        SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
        
        // Возвращаем оригинальный размер
        transform.localScale = originalScale;
    }
    
    void OnMouseDown()
    {
        // Вызываем стандартные события Unity
        SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
    }
    
    /// <summary>
    /// Устанавливает радиус зоны клика
    /// </summary>
    public void SetClickZoneRadius(float radius)
    {
        clickZoneRadius = radius;
    }
    
    /// <summary>
    /// Увеличивает радиус зоны клика
    /// </summary>
    public void IncreaseClickZoneRadius(float multiplier = 1.5f)
    {
        clickZoneRadius *= multiplier;
    }
    
    /// <summary>
    /// Получает текущий радиус зоны клика
    /// </summary>
    public float GetClickZoneRadius()
    {
        return clickZoneRadius;
    }
    
    // Рисуем зону клика в редакторе
    void OnDrawGizmosSelected()
    {
        if (showDebugZone)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, clickZoneRadius / Screen.height * (2f * Camera.main.orthographicSize));
        }
    }
} 