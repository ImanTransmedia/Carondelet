using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    // Script centralizado para todas las funciones de UI
    void Start()
    {
        // Desactivar cualquier elemento de UI que no deba estar activo al iniciar el programa
    }

    // Función reutilizable para cargar cualquier escena
    public void StartAsyncLoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
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
}
