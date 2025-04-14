using UnityEngine;
using UnityEngine.EventSystems;

public class RotateWithDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float force = 10f;
    private Rigidbody rb;
    private bool isDragging = false;
    private Vector2 dragDelta;

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
    }
}
