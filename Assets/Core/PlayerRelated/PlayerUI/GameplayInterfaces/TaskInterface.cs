using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TaskInterface : MonoBehaviour
{
    private TaskManager manager;
    private TMP_Text textLabel;
    void Start()
    {
        textLabel = GetComponent<TMP_Text>();
        manager = FindAnyObjectByType<TaskManager>();
        if (manager == null)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        textLabel.text = manager.currentTaskMessage;
    }
}
