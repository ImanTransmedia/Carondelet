using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [SerializeField]
    InputSystem_Actions inputActions;

    [SerializeField]
    private float interactRange = 3f;

    [SerializeField]
    private LayerMask interactableLayer;

    public FirstPersonMovement firstPerson;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.started += OnInteractPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.started -= OnInteractPerformed;
        inputActions.Player.Disable();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        //
        //if (context.phase == InputActionPhase.Performed)
        Debug.Log("Mandando Señal De Interaccion");
        TryInteract();
    }

    public void TryInteract()
    {
        // Raycast para detectar objetos interactuables
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            ItemDisplay item = hit.collider.GetComponent<ItemDisplay>();
            if (item != null)
            {
                item.OnInteract();
                if (firstPerson.isInteracting == false)
                {
                    firstPerson.isInteracting = true;
                }
                else
                {
                    firstPerson.isInteracting = false;
                }

            }
        }
    }

    public GameObject interactPrefab;
    private Transform currentTarget;
    private GameObject currentInstance;
    public Vector3 offsetAbove = new Vector3(0.1f, 0.1f, 0.3f);

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            UIIngameManager.Instance.ShowInteractPrompt();
            if (hit.transform != currentTarget)
            {
                DestroyCurrentInstance();
                currentTarget = hit.transform;
                currentInstance = Instantiate(interactPrefab);
                Bounds bounds = GetBounds(currentTarget.gameObject);
                Vector3 topPosition = bounds.center + Vector3.up * bounds.extents.y + offsetAbove;
                currentInstance.transform.position = topPosition;
                currentInstance.transform.localScale = interactPrefab.transform.localScale;
            }
        }
        else
        {
            UIIngameManager.Instance.HideInteractPrompt();
            DestroyCurrentInstance();
        }
    }

    void DestroyCurrentInstance()
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null;
        }
        currentTarget = null;
    }

    Bounds GetBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(obj.transform.position, Vector3.zero);

        foreach (Renderer rend in renderers)
        {
            bounds.Encapsulate(rend.bounds);
        }

        return bounds;
    }
}
