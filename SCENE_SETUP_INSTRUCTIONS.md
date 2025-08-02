# Инструкция по настройке сцен

## Настройка сцены Menu

1. Откройте сцену `Menu.unity`
2. Добавьте пустой GameObject и назовите его `MenuController`
3. Добавьте компонент `MenuController` к этому GameObject
4. В настройках `MenuController` установите:
   - **Click Anywhere To Start**: `true`
   - **Target Scene**: `Level1`

## Настройка сцены Level 1

1. Откройте сцену `Level 1.unity`
2. Добавьте пустой GameObject и назовите его `LevelTimer`
3. Добавьте компонент `LevelTimer` к этому GameObject
4. Убедитесь, что в настройках `LevelTimer` установлено время `30` секунд
5. Проверьте, что `GameManager` присутствует на сцене
6. Создайте UI элементы вручную:
   - Canvas (если его нет)
   - TextMeshProUGUI для счета
   - Перетащите TextMeshProUGUI для счета в GameManager в поле Score Text
7. В GameManager выберите **Victory Scene**:
   - `Win` - переход к сцене победы
   - `Menu` - переход в главное меню
   - `Level1` - повторить уровень
   - `NextLevel` - следующий уровень

### Автоматическая инициализация (рекомендуется)

8. Добавьте `GameInitializer` для автоматического создания необходимых компонентов:
   - Создайте пустой GameObject и назовите его `GameInitializer`
   - Добавьте компонент `GameInitializer`
   - В настройках установите:
     - **Auto Create Scene Controller**: `true`
     - **Auto Create Game Manager**: `true`
   - Это позволит запускать игру сразу с Level 1 без ошибок

### Настройка отступов для голов

9. Найдите GameObject с компонентом `HeadSpawner` на сцене
10. В настройках `HeadSpawner` настройте отступы:
    - **Margin From Edges Percent**: `0.1` (10% от размера экрана)
    - **Min Margin Pixels**: `50` (минимальный отступ в пикселях)
    - **Use Screen Bounds**: `true`
    - **Exclude UI Zones**: `true`

11. (Опционально) Добавьте `UIZoneManager` для автоматического исключения UI зон:
    - Создайте пустой GameObject и назовите его `UIZoneManager`
    - Добавьте компонент `UIZoneManager`
    - В настройках установите:
      - **Head Spawner**: перетащите GameObject с `HeadSpawner`
      - **Auto Find UI Elements**: `true`
      - **Exclude Top UI**: `true` (исключить верхние UI элементы)
      - **Exclude Bottom UI**: `true` (исключить нижние UI элементы)
      - **Exclude Left UI**: `true` (исключить левые UI элементы)
      - **Exclude Right UI**: `true` (исключить правые UI элементы)

### Ручная настройка UI зон исключения

12. Если нужно исключить конкретные UI элементы:
    - В `HeadSpawner` найдите массив **UI Zones To Exclude**
    - Перетащите нужные RectTransform UI элементов в этот массив
    - Или используйте метод `AddUIZoneToExclude()` из кода

## Настройка сцены Win

1. Откройте сцену `Win.unity`
2. Добавьте пустой GameObject и назовите его `WinController`
3. Добавьте компонент `MenuController` к этому GameObject
4. В настройках `MenuController` установите:
   - **Click Anywhere To Start**: `true`
   - **Target Scene**: `Menu`

## Настройка сцены Lose

1. Откройте сцену `Lose.unity`
2. Добавьте пустой GameObject и назовите его `LoseController`
3. Добавьте компонент `MenuController` к этому GameObject
4. В настройках `MenuController` установите:
   - **Click Anywhere To Start**: `true`
   - **Target Scene**: `Menu`

## Порядок сцен в Build Settings

Убедитесь, что сцены расположены в следующем порядке:
1. Menu (индекс 0)
2. Level 1 (индекс 1)
3. Win (индекс 2)
4. Lose (индекс 3)

## Логика работы

1. **Menu** → клик в любом месте → **Level 1**
2. **Level 1** → набрал target score → **выбранная сцена** (Win/Menu/Level1/NextLevel)
3. **Level 1** → истекло 30 секунд → **Lose**
4. **Win/Lose** → клик в любом месте → **Menu**

## Важные моменты

- `SceneController` автоматически создается как singleton и не уничтожается при переходах между сценами
- `GameManager` также использует singleton pattern и сохраняется между сценами
- `GameManager` работает с UI элементами, которые создаются вручную
- `LevelTimer` автоматически создает UI элемент для отображения времени
- При истечении времени игра автоматически переходит к сцене проигрыша
- При достижении target score игра автоматически переходит к сцене победы

## Настройка отступов голов

### Адаптивные отступы
- **Margin From Edges Percent**: процент от размера экрана (рекомендуется 0.1 = 10%)
- **Min Margin Pixels**: минимальный отступ в пикселях (рекомендуется 50)
- Отступы автоматически адаптируются к размеру экрана

### Исключение UI зон
- **Exclude UI Zones**: включить/выключить исключение UI элементов
- **UI Zones To Exclude**: массив UI элементов для исключения
- `UIZoneManager` автоматически находит и исключает UI элементы

### Debug функции
- **Show Spawn Bounds**: показать границы спавна в Scene View (зеленая рамка)
- **Show Debug Info**: показать информацию о спавне в консоли 