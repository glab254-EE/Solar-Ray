using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OnObjectClickInvoker : MonoBehaviour
{
    [field:SerializeField]
    private UnityEvent OnClickActions = new();
    [field:SerializeField]
    private float ActivationDelay = -1;
    [field:SerializeField]
    private HoverOverInterfaceGiver HoverOverChecker;
    private bool activated = false;
    private InputSystem_Actions actions;
    void Start()
    {
        actions = new();
    }
    void Update()
    {
        if (HoverOverChecker.IsHoveringOver == true && Mouse.current.leftButton.isPressed && !activated)
        {
            OnMouseDown();
        }
    }
    void OnMouseDown()
    {
        Debug.Log("Activated!");
        if (activated == false)
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
