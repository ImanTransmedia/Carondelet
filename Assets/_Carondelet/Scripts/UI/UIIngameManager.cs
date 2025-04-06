using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIIngameManager : MonoBehaviour
{
    public GameObject interactionText;

    public static UIIngameManager Instance { get; private set; }


    [Header("Item Display UI")]
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private RawImage item3DRender;

    [Header("3D Render Settings")]
    [SerializeField] private Vector3 modelRotationSpeed = new Vector3(0, 30, 0);
    [SerializeField] private LayerMask modelRenderLayer;
    [SerializeField] private Vector3 modelPositionOffset = new Vector3(0, 0, 2);

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
    }
    public void ShowInteractPrompt()
    {
        interactionText.SetActive(true);
    }
    public void HideInteractPrompt()
    {
        interactionText.SetActive(false);
    }

    private Camera modelRenderCamera;
    private GameObject currentModel;

    public void ShowItemPanel(string name, string description)
    {
        itemPanel.SetActive(true);
        itemNameText.text = name;
        itemDescriptionText.text = description;
    }

    public void HideItemPanel()
    {
        itemPanel.SetActive(false);
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
        // Configura cámara de renderizado
        if (modelRenderCamera == null)
        {
            CreateRenderCamera();
        }

        // Posiciona el modelo
        currentModel.transform.position = modelRenderCamera.transform.position + modelPositionOffset;
        currentModel.layer = LayerMask.NameToLayer("UI Model");

        // Asegura que solo esta cámara vea el modelo
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
}