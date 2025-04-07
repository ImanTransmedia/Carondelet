using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIngameManager : MonoBehaviour
{

     [SerializeField]
    private AccessibilityManager accessibilityManager;
    public GameObject interactionText;

    public static UIIngameManager Instance { get; private set; }

    [Header("Item Display UI")]
    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    [SerializeField]
    private Camera modelRenderCamera;

    [SerializeField]
    private float fadeDuration = 0.4f;

    [SerializeField]
    private GameObject itemPanel;

    [SerializeField]
    private TextMeshProUGUI itemNameText;

    [SerializeField]
    private TextMeshProUGUI itemSubTitleText;

    [SerializeField]
    private TextMeshProUGUI itemDescriptionText;

    [SerializeField]
    private RawImage item3DRender;

    [Header("3D Render Settings")]
    [SerializeField]
    private Vector3 modelRotationSpeed = new Vector3(0, 30, 0);

    [SerializeField]
    private LayerMask modelRenderLayer;

    [SerializeField]
    private Vector3 modelPositionOffset = new Vector3(0, 0, 2);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (itemPanel != null)
        {
            canvasGroup = itemPanel.GetComponent<CanvasGroup>();
        }
    }

    public void ShowInteractPrompt()
    {
        interactionText.SetActive(true);
    }

    public void HideInteractPrompt()
    {
        interactionText.SetActive(false);
    }


    private GameObject currentModel;

    public void ShowItemPanel(string name, string subTitle, string description)
    {
        accessibilityManager.RefreshAccessibilitySettings();
        accessibilityManager.OnPanelActivated();
        itemNameText.text = name;
        itemSubTitleText.text = subTitle;
        itemDescriptionText.text = description;
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, true));
    }

    public void HideItemPanel()
    {
         Debug.Log("panel 3D cerrado");
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, false));
        }

        if (currentModel != null)
        {
            Destroy(currentModel);
        }
    }

    public void Set3DModelForUI(GameObject model)
    {
        currentModel = model;
        ConfigureModelForRender();
    }

    private void ConfigureModelForRender()
    {
        if (modelRenderCamera == null)
        {
            CreateRenderCamera();
        }

        // Posicion del modelo
        currentModel.transform.position =
        modelRenderCamera.transform.position + modelPositionOffset;

        currentModel.layer = LayerMask.NameToLayer("UI Model");


        modelRenderCamera.cullingMask = modelRenderLayer;
    }

    private void CreateRenderCamera()
    {
        GameObject cameraGO = new GameObject("Model Render Camera");
        modelRenderCamera = cameraGO.AddComponent<Camera>();
        modelRenderCamera.transform.SetParent(item3DRender.transform);
        modelRenderCamera.targetTexture = item3DRender.texture as RenderTexture;
        modelRenderCamera.clearFlags = CameraClearFlags.SolidColor;
        modelRenderCamera.backgroundColor = Color.clear;
    }

    private void Update()
    {
        if (currentModel != null)
        {
            // Rota el modelo suavemente
            currentModel.transform.Rotate(modelRotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator FadeCanvasGroup(
        CanvasGroup group,
        float startAlpha,
        float endAlpha,
        bool activate
    )
    {
        if (activate)
            itemPanel.SetActive(true);

        float elapsed = 0f;
        group.alpha = startAlpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            yield return null;
        }

        group.alpha = endAlpha;

        if (!activate)
            itemPanel.SetActive(false);
    }
}
