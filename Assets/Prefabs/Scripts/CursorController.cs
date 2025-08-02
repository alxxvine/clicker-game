using UnityEngine;

public class CursorController : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] private Texture2D handCursor;
    [SerializeField] private Vector2 hotspot = new Vector2(0, 0);
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    
    [Header("Click Zone")]
    [SerializeField] private float clickZoneRadius = 0f; // Радиус зоны клика в пикселях (0 = стандартный)
    
    [Header("Auto Setup")]
    [SerializeField] private bool setCursorOnStart = true;
    

    
    void Start()
    {
        if (setCursorOnStart)
        {
            SetHandCursor();
        }
    }
    
    /// <summary>
    /// Устанавливает курсор руки
    /// </summary>
    public void SetHandCursor()
    {
        if (handCursor != null)
        {
            Cursor.SetCursor(handCursor, hotspot, cursorMode);
            Debug.Log("Hand cursor set");
        }
        else
        {
            Debug.LogWarning("Hand cursor texture not assigned!");
        }
    }
    
    /// <summary>
    /// Возвращает стандартный курсор
    /// </summary>
    public void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Debug.Log("Default cursor restored");
    }
    
    /// <summary>
    /// Устанавливает кастомный курсор
    /// </summary>
    public void SetCustomCursor(Texture2D texture, Vector2 customHotspot)
    {
        if (texture != null)
        {
            Cursor.SetCursor(texture, customHotspot, cursorMode);
            Debug.Log($"Custom cursor set: {texture.name}");
        }
    }
    
    /// <summary>
    /// Устанавливает радиус зоны клика
    /// </summary>
    public void SetClickZoneRadius(float radius)
    {
        clickZoneRadius = radius;
        Debug.Log($"Click zone radius set to: {radius} pixels");
    }
    
    /// <summary>
    /// Получает текущий радиус зоны клика
    /// </summary>
    public float GetClickZoneRadius()
    {
        return clickZoneRadius;
    }
    
    /// <summary>
    /// Устанавливает точку клика
    /// </summary>
    public void SetCursorHotspot(Vector2 newHotspot)
    {
        hotspot = newHotspot;
        
        if (handCursor != null)
        {
            SetHandCursor(); // Переустанавливаем курсор с новой точкой клика
        }
    }
    
    /// <summary>
    /// Показывает курсор
    /// </summary>
    public void ShowCursor()
    {
        Cursor.visible = true;
    }
    
    /// <summary>
    /// Скрывает курсор
    /// </summary>
    public void HideCursor()
    {
        Cursor.visible = false;
    }
    
    /// <summary>
    /// Блокирует курсор в центре экрана
    /// </summary>
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    /// <summary>
    /// Разблокирует курсор
    /// </summary>
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    

    
    /// <summary>
    /// Устанавливает точку клика в левом верхнем углу
    /// </summary>
    public void SetHotspotTopLeft()
    {
        SetCursorHotspot(new Vector2(0, 0));
    }
    
    /// <summary>
    /// Устанавливает точку клика в центре
    /// </summary>
    public void SetHotspotCenter()
    {
        if (handCursor != null)
        {
            Vector2 center = new Vector2(handCursor.width / 2f, handCursor.height / 2f);
            SetCursorHotspot(center);
        }
    }
    
    /// <summary>
    /// Устанавливает точку клика в правом нижнем углу
    /// </summary>
    public void SetHotspotBottomRight()
    {
        if (handCursor != null)
        {
            Vector2 bottomRight = new Vector2(handCursor.width, handCursor.height);
            SetCursorHotspot(bottomRight);
        }
    }
    
    void OnDestroy()
    {
        // Возвращаем стандартный курсор при уничтожении
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
} 