using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerHealthBehaviour))]
public class PlayerMovementBehaviour : MonoBehaviour
{
    [SerializeField, Tooltip("Attribute for base walking speed")]
    private float baseMovementSpeed = 15f;
    [SerializeField, Tooltip("Attribute for running speed")]
    private float baseRunSpeedMultiplier = 1.5f;
    [SerializeField, Tooltip("Attribute for speed change")]
    private float velocityChangeSpeed = .2f;
    [SerializeField, Tooltip("Attribute for base jump power")]
    private float jumpPower = 15f;
    internal bool IsMovementEnabled = true;
    private bool IsRunning = false;
    private PlayerMovementInvoker invoker;
    private Vector3 currentVelocity;
    private Vector3 targetVelocity;
    private InputSystem_Actions inputActions;
    private PlayerHealthBehaviour playerHealthBehaviour;
    void Start()
    {
        inputActions = new();
        invoker = new(gameObject.GetComponent<Rigidbody>());
        currentVelocity = Vector3.zero;
        targetVelocity = Vector3.zero;
        ConnectInputs();
        inputActions.Player.Move.Enable();
        inputActions.Player.Sprint.Enable();
        inputActions.Player.Jump.Enable();
        playerHealthBehaviour = GetComponent<PlayerHealthBehaviour>();
        playerHealthBehaviour.OnPlayerDeath += OnPlayerDeath;
    }
    void FixedUpdate()
    {
        HandleMovement();
    }
    void OnDestroy()
    {
        inputActions.Player.Move.Disable();
        inputActions.Player.Sprint.Disable();
        inputActions.Player.Disable();
    }
    private void OnPlayerDeath()
    {
        IsMovementEnabled = false;
        OnDestroy();
    }
    private void HandleMovement()
    {
        if (IsMovementEnabled)
        {
            Vector2 reading = inputActions.Player.Move.ReadValue<Vector2>();
            targetVelocity = transform.forward * reading.y + transform.right * reading.x;
            targetVelocity *= baseMovementSpeed;
            if (IsRunning && reading.y > 0)
            {
                targetVelocity *= baseRunSpeedMultiplier;
            }
        }
        else
        {
            ResetMovementStatus();
        }
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, velocityChangeSpeed);
        invoker.MovePlayer(currentVelocity);
    }
    private void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (Physics.Raycast(
            new Ray(
                transform.position
                + Vector3.down
                * transform.localScale.y,
                Vector3.down * .5f),

             0.5f, LayerMask.NameToLayer("Surface")))
        {
            invoker.JumpPlayer(jumpPower);
        }
    }
    private void ResetMovementStatus()
    {
        targetVelocity = Vector3.zero;
    }
    private void ConnectInputs()
    {
        inputActions.Player.Sprint.performed += OnRunButtonPress;
        inputActions.Player.Sprint.canceled += OnRunButtonPress;
        inputActions.Player.Jump.performed += OnJump;
    }
    private void OnRunButtonPress(InputAction.CallbackContext callbackContext)
    {
        IsRunning = callbackContext.ReadValueAsButton();
    }
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.down * transform.localScale.y, transform.position + Vector3.down * transform.localScale.y + new Vector3(0, -0.5f, 0));
    }
#endif
}
