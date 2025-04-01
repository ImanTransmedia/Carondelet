using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour

{
    [Header("Fade Settings")]
    public CanvasGroup fadeCanva; 
    public float fadeDuration = 0.4f;

    // Script centralizado para todas las funciones de UI
    void Start()
    {
       FadeOut();
    }

    // Función reutilizable para cargar cualquier escena de forma asíncrona
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
    public void FadeIn(){
         StartCoroutine(FadeInCorutine());
    }

    // Función de Fade Out
     public void FadeOut(){
         StartCoroutine(FadeOutCorutine());
    }

}