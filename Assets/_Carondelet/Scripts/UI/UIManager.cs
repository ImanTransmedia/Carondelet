using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour
{

    [Header("Script References")]
    public SceneLoader sceneLoader;
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
         //   accessibilityManager.enabled = false;
        }
        else
        {
            ShowDesktopUI();
         //   accessibilityManager.enabled = true;
        }
    }
    

    bool DetectMobileWebGL()
    {
        return Application.isMobilePlatform;
    }


    public void StartAsyncLoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        
        yield return FadeInCorutine();
        if (sceneLoader != null)
        {
            sceneLoader.StartAsyncLoadScene(sceneName); 
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

    public void FadeIn()
    {
        StartCoroutine(FadeInCorutine());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCorutine());
    }

    void ShowMobileUI()
    {
        SetActiveObjects(mobileUIObjects, true);
        SetActiveObjects(desktopUIObjects, false);
    }

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
