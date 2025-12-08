using UnityEngine;
using UnityEngine.Events;

public class ScrollDownUntilPoint : MonoBehaviour
{
    [field:SerializeField]
    private float SetLimitY;
    [field:SerializeField]
    private float Speed = 2;
    [field:SerializeField]
    private UnityEvent afterDeathEvent;
    private bool Started = false;
    void Update()
    {
        if (Started)
        {
            transform.position -= Vector3.down*Time.deltaTime*Speed;
        }
        if (transform.position.y <= SetLimitY)
        {
            afterDeathEvent?.Invoke();
            Destroy(gameObject);
        }
    }
    public void Activate()
    {
        Started = true;
    }
}
