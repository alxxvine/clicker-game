using UnityEngine;
using System.Collections;

public class AutoMenuTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private float delayBeforeMenu = 3f;
    [SerializeField] private bool enableAutoTransition = true;
    
    void Start()
    {
        Debug.Log($"AutoMenuTransition Start called on {gameObject.name} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        
        if (enableAutoTransition)
        {
            StartCoroutine(TransitionToMenuAfterDelay());
        }
    }
    
    private IEnumerator TransitionToMenuAfterDelay()
    {
        Debug.Log($"Auto transition to menu will happen in {delayBeforeMenu} seconds");
        
        yield return new WaitForSeconds(delayBeforeMenu);
        
        Debug.Log("Auto transitioning to menu...");
        
        // Проверяем и создаем SceneController если его нет
        if (SceneController.Instance == null)
        {
            Debug.Log("SceneController not found, creating one...");
            GameObject sceneControllerGO = new GameObject("SceneController");
            SceneController sceneController = sceneControllerGO.AddComponent<SceneController>();
        }
        
        // Переходим в меню
        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadMenu();
        }
        else
        {
            Debug.LogError("Failed to create SceneController!");
        }
    }
    
    // Метод для отключения автоматического перехода (если игрок нажал кнопку)
    public void DisableAutoTransition()
    {
        enableAutoTransition = false;
        StopAllCoroutines();
        Debug.Log("Auto transition disabled");
    }
} 