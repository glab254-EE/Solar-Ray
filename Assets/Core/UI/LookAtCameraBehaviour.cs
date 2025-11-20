using UnityEngine;

public class LookAtCameraBehaviour : MonoBehaviour
{
    private Transform cameraTransform;
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    void FixedUpdate()
    {
        transform.rotation =cameraTransform.rotation;
    }
}
