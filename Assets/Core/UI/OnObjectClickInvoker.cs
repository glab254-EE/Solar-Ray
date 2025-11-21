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
    [field:SerializeField]
    private float ActivationDelay = -1;
    private bool activated = false;
    private InputSystem_Actions actions;
    void Start()
    {
        actions = new();
    }
    void Update()
    {
        if (activated == false && detector.IsHoveringOver == true && actions.Player.Attack.IsPressed() == true)
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
