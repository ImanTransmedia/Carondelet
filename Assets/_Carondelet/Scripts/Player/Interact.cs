using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [SerializeField]InputSystem_Actions inputActions;

    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayer;

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
    private void TryInteract()
    {
        // Raycast para detectar objetos interactuables
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            ItemDisplay item = hit.collider.GetComponent<ItemDisplay>();
            if (item != null)
            {
                item.OnInteract();
            }
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            UIIngameManager.Instance.ShowInteractPrompt();
        }
        else
        {
            UIIngameManager.Instance.HideInteractPrompt();
        }
    }
}
