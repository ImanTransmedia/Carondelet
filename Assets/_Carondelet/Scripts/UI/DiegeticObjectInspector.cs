using UnityEngine;
using System.Collections;

public class DiegeticObjectInspector : MonoBehaviour
{
    //Este script sirve para la interaccion con el diorama
    [Header("Camera configuration")]
    public Transform cameraTransform;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float zoomDistance = 2f;

    [Header("Diorama configuration")]
    public GameObject diorama;
    public float rotationDuration = 1f;
    public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine rotationCoroutine;

    [Header("UI Panels")]
    public CanvasGroup panelAreaExterior; // Panel por defecto
    public CanvasGroup panelAreaSeleccionada; // Panel cuando se inspecciona un objeto
    public float fadeDuration = 0.4f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public bool isReturning = false;

    void Start()
    {
        if (cameraTransform == null) return;
        initialPosition = cameraTransform.position;
        initialRotation = cameraTransform.rotation;
    }

    // Mueve la cámara para inspeccionar un objeto
    public void InspectObject(Transform target)
    {
        if (isReturning || cameraTransform == null) return;
        StopAllCoroutines();
        StartCoroutine(MoveToTarget(target));
        StartCoroutine(SwitchPanelsWithFade(panelAreaExterior, panelAreaSeleccionada));
        RotateObject();
    }

    // Restablece la cámara a su posición inicial
    public void ResetCamera()
    {
        if (cameraTransform == null) return;
        StopAllCoroutines();
        StartCoroutine(ReturnToStart());
        StartCoroutine(SwitchPanelsWithFade(panelAreaSeleccionada, panelAreaExterior));
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        if (cameraTransform == null) yield break;

        Vector3 targetPosition = target.position - target.forward * zoomDistance;
        Quaternion targetRotation = Quaternion.LookRotation(target.position - targetPosition);

        while (Vector3.Distance(cameraTransform.position, targetPosition) > 0.01f)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, moveSpeed * Time.deltaTime);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        cameraTransform.position = targetPosition;
        cameraTransform.rotation = targetRotation;
    }

    private IEnumerator ReturnToStart()
    {
        if (cameraTransform == null) yield break;

        isReturning = true;

        while (Vector3.Distance(cameraTransform.position, initialPosition) > 0.01f)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, initialPosition, moveSpeed * Time.deltaTime);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, initialRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        cameraTransform.position = initialPosition;
        cameraTransform.rotation = initialRotation;
        isReturning = false;
    }

    // Corutina para alternar entre los paneles con Fade
    private IEnumerator SwitchPanelsWithFade(CanvasGroup fadeOutPanel, CanvasGroup fadeInPanel)
    {
        yield return StartCoroutine(FadeOut(fadeOutPanel));
        fadeOutPanel.gameObject.SetActive(false);

        fadeInPanel.gameObject.SetActive(true);
        yield return StartCoroutine(FadeIn(fadeInPanel));
    }

    // Fade In para un CanvasGroup
    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    // Fade Out para un CanvasGroup
    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }


     public void RotateObject()
    {
        if (diorama == null) return;

        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(RotateOverTime());
    }

    private IEnumerator RotateOverTime()
    {
        float elapsed = 0f;
        float startY = diorama.transform.eulerAngles.y;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            float angle = rotationCurve.Evaluate(t) * 360f;
            float currentY = startY + angle;

            Vector3 currentEuler = diorama.transform.eulerAngles;
            diorama.transform.eulerAngles = new Vector3(currentEuler.x, currentY, currentEuler.z);

            yield return null;
        }

        // Asegurar que termine exactamente en el ángulo final
        float finalY = startY + 360f;
        Vector3 finalEuler = diorama.transform.eulerAngles;
        diorama.transform.eulerAngles = new Vector3(finalEuler.x, finalY, finalEuler.z);

        rotationCoroutine = null;
    }
}
