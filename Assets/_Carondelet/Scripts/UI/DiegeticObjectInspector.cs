using UnityEngine;
using System.Collections;

public class DiegeticObjectInspector : MonoBehaviour
{
    [Header("Camera configuration")]
    public Transform cameraTransform;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float zoomDistance = 2f;

    [Header("FOV Zoom Settings")]
    public float initialFOV = 60f;
    public float zoomFOV = 40f;
    public float fovTransitionDuration = 0.5f;

    private Camera cam;

    [Header("Configuracion entrada")]
    public float duration = 2f;
    public float speed = 0.25f;

    [Header("Diorama configuration")]
    public GameObject diorama;

    [Header("UI Panels")]
    public CanvasGroup panelAreaExterior;
    public CanvasGroup panelAreaSeleccionada;
    public float fadeDuration = 0.4f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public bool isReturning = false;

    void Start()
    {
        if (cameraTransform == null) return;

        cam = cameraTransform.GetComponent<Camera>();
        if (cam == null)
        {
            cam = cameraTransform.GetComponentInChildren<Camera>();
        }

        if (cam != null)
        {
            cam.fieldOfView = initialFOV;
        }

        initialPosition = cameraTransform.position;
        initialRotation = cameraTransform.rotation;
    }

    public void InspectObject(Transform target)
    {
        if (isReturning || cameraTransform == null) return;
        StopAllCoroutines();
        StartCoroutine(MoveToTarget(target));
        StartCoroutine(SwitchPanelsWithFade(panelAreaExterior, panelAreaSeleccionada));

        if (cam != null)
            StartCoroutine(ChangeFOV(cam.fieldOfView, zoomFOV, fovTransitionDuration));
    }

    public void ResetCamera()
    {
        if (cameraTransform == null) return;
        StopAllCoroutines();
        StartCoroutine(ReturnToStart());
        StartCoroutine(SwitchPanelsWithFade(panelAreaSeleccionada, panelAreaExterior));

        if (cam != null)
            StartCoroutine(ChangeFOV(cam.fieldOfView, initialFOV, fovTransitionDuration));
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

    private IEnumerator SwitchPanelsWithFade(CanvasGroup fadeOutPanel, CanvasGroup fadeInPanel)
    {
        yield return StartCoroutine(FadeOut(fadeOutPanel));
        fadeOutPanel.gameObject.SetActive(false);

        fadeInPanel.gameObject.SetActive(true);
        yield return StartCoroutine(FadeIn(fadeInPanel));
    }

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

    private IEnumerator ChangeFOV(float startFOV, float endFOV, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (cam != null)
            {
                cam.fieldOfView = Mathf.Lerp(startFOV, endFOV, elapsed / duration);
                elapsed += Time.deltaTime;
            }
            yield return null;
        }
        if (cam != null)
            cam.fieldOfView = endFOV;
    }

    public void MoveCameraForward()
    {
        if (cameraTransform == null) return;
        StopAllCoroutines();
        StartCoroutine(MoveCameraForwardCoroutine(duration, speed));
    }

    private IEnumerator MoveCameraForwardCoroutine(float duration, float speed)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            cameraTransform.position += cameraTransform.forward * speed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
