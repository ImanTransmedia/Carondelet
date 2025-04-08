using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class IntereactiveTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CanvasGroup panelCanvasGroup;
    public TextMeshProUGUI targetText;
    public float fadeDuration = 0.2f;

    public Transform contenidoTransform;
    
    public UnityEvent OnHoverEnter;
    public UnityEvent OnHoverExit;
    public UnityEvent OnClick;
    public List<GameObject> buttonList;


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

    private string GetTextFromContenidoChild()
    {
        if (contenidoTransform != null)
        {
            TextMeshProUGUI contenidoTMP = contenidoTransform.GetComponent<TextMeshProUGUI>();

            if (contenidoTMP != null)
            {
                return contenidoTMP.text;
            }
        }

        return string.Empty;
    }

    public void SetTargetTextFromContenido()
    {
        if (targetText != null)
        {
                targetText.text = GetTextFromContenidoChild();
                for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i] != null)
                {   
                    buttonList[i].SetActive(i == 0);
                }
            }
        }
    }
}
