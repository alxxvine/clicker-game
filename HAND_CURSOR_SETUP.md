# Настройка курсора руки в каждой сцене

## Способ 1: Автоматическое создание (Рекомендуемый)

### Шаг 1: Настройка GameInitializer
1. **Найдите объект с GameInitializer** в сцене
2. **В инспекторе включите** `Auto Create UI Cursor`
3. **Курсор будет создаваться автоматически** в каждой сцене

### Шаг 2: Настройка курсора
1. **Найдите объект HandCursor** в сцене (создается автоматически)
2. **Перетащите спрайт "Hand"** в поле `Hand Sprite`
3. **Настройте размеры** в `Cursor Width` и `Cursor Height`
4. **Настройте смещение** в `Offset` если нужно

## Способ 2: Ручное создание в каждой сцене

### Шаг 1: Создание курсора
1. **Создайте Canvas** (если его нет):
   - Правый клик в Hierarchy → UI → Canvas
   - Установите `Render Mode` = `Screen Space - Overlay`
   - Установите `Sort Order` = 999

2. **Создайте курсор**:
   - Правый клик на Canvas → UI → Image
   - Переименуйте в "HandCursor"
   - Добавьте компонент `HandCursor`

3. **Настройте курсор**:
   - Перетащите спрайт "Hand" в поле `Hand Sprite`
   - Установите размеры в `Cursor Width/Height`
   - Настройте смещение в `Offset`

### Шаг 2: Создание Prefab
1. **Перетащите HandCursor** из Hierarchy в папку Prefabs
2. **Удалите объект** из сцены
3. **Перетащите prefab** в каждую сцену

## Использование в коде

```csharp
// Изменить спрайт курсора
HandCursor.Instance.SetHandSprite(newSprite);

// Изменить размер
HandCursor.Instance.SetCursorSize(48f, 48f);

// Изменить смещение
HandCursor.Instance.SetCursorOffset(new Vector2(-10f, -10f));

// Показать/скрыть курсор
HandCursor.Instance.ShowCursor();
HandCursor.Instance.HideCursor();

// Создать курсор если его нет
HandCursor.CreateCursorInScene();
```

## Настройка для разных ситуаций

### Изменение курсора при наведении:
```csharp
public class ClickableObject : MonoBehaviour
{
    [SerializeField] private Sprite handSprite;
    [SerializeField] private Sprite defaultSprite;
    
    void OnMouseEnter()
    {
        HandCursor.Instance.SetHandSprite(handSprite);
    }
    
    void OnMouseExit()
    {
        HandCursor.Instance.SetHandSprite(defaultSprite);
    }
}
```

### Анимация курсора:
```csharp
public class CursorAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] cursorFrames;
    private int currentFrame = 0;
    
    void Start()
    {
        InvokeRepeating(nameof(AnimateCursor), 0f, 0.1f);
    }
    
    void AnimateCursor()
    {
        if (cursorFrames.Length > 0)
        {
            HandCursor.Instance.SetHandSprite(cursorFrames[currentFrame]);
            currentFrame = (currentFrame + 1) % cursorFrames.Length;
        }
    }
}
```

## Преимущества нового HandCursor

✅ **Один универсальный скрипт** - все функции в одном месте  
✅ **Автоматически в каждой сцене** - DontDestroyOnLoad  
✅ **Простая настройка** - все в инспекторе  
✅ **Гибкое управление** - методы для изменения во время игры  
✅ **Автоматическое скрытие системного курсора**  

## Устранение проблем

1. **Курсор не появляется**: Проверьте `Auto Create UI Cursor` в GameInitializer
2. **Белый кубик**: Проверьте что спрайт назначен в `Hand Sprite`
3. **Системный курсор виден**: HandCursor автоматически скрывает его
4. **Курсор под UI**: Увеличьте `Sort Order` Canvas

## Настройка спрайта

1. **Выберите текстуру** в Project window
2. **В инспекторе установите**:
   ```
   Texture Type: Sprite (2D and UI)
   Sprite Mode: Single
   Pixels Per Unit: 100
   ```
3. **Нажмите Apply**
4. **Перетащите в поле Hand Sprite** компонента HandCursor 