using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ItemDisplay : MonoBehaviour
{
    [Header("Configuración del Objeto")]
    [SerializeField] private LocalizedString  itemName;
    [SerializeField] private LocalizedString  itemSubTitle;
    [SerializeField] private LocalizedString  itemDescription;
    [SerializeField] private GameObject item3DPrefab; // Prefab del modelo 3D

    [Header("Eventos")]
    public UnityEvent onDisplayStart;
    public UnityEvent onDisplayEnd;

    private GameObject current3DModel;
    private bool isUIOpen = false;

    public void OnInteract()
    {
        if (!isUIOpen)
        {
            ShowItemUI();
        }
        else
        {
            CloseItemUI();
        }
    }

    private void ShowItemUI()
    {
        string name = itemName.GetLocalizedString();
        string subTitle = itemSubTitle.GetLocalizedString();
        string description = itemDescription.GetLocalizedString();

        // Notifica al UI Manager
       UIIngameManager.Instance.ShowItemPanel(name, subTitle, description);

        // Crea el modelo 3D en la UI
        if (item3DPrefab != null)
        {
            current3DModel = Instantiate(item3DPrefab, Vector3.zero, Quaternion.identity);
            UIIngameManager.Instance.Set3DModelForUI(current3DModel);
        }

        isUIOpen = true;
        onDisplayStart?.Invoke();
    }

    private void CloseItemUI()
    {
        UIIngameManager.Instance.HideItemPanel();

        if (current3DModel != null)
        {
            Destroy(current3DModel);
        }

        isUIOpen = false;
        onDisplayEnd?.Invoke();
    }
}