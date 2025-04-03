using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AccessibilityManager : MonoBehaviour
{
    // Script central de accesibilidad

    [Header("Control Settings")]
    public bool enableAlternativeControls = false;
    public CanvasGroup defaultPanel;  // Panel con las instrucciones por defecto
    public CanvasGroup alternatePanel;  // Panel con las instrucciones para controles alternativos
    public float tooltipFadeDuration = 0.4f;

    [Header("UI Settings")]
    public List<Button> buttons; // Lista de botones para asignar teclas
    public List<KeyCode> keyAssignments; // Teclas asignadas para cada botón
  

    void Start()
    {
         UpdateButtonList();
        
        if (enableAlternativeControls)
        {
            AssignKeyBindings();
        }
        SetControlSettings();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateButtonList();

        if (enableAlternativeControls)
        {
            AssignKeyBindings();
        }
    }

  private void UpdateButtonList()
{
    buttons.Clear();

    GameObject[] buttonObjects = GameObject.FindGameObjectsWithTag("UIbutton");

    for (int i = 0; i < buttonObjects.Length; i++)
    {
        Button button = buttonObjects[i].GetComponent<Button>();

        if (button != null)
        {
            buttons.Add(button);

            // Buscar el TextMeshPro en los hijos del botón
            TextMeshProUGUI keyLabel = buttonObjects[i].GetComponentInChildren<TextMeshProUGUI>();

            if (keyLabel != null && i < keyAssignments.Count)
            {
                keyLabel.text = keyAssignments[i].ToString().Replace("Keypad", ""); // Remueve "Keypad" del nombre de la tecla
            }
        }
    }

    if (buttons.Count > keyAssignments.Count)
    {
        Debug.LogWarning("Hay más botones que teclas asignadas. Añade más teclas en el inspector.");
    }
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
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
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
            if (Input.GetKeyDown(keyAssignments[i]) || Input.GetKeyDown(ConvertKeypadToAlpha(keyAssignments[i])))
            {
                buttons[i].onClick.Invoke();
            }
        }
    }
}


    private void ToggleKeyLabels(bool state)
{
    foreach (Button button in buttons)
    {
        Transform keyLabel = button.transform.Find("keyLabel"); // Busca el hijo llamado "keyLabel"

        if (keyLabel != null)
        {
            keyLabel.gameObject.SetActive(state); // Activa o desactiva según el modo de accesibilidad
        }
    }
}

private KeyCode ConvertKeypadToAlpha(KeyCode key)
{
    switch (key)
    {
        case KeyCode.Keypad0: return KeyCode.Alpha0;
        case KeyCode.Keypad1: return KeyCode.Alpha1;
        case KeyCode.Keypad2: return KeyCode.Alpha2;
        case KeyCode.Keypad3: return KeyCode.Alpha3;
        case KeyCode.Keypad4: return KeyCode.Alpha4;
        case KeyCode.Keypad5: return KeyCode.Alpha5;
        case KeyCode.Keypad6: return KeyCode.Alpha6;
        case KeyCode.Keypad7: return KeyCode.Alpha7;
        case KeyCode.Keypad8: return KeyCode.Alpha8;
        case KeyCode.Keypad9: return KeyCode.Alpha9;
        default: return key;
    }
}
    // Alternar entre los controles alternativos y los controles predeterminados
    public void ToggleControlScheme()
    {
        enableAlternativeControls = !enableAlternativeControls;
        SetControlSettings();
    }


    // Corutina para alternar entre los paneles con el efecto de Fade
    private IEnumerator SwitchPanelsWithFade()
    {
        // Desactivar el panel actual (Fade Out)
        if (defaultPanel.alpha > 0f)
        {
            yield return StartCoroutine(FadeOut(defaultPanel));
        }
        else if (alternatePanel.alpha > 0f)
        {
            yield return StartCoroutine(FadeOut(alternatePanel));
        }

        // Alternar entre los paneles
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

        // Activar el panel alternativo con Fade In
        if (enableAlternativeControls)
        {
            yield return StartCoroutine(FadeIn(alternatePanel));
        }
        else
        {
            yield return StartCoroutine(FadeIn(defaultPanel));
        }
    }

    // Fade In para el CanvasGroup
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

    // Fade Out para el CanvasGroup
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
        UpdateButtonList();  // Vuelve a buscar los botones
        AssignKeyBindings(); // Reasigna las teclas a los botones
        Debug.Log("Configuración de accesibilidad actualizada.");
    }
}
