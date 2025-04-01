using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IntereactiveTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //Este script desplbiega un panel hacer click o hover sobre un tooltip en espacio 3d, adicionalmente se pueden añadir funciones extras con eventos publicos en el inspector
    [Header("Panel de despliegue")]
    public CanvasGroup panelCanvasGroup;
    public float fadeDuration = 0.2f;

    [Header("Eventos Públicos")]
    public UnityEvent OnHoverEnter;
    public UnityEvent OnHoverExit;
    public UnityEvent OnClick;

    private void SetPanelVisibility(bool visible)
    {
        StopAllCoroutines();
        StartCoroutine(FadePanel(visible ? 1f : 0f));
    }

    private System.Collections.IEnumerator FadePanel(float targetAlpha)
    {
        float startAlpha = panelCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
        panelCanvasGroup.alpha = targetAlpha;
        panelCanvasGroup.interactable = targetAlpha == 1f;
        panelCanvasGroup.blocksRaycasts = targetAlpha == 1f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetPanelVisibility(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetPanelVisibility(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetPanelVisibility(true);
    }
}

