using UnityEngine;
using UnityEngine.EventSystems;

public class RotateWithDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float force = 10f;
    private Rigidbody rb;
    private bool isDragging = false;
    private Vector2 dragDelta;

    [Header("Rotation Limits")]
    public bool limitX = true;
    public float minX = -30f;
    public float maxX = 30f;

    public bool limitY = true;
    public float minY = -60f;
    public float maxY = 60f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        dragDelta = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragDelta = eventData.delta;
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 torque = new Vector3(dragDelta.y, -dragDelta.x, 0f);
            rb.AddTorque(torque * force * Time.fixedDeltaTime);
        }

        ClampRotation();
    }

    void ClampRotation()
    {
        Vector3 currentRotation = transform.localEulerAngles;

        currentRotation.x = NormalizeAngle(currentRotation.x);
        currentRotation.y = NormalizeAngle(currentRotation.y);

        if (limitX)
            currentRotation.x = Mathf.Clamp(currentRotation.x, minX, maxX);

        if (limitY)
            currentRotation.y = Mathf.Clamp(currentRotation.y, minY, maxY);

       
        transform.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0f);
        rb.angularVelocity = Vector3.zero; 
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }
}
