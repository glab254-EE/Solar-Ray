using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField, Tooltip("X axis for horizontal, Y vertical.")] // This time, there is only one vector2 value for both horizontal and vertical sensitivity, to keep code clean.
    private Vector2 CameraSensitivity;
    [SerializeField]
    private FloatRange CameraBounds;
    [SerializeField]
    private Transform playerTransform;
    internal bool CameraLock = true;
    private InputSystem_Actions inputActions;
    private float currentXAngle = 0f;
    private PlayerHealthBehaviour playerHealthBehaviour;
    void Start()
    {
        inputActions = new();
        ConnectInputs();
        playerHealthBehaviour = FindAnyObjectByType<PlayerHealthBehaviour>();
        if (playerHealthBehaviour != null)
        {
            playerHealthBehaviour.OnPlayerDeath += OnPlayerDeath;
        }
    }
    void Update()
    {
        
        if (CameraLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!CameraLock)
        {
            Cursor.lockState = CursorLockMode.None;            
        }
    }
    void OnDestroy()
    {
        CameraLock = false;
        inputActions.Player.Look.Disable();
        inputActions.Player.ToggleCameraLock.Disable();
    }
    private void OnPlayerDeath()
    {
        OnDestroy();
    }
    private void ConnectInputs()
    {
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.ToggleCameraLock.performed += OnCameraLockPress;
        inputActions.Player.ToggleCameraLock.Enable();
        inputActions.Player.Look.Enable();
    }
    internal void ToggleLock()
    {
        CameraLock = !CameraLock;
    }
    private void OnCameraLockPress(InputAction.CallbackContext callbackContext)
    {
        ToggleLock();
    }
    private void OnLook(InputAction.CallbackContext callbackContext)
    {
        Vector2 delta = callbackContext.ReadValue<Vector2>();
        if (delta != null && CameraLock)
        {
            playerTransform.Rotate(new Vector3(0, delta.x * CameraSensitivity.x * Time.deltaTime, 0));
            currentXAngle = Mathf.Clamp(currentXAngle - delta.y * CameraSensitivity.y * Time.deltaTime, CameraBounds.minimum, CameraBounds.maximum);
            transform.localRotation = Quaternion.Euler(currentXAngle, 0, 0);
        }
    }
}

[System.Serializable]
public struct FloatRange
{
    [SerializeField]
    public float minimum;
    [SerializeField]
    public float maximum;
}