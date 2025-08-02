using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeadSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject headPrefab;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private float headLifetime = 1f;
    
    [Header("Screen Bounds")]
    [SerializeField] private float marginFromEdgesPercent = 0.1f; // 10% от размера экрана
    [SerializeField] private float minMarginPixels = 50f; // Минимальный отступ в пикселях
    [SerializeField] private bool useScreenBounds = true;
    
    [Header("UI Exclusion Zones")]
    [SerializeField] private bool excludeUIZones = true;
    [SerializeField] private RectTransform[] uiZonesToExclude; // UI элементы, которые нужно исключить
    
    [Header("Random Position")]
    [SerializeField] private Vector2 minPosition = new Vector2(-5f, -3f);
    [SerializeField] private Vector2 maxPosition = new Vector2(5f, 3f);
    
    [Header("Spawn Zone")]
    [SerializeField] private float minDistanceBetweenHeads = 2f;
    [SerializeField] private int maxSpawnAttempts = 10;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private bool showSpawnBounds = false;
    
    private Camera mainCamera;
    private bool isSpawning = false;
    private List<GameObject> activeHeads = new List<GameObject>();
    
    void Start()
    {
        Debug.Log($"HeadSpawner Start called on {gameObject.name} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found! Make sure you have a camera tagged as 'MainCamera'");
            return;
        }
        
        if (headPrefab == null)
        {
            Debug.LogError("Head prefab not assigned! Please assign a prefab in the inspector.");
            return;
        }
        
        StartSpawning();
    }
    
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
    
    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            SpawnHead();
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    
    private void SpawnHead()
    {
        Vector3 spawnPosition = GetRandomSpawnPositionWithDistanceCheck();
        
        if (spawnPosition != Vector3.zero) // Если нашли подходящую позицию
        {
            GameObject head = Instantiate(headPrefab, spawnPosition, Quaternion.identity);
            activeHeads.Add(head);
            
            // Добавляем скрипт Head если его нет
            if (head.GetComponent<Head>() == null)
            {
                head.AddComponent<Head>();
            }
            
            if (showDebugInfo)
            {
                Debug.Log($"Spawned head at position: {spawnPosition}");
            }
            
            StartCoroutine(DestroyHeadAfterDelay(head));
        }
        else
        {
            if (showDebugInfo)
            {
                Debug.LogWarning("Could not find suitable spawn position after maximum attempts");
            }
        }
    }
    
    private Vector3 GetRandomSpawnPositionWithDistanceCheck()
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector3 position = GetRandomSpawnPosition();
            
            // Проверяем, не попадает ли позиция в UI зоны
            if (excludeUIZones && IsPositionInUIZone(position))
            {
                continue;
            }
            
            // Проверяем расстояние до всех активных голов
            bool isPositionValid = true;
            foreach (GameObject head in activeHeads)
            {
                if (head != null)
                {
                    float distance = Vector3.Distance(position, head.transform.position);
                    if (distance < minDistanceBetweenHeads)
                    {
                        isPositionValid = false;
                        break;
                    }
                }
            }
            
            if (isPositionValid)
            {
                return position;
            }
        }
        
        // Если не удалось найти подходящую позицию, возвращаем Vector3.zero
        return Vector3.zero;
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 position;
        
        if (useScreenBounds)
        {
            // Вычисляем адаптивные отступы
            float marginX = Mathf.Max(minMarginPixels, Screen.width * marginFromEdgesPercent);
            float marginY = Mathf.Max(minMarginPixels, Screen.height * marginFromEdgesPercent);
            
            float x = Random.Range(marginX, Screen.width - marginX);
            float y = Random.Range(marginY, Screen.height - marginY);
            
            Vector3 screenPos = new Vector3(x, y, 0);
            position = mainCamera.ScreenToWorldPoint(screenPos);
            position.z = 0; // Устанавливаем Z в 0 для 2D
        }
        else
        {
            // Используем заданные границы
            float x = Random.Range(minPosition.x, maxPosition.x);
            float y = Random.Range(minPosition.y, maxPosition.y);
            position = new Vector3(x, y, 0);
        }
        
        return position;
    }
    
    private bool IsPositionInUIZone(Vector3 worldPosition)
    {
        if (uiZonesToExclude == null || uiZonesToExclude.Length == 0)
            return false;
            
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        
        foreach (RectTransform uiZone in uiZonesToExclude)
        {
            if (uiZone != null && uiZone.gameObject.activeInHierarchy)
            {
                // Получаем границы UI элемента в экранных координатах
                Vector3[] corners = new Vector3[4];
                uiZone.GetWorldCorners(corners);
                
                // Конвертируем углы в экранные координаты
                for (int i = 0; i < 4; i++)
                {
                    corners[i] = mainCamera.WorldToScreenPoint(corners[i]);
                }
                
                // Проверяем, находится ли точка внутри прямоугольника UI
                if (IsPointInRect(screenPosition, corners))
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    private bool IsPointInRect(Vector3 point, Vector3[] corners)
    {
        // Простая проверка: точка внутри прямоугольника, если она находится между минимальными и максимальными координатами
        float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
        float maxY = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
        
        return point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY;
    }
    
    private IEnumerator DestroyHeadAfterDelay(GameObject head)
    {
        yield return new WaitForSeconds(headLifetime);
        
        if (head != null)
        {
            if (showDebugInfo)
            {
                Debug.Log($"Destroying head at position: {head.transform.position}");
            }
            activeHeads.Remove(head);
            Destroy(head);
        }
    }
    
    // Методы для изменения параметров во время выполнения
    public void SetSpawnDelay(float delay)
    {
        spawnDelay = delay;
    }
    
    public void SetHeadLifetime(float lifetime)
    {
        headLifetime = lifetime;
    }
    
    public void SetMarginFromEdgesPercent(float marginPercent)
    {
        marginFromEdgesPercent = marginPercent;
    }
    
    public void SetMinMarginPixels(float minPixels)
    {
        minMarginPixels = minPixels;
    }
    
    public void SetUseScreenBounds(bool useBounds)
    {
        useScreenBounds = useBounds;
    }
    
    public void SetExcludeUIZones(bool exclude)
    {
        excludeUIZones = exclude;
    }
    
    public void SetMinPosition(Vector2 min)
    {
        minPosition = min;
    }
    
    public void SetMaxPosition(Vector2 max)
    {
        maxPosition = max;
    }
    
    public void SetMinDistanceBetweenHeads(float distance)
    {
        minDistanceBetweenHeads = distance;
    }
    
    public void SetMaxSpawnAttempts(int attempts)
    {
        maxSpawnAttempts = attempts;
    }
    
    // Методы для получения текущих значений
    public float GetSpawnDelay() => spawnDelay;
    public float GetHeadLifetime() => headLifetime;
    public float GetMarginFromEdgesPercent() => marginFromEdgesPercent;
    public float GetMinMarginPixels() => minMarginPixels;
    public bool GetUseScreenBounds() => useScreenBounds;
    public bool GetExcludeUIZones() => excludeUIZones;
    public Vector2 GetMinPosition() => minPosition;
    public Vector2 GetMaxPosition() => maxPosition;
    public float GetMinDistanceBetweenHeads() => minDistanceBetweenHeads;
    public int GetMaxSpawnAttempts() => maxSpawnAttempts;
    
    // Метод для добавления UI зоны для исключения
    public void AddUIZoneToExclude(RectTransform uiZone)
    {
        if (uiZone != null)
        {
            if (uiZonesToExclude == null)
            {
                uiZonesToExclude = new RectTransform[1];
            }
            else
            {
                System.Array.Resize(ref uiZonesToExclude, uiZonesToExclude.Length + 1);
            }
            uiZonesToExclude[uiZonesToExclude.Length - 1] = uiZone;
        }
    }
    
    // Метод для очистки всех UI зон
    public void ClearUIZones()
    {
        uiZonesToExclude = null;
    }
    
    // Debug метод для отображения границ спавна
    void OnDrawGizmos()
    {
        if (showSpawnBounds && mainCamera != null && useScreenBounds)
        {
            float marginX = Mathf.Max(minMarginPixels, Screen.width * marginFromEdgesPercent);
            float marginY = Mathf.Max(minMarginPixels, Screen.height * marginFromEdgesPercent);
            
            Vector3 topLeft = mainCamera.ScreenToWorldPoint(new Vector3(marginX, Screen.height - marginY, 0));
            Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width - marginX, Screen.height - marginY, 0));
            Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(marginX, marginY, 0));
            Vector3 bottomRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width - marginX, marginY, 0));
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
} 