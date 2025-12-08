using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OnButtonClickInvoker : MonoBehaviour
{
    [field:SerializeField]
    private UnityEvent OnClickActions = new();
    [field:SerializeField]
    private float ActivationDelay = -1;
    private bool activated = false;
    private InputSystem_Actions actions;
    void Start()
    {
        actions = new();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnMouseDown);
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
