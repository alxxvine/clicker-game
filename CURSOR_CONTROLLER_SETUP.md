# Настройка курсора через CursorController

## Как использовать

### Шаг 1: Создание CursorController
1. **Создайте пустой GameObject** в сцене
2. **Переименуйте его в "CursorController"**
3. **Добавьте компонент `CursorController`**

### Шаг 2: Настройка курсора
1. **В компоненте CursorController**:
   - Перетащите текстуру "Hand" в поле `Hand Cursor`
   - Установите `Hotspot` - точку клика курсора
   - Настройте `Click Zone Radius` - радиус зоны клика в пикселях (0 = стандартный)
   - Включите `Set Cursor On Start` для автоматической установки

### Шаг 3: Настройка точки клика
- **Левый верхний угол**: `Hotspot = (0, 0)`
- **Центр**: `Hotspot = (16, 16)` для курсора 32x32
- **Правый нижний угол**: `Hotspot = (32, 32)` для курсора 32x32

## Использование в коде

```csharp
// Найти CursorController в сцене
CursorController cursorController = FindObjectOfType<CursorController>();

// Установить курсор руки
cursorController.SetHandCursor();

// Изменить размер курсора
cursorController.SetCursorScale(2f); // Увеличить в 2 раза

// Установить точку клика
cursorController.SetCursorHotspot(new Vector2(0, 0));

// Готовые методы для точки клика
cursorController.SetHotspotTopLeft();      // Левый верхний угол
cursorController.SetHotspotCenter();       // Центр
cursorController.SetHotspotBottomRight();  // Правый нижний угол

// Установить радиус зоны клика
cursorController.SetClickZoneRadius(20f); // Радиус 20 пикселей

// Показать/скрыть курсор
cursorController.ShowCursor();
cursorController.HideCursor();

// Заблокировать/разблокировать курсор
cursorController.LockCursor();
cursorController.UnlockCursor();
```

## Примеры использования

### Изменение курсора при наведении:
```csharp
public class ClickableObject : MonoBehaviour
{
    [SerializeField] private Texture2D handSprite;
    [SerializeField] private Texture2D defaultSprite;
    
    void OnMouseEnter()
    {
        CursorController controller = FindObjectOfType<CursorController>();
        if (controller != null)
        {
            controller.SetCustomCursor(handSprite, new Vector2(0, 0));
        }
    }
    
    void OnMouseExit()
    {
        CursorController controller = FindObjectOfType<CursorController>();
        if (controller != null)
        {
            controller.SetCustomCursor(defaultSprite, new Vector2(0, 0));
        }
    }
}
```

### Масштабирование курсора под разрешение экрана:
```csharp
public class CursorScaler : MonoBehaviour
{
    void Start()
    {
        CursorController controller = FindObjectOfType<CursorController>();
        if (controller != null)
        {
            // Масштабировать курсор под разрешение экрана
            float screenScale = Screen.width / 1920f; // Базовое разрешение
            controller.SetCursorScale(screenScale);
        }
    }
}
```

## Настройка текстуры

1. **Выберите текстуру** в Project window
2. **В инспекторе установите**:
   ```
   Texture Type: Default
   Max Size: 128 (или больше если нужно)
   Format: RGBA 32 bit
   ```
3. **Нажмите Apply**

## Преимущества

✅ **Системный курсор** - работает через настройки Unity  
✅ **Масштабирование** - можно изменять размер курсора  
✅ **Гибкость** - множество методов для управления  
✅ **Производительность** - быстрее UI курсора  
✅ **Совместимость** - работает на всех платформах  
✅ **Автоматическое восстановление** - возвращает оригинальный курсор  

## Настройка зоны клика

### В инспекторе:
- **Click Zone Radius** - радиус зоны клика в пикселях
- `0` = стандартная зона клика
- `20` = увеличенная зона клика на 20 пикселей
- `50` = большая зона клика на 50 пикселей

### В коде:
```csharp
// Установить радиус зоны клика
cursorController.SetClickZoneRadius(30f); // Радиус 30 пикселей

// Получить текущий радиус
float radius = cursorController.GetClickZoneRadius();
```

### Для объектов с увеличенной зоной клика:
1. **Добавьте компонент `ExtendedClickZone`** на объект
2. **Настройте `Click Zone Radius`** - радиус зоны клика в пикселях
3. **Включите `Show Debug Zone`** для визуализации зоны в редакторе

## Устранение проблем

1. **Курсор не меняется**: Проверьте что текстура назначена в `Hand Cursor`
2. **Неправильная точка клика**: Настройте `Hotspot`
3. **Зона клика не работает**: Проверьте что камера ортографическая для ExtendedClickZone 