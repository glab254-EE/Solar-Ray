using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [Serializable]
    public struct GameplayTask
    {
        [field:SerializeField]
        public string Message;
        [field:SerializeField]
        public GameObject AddedObject;
        public GameplayTask(string msg, GameObject obj)
        {
            Message = msg;
            AddedObject = obj;
        }
    }
    [field:SerializeField]
    private List<GameplayTask> tasks = new();
    internal string currentTaskMessage {get;private set;}= "";
    private void Awake()
    {
        ContinueTask();
    }
    internal void ContinueTask()
    {
        if (tasks.Count > 0)
        {
            GameplayTask nexttask = tasks[0]; 
            if (nexttask.AddedObject != null)
            {
                Instantiate(nexttask.AddedObject);
            }           
            currentTaskMessage = nexttask.Message;
            tasks.RemoveAt(0);
        }
    }
}
