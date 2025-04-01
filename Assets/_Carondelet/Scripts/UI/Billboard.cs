using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    //Este script hace que el canva siempre mire hacia la camara
    void Start()
    {
        if (mainCamera == null)
        {
              mainCamera = Camera.main;
        }
      
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
    }
}
