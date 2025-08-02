using UnityEngine;
using UnityEngine.UI;

public class UIZoneManager : MonoBehaviour
{
    [Header("UI Zone Management")]
    [SerializeField] private HeadSpawner headSpawner;
    [SerializeField] private bool autoFindUIElements = true;
    [SerializeField] private string[] uiElementTags = { "UI", "Canvas", "Panel" };
    [SerializeField] private bool excludeTopUI = true;
    [SerializeField] private bool excludeBottomUI = true;
    [SerializeField] private bool excludeLeftUI = true;
    [SerializeField] private bool excludeRightUI = true;
    
    [Header("Manual UI Elements")]
    [SerializeField] private RectTransform[] manualUIElements;
    
    void Start()
    {
        if (headSpawner == null)
        {
            headSpawner = FindFirstObjectByType<HeadSpawner>();
        }
        
        if (headSpawner != null)
        {
            SetupUIZones();
        }
        else
        {
            Debug.LogError("HeadSpawner not found! Please assign it manually.");
        }
    }
    
    void SetupUIZones()
    {
        // Очищаем предыдущие зоны
        headSpawner.ClearUIZones();
        
        // Добавляем автоматически найденные UI элементы
        if (autoFindUIElements)
        {
            FindAndAddUIElements();
        }
        
        // Добавляем вручную указанные элементы
        if (manualUIElements != null)
        {
            foreach (RectTransform uiElement in manualUIElements)
            {
                if (uiElement != null)
                {
                    headSpawner.AddUIZoneToExclude(uiElement);
                }
            }
        }
        
        // Включаем исключение UI зон
        headSpawner.SetExcludeUIZones(true);
    }
    
    void FindAndAddUIElements()
    {
        // Ищем Canvas элементы
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || 
                canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                if (canvasRect != null)
                {
                    // Проверяем позицию Canvas для определения, какие части экрана он занимает
                    if (ShouldExcludeCanvas(canvasRect))
                    {
                        headSpawner.AddUIZoneToExclude(canvasRect);
                    }
                }
            }
        }
        
        // Ищем элементы по тегам
        foreach (string tag in uiElementTags)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in taggedObjects)
            {
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    headSpawner.AddUIZoneToExclude(rectTransform);
                }
            }
        }
    }
    
    bool ShouldExcludeCanvas(RectTransform canvasRect)
    {
        if (!excludeTopUI && !excludeBottomUI && !excludeLeftUI && !excludeRightUI)
            return false;
            
        // Получаем границы Canvas в экранных координатах
        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);
        
        // Конвертируем в экранные координаты
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            for (int i = 0; i < 4; i++)
            {
                corners[i] = mainCamera.WorldToScreenPoint(corners[i]);
            }
        }
        
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;
        
        // Проверяем, находится ли Canvas в верхней части экрана
        bool isTop = corners[1].y > screenHeight * 0.7f; // Верхние 30% экрана
        bool isBottom = corners[0].y < screenHeight * 0.3f; // Нижние 30% экрана
        bool isLeft = corners[0].x < screenWidth * 0.3f; // Левые 30% экрана
        bool isRight = corners[1].x > screenWidth * 0.7f; // Правые 30% экрана
        
        return (isTop && excludeTopUI) || (isBottom && excludeBottomUI) || 
               (isLeft && excludeLeftUI) || (isRight && excludeRightUI);
    }
    
    // Публичные методы для динамического управления
    public void AddUIElement(RectTransform uiElement)
    {
        if (headSpawner != null && uiElement != null)
        {
            headSpawner.AddUIZoneToExclude(uiElement);
        }
    }
    
    public void RemoveUIElement(RectTransform uiElement)
    {
        // Для удаления конкретного элемента нужно пересоздать список
        // Это можно реализовать, если потребуется
        Debug.Log("RemoveUIElement: This feature requires reimplementation of the exclusion list");
    }
    
    public void RefreshUIZones()
    {
        if (headSpawner != null)
        {
            SetupUIZones();
        }
    }
    
    public void SetExcludeTopUI(bool exclude)
    {
        excludeTopUI = exclude;
        RefreshUIZones();
    }
    
    public void SetExcludeBottomUI(bool exclude)
    {
        excludeBottomUI = exclude;
        RefreshUIZones();
    }
    
    public void SetExcludeLeftUI(bool exclude)
    {
        excludeLeftUI = exclude;
        RefreshUIZones();
    }
    
    public void SetExcludeRightUI(bool exclude)
    {
        excludeRightUI = exclude;
        RefreshUIZones();
    }
} 