using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccessibilityManager : MonoBehaviour
{
    public bool enableAlternativeControls = false;
    public CanvasGroup defaultPanel;
    public CanvasGroup alternatePanel;
    public float tooltipFadeDuration = 0.4f;

    public float repeatDelay = 0.5f;

    public List<Button> buttons;
    public List<KeyCode> keyAssignments;

    void Start()
    {
        UpdateButtonList();
        if (enableAlternativeControls)
            AssignKeyBindings();
            SetControlSettings();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshAccessibilitySettings();
    }

    private void UpdateButtonList()
    {
        buttons.Clear();
        GameObject[] buttonObjects = GameObject.FindGameObjectsWithTag("UIbutton");

        for (int i = 0; i < buttonObjects.Length; i++)
        {
            GameObject obj = buttonObjects[i];

            if (!obj.activeInHierarchy)
                continue;

            Button button = obj.GetComponent<Button>();

            if (button != null)
            {
                buttons.Add(button);

                Transform keyLabelTransform = obj.transform.Find("keyLabel");

                if (keyLabelTransform != null && i < keyAssignments.Count)
                {
                    TextMeshProUGUI keyLabelText =
                        keyLabelTransform.GetComponentInChildren<TextMeshProUGUI>();
                    if (keyLabelText != null)
                    {
                        keyLabelText.text = keyAssignments[i].ToString().Replace("Keypad", "");
                    }
                }
            }
        }

        if (buttons.Count > keyAssignments.Count)
        {
            Debug.LogWarning(
                "Hay mas botones que teclas asignadas"
            );
        }

        Invoke(nameof(ExecuteUpdateButtonListOnceMore), 0.5f);
    }

    private void ExecuteUpdateButtonListOnceMore()
    {
        UpdateButtonList(); 
    }

    private void SetControlSettings()
    {
        if (enableAlternativeControls)
        {
            Debug.Log("Controles alternativos activados");
            AssignKeyBindings();
        }
        else
        {
            Debug.Log("Controles predeterminados activados");
            ClearKeyBindings();
        }

        ToggleKeyLabels(enableAlternativeControls);
    }

    private void AssignKeyBindings()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        CancelInvoke(nameof(AssignKeyBindings));
        Invoke(nameof(AssignKeyBindings), repeatDelay);
    }

    private void ClearKeyBindings()
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void OnButtonClick(int buttonIndex)
    {
        Debug.Log($"Botón {buttons[buttonIndex].name} presionado");

        Invoke(nameof(RefreshAccessibilitySettings), 0.1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleControlScheme();
            StartCoroutine(SwitchPanelsWithFade());
        }

        if (enableAlternativeControls)
        {
            for (int i = 0; i < keyAssignments.Count; i++)
            {
                if (
                    Input.GetKeyDown(keyAssignments[i])
                    || Input.GetKeyDown(ConvertKeypadToAlpha(keyAssignments[i]))
                )
                {
                    if (i < buttons.Count)
                        buttons[i].onClick.Invoke();
                }
            }
        }
    }

    private void ToggleKeyLabels(bool state)
    {
        foreach (Button button in buttons)
        {
            Transform keyLabel = button.transform.Find("keyLabel");

            if (keyLabel != null)
            {
                keyLabel.gameObject.SetActive(state);
            }
        }
    }

    private KeyCode ConvertKeypadToAlpha(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Keypad0:
                return KeyCode.Alpha0;
            case KeyCode.Keypad1:
                return KeyCode.Alpha1;
            case KeyCode.Keypad2:
                return KeyCode.Alpha2;
            case KeyCode.Keypad3:
                return KeyCode.Alpha3;
            case KeyCode.Keypad4:
                return KeyCode.Alpha4;
            case KeyCode.Keypad5:
                return KeyCode.Alpha5;
            case KeyCode.Keypad6:
                return KeyCode.Alpha6;
            case KeyCode.Keypad7:
                return KeyCode.Alpha7;
            case KeyCode.Keypad8:
                return KeyCode.Alpha8;
            case KeyCode.Keypad9:
                return KeyCode.Alpha9;
            default:
                return key;
        }
    }

    public void ToggleControlScheme()
    {
        enableAlternativeControls = !enableAlternativeControls;
        SetControlSettings();
    }

    private IEnumerator SwitchPanelsWithFade()
    {
        if (defaultPanel.alpha > 0f)
        {
            yield return StartCoroutine(FadeOut(defaultPanel));
        }
        else if (alternatePanel.alpha > 0f)
        {
            yield return StartCoroutine(FadeOut(alternatePanel));
        }

        if (enableAlternativeControls)
        {
            defaultPanel.alpha = 0f;
            alternatePanel.alpha = 1f;
        }
        else
        {
            alternatePanel.alpha = 0f;
            defaultPanel.alpha = 1f;
        }

        if (enableAlternativeControls)
        {
            yield return StartCoroutine(FadeIn(alternatePanel));
        }
        else
        {
            yield return StartCoroutine(FadeIn(defaultPanel));
        }
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        canvasGroup.gameObject.SetActive(true);
        while (elapsedTime < tooltipFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / tooltipFadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < tooltipFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / tooltipFadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }

    public void RefreshAccessibilitySettings()
    {
        UpdateButtonList();
        AssignKeyBindings();
        ToggleKeyLabels(enableAlternativeControls);
        CancelInvoke(nameof(RefreshAccessibilitySettings));
        Invoke(nameof(RefreshAccessibilitySettings), repeatDelay);
    }

    public void OnPanelActivated()
    {
        RefreshAccessibilitySettings();
        ToggleKeyLabels(enableAlternativeControls);
    }
}
