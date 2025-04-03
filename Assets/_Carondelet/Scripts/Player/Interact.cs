using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    InputSystem_Actions inputActions;
    bool lookingAtInteractuable;

    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private void Awake()
    {
        lookingAtInteractuable = false;
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += OnActivate;
    }
    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Interact.performed -= OnActivate;
    }

    private void OnActivate(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("Mandando Señal De Interaccion");
            TryInteract();
        }
    }
    private void TryInteract()
    {
        // Raycast para detectar objetos interactuables
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnInteract();
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

    public interface IInteractable
    {
        void OnInteract();
    }

}
