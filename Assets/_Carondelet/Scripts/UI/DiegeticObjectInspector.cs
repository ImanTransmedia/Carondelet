using UnityEngine;

public class DiegeticObjectInspector : MonoBehaviour
{
    //Este script sirve para rotar objetos en 3d con el mouse o con input touch
     [Header("Rotation Settings")]
    public float rotationSpeed = 5f;
    public float minXRotation = -45f;
    public float maxXRotation = 45f;
    public float minYRotation = -45f;
    public float maxYRotation = 45f;
    public float smoothX = 5f;
    public float smoothY = 5f;

    private bool isDragging = false;
    private Vector2 currentRotation;
    private Vector2 targetRotation;
    private Vector2 lastMousePosition;

    void Start()
    {
        targetRotation = transform.eulerAngles;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mouseDelta = (Vector2)Input.mousePosition - lastMousePosition;
            
            // Calculamos la nueva rotación basada en el movimiento del mouse
            targetRotation.x -= mouseDelta.y * rotationSpeed * Time.deltaTime;
            targetRotation.y += mouseDelta.x * rotationSpeed * Time.deltaTime;

            // Limitar la rotación
            targetRotation.x = Mathf.Clamp(targetRotation.x, minXRotation, maxXRotation);
            targetRotation.y = Mathf.Clamp(targetRotation.y, minYRotation, maxYRotation);

            // Suavizar la rotación
            currentRotation.x = Mathf.Lerp(currentRotation.x, targetRotation.x, smoothX * Time.deltaTime);
            currentRotation.y = Mathf.Lerp(currentRotation.y, targetRotation.y, smoothY * Time.deltaTime);

            // Aplicar la rotación al objeto
            transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, transform.eulerAngles.z);

            // Actualizar la posición del mouse
            lastMousePosition = Input.mousePosition;
        }
    }

    void OnMouseDown()
    {
        // Imprime el nombre del objeto 3D cuando se hace clic sobre él
        Debug.Log("Objeto 3D seleccionado: " + gameObject.name);

        isDragging = true;
        lastMousePosition = Input.mousePosition; // Guardar la posición inicial del mouse
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
