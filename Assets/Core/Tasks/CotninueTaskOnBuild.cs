using UnityEngine;

public class CotninueTaskOnBuild : MonoBehaviour
{
    private TaskManager taskManager;
    void Start()
    {
        BuildingManager.instance.OnBuildEvent += Continue;
        taskManager = FindAnyObjectByType<TaskManager>();
    }
    void Continue()
    {
        taskManager.ContinueTask(0);
    }
}
