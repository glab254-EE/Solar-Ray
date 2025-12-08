using UnityEngine;
using UnityEngine.Events;
public class OnObjectPlayerEntryInvoker : MonoBehaviour
{
    [field:SerializeField]
    private UnityEvent OnClickActions;
    [field:SerializeField]
    private GeneralPurposeEventBehaviour generalPurposeEventBehaviour;
    private bool activated = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false) return;
        if (activated == false)
        {
            activated = true;
            OnClickActions?.Invoke();
            if (generalPurposeEventBehaviour != null)
            {
                generalPurposeEventBehaviour.Trigger();
            }
        }
    }
}
