using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OnObjectClickInvoker : MonoBehaviour
{
    [field:SerializeField]
    private HoverOverInterfaceGiver detector;
    [field:SerializeField]
    private UnityEvent OnClickActions = new();
    [field:SerializeField,Tooltip("To make it one-time use, make it -1")]
    private float ActivationDelay = -1;
    private bool activated = false;
    private InputSystem_Actions actions;
    void Start()
    {
        actions = new();
    }
    void Update()
    {
        if (!activated && detector.IsHoveringOver && actions.Player.Attack.ReadValue<bool>())
        {
            OnClickActions?.Invoke();
            StartCoroutine(ActivationEnumerator());
        }
    }
    private IEnumerator ActivationEnumerator()
    {
        activated = true;
        if (ActivationDelay > 0)
        {
            yield return new WaitForSeconds(ActivationDelay);
            activated = false;
        }
    }
}
