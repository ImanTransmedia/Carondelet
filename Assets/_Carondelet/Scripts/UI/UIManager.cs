using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
// Script centralizado para todas las funciones de UI
{
    [Header("Script References")]
    public AccessibilityManager accessibilityManager;
    
    [Header("Mobile device status")]
    public bool isMobile;

    [Header("Desktop UI Elements")]
    public List<GameObject> desktopUIObjects;

    [Header("Mobile UI Elements")]
    public List<GameObject> mobileUIObjects;

   

    [Header("Fade Settings")]
    public CanvasGroup fadeCanva;
    public float fadeDuration = 0.4f;

    void Start()
    {
        if (accessibilityManager == null)
        {
            accessibilityManager = GetComponent<AccessibilityManager>();
        }
        FadeOut();
        isMobile = DetectMobileWebGL();

        if (isMobile)
        {
            ShowMobileUI();
            accessibilityManager.enabled = false;
        }
        else
        {
            ShowDesktopUI();
            accessibilityManager.enabled = true;
        }
    }

    bool DetectMobileWebGL()
    {
        return Application.isMobilePlatform;
    }

    // Función  para cargar  escena de forma asíncrona
    public void StartAsyncLoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        yield return FadeInCorutine(); // Esperar a que termine FadeIn completamente
        yield return LoadSceneAsync(sceneName); //Empezar a cargar la escena
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void TogglePanel(GameObject panel, bool state)
    {
        if (panel != null)
        {
            Animator animator = panel.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("isOpen", state);
            }
            else
            {
                panel.SetActive(state);
            }
        }
    }

    public void OpenPanel(GameObject panel)
    {
        TogglePanel(panel, true);
    }

    public void ClosePanel(GameObject panel)
    {
        TogglePanel(panel, false);
    }

    public IEnumerator FadeInCorutine()
    {
        fadeCanva.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanva.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }
        fadeCanva.alpha = 1f;
    }

    public IEnumerator FadeOutCorutine()
    {
        fadeCanva.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanva.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        fadeCanva.alpha = 0f;
        fadeCanva.gameObject.SetActive(false);
    }

    // Función de Fade In
    public void FadeIn()
    {
        StartCoroutine(FadeInCorutine());
    }

    // Función de Fade Out
    public void FadeOut()
    {
        StartCoroutine(FadeOutCorutine());
    }

    //Funcion mostrar Ui solo de mobile
    void ShowMobileUI()
    {
        SetActiveObjects(mobileUIObjects, true);
        SetActiveObjects(desktopUIObjects, false);
    }

    //Funcion mostrar Ui solo de desktop
    void ShowDesktopUI()
    {
        SetActiveObjects(mobileUIObjects, false);
        SetActiveObjects(desktopUIObjects, true);
    }

    void SetActiveObjects(List<GameObject> objects, bool state)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }
}
