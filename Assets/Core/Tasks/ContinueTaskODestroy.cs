using UnityEngine;

public class ContinueTaskODestroy : MonoBehaviour // OLD CODE, USE OnObjectDestroyInvoker INSTEAD.
{
    [field:SerializeField]
    private int priority = 0; 
    private void OnDestroy()
    {            
        TaskManager potentialManager = FindAnyObjectByType<TaskManager>();
        if (potentialManager != null)
        {
            potentialManager.ContinueTask(priority);
        }
    }
}
