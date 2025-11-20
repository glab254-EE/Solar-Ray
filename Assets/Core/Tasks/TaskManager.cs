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
        public int Priority;
        [field:SerializeField]
        public GameObject AddedObject;
        public GameplayTask(string msg, GameObject obj,int priority)
        {
            Message = msg;
            AddedObject = obj;
            Priority = priority;
        }
    }
    [field:SerializeField]
    private List<GameplayTask> tasks = new();
    internal string currentTaskMessage {get;private set;}= "";
    private void Awake()
    {
        ContinueTask(0);
    }
    internal void ContinueTask(int priority)
    {
        if (tasks.Count > 0)
        {
            GameplayTask nexttask = tasks[0]; 
            if (nexttask.Priority > priority) return;
            if (nexttask.AddedObject != null)
            {
                Instantiate(nexttask.AddedObject);
            }           
            currentTaskMessage = nexttask.Message;
            tasks.RemoveAt(0);
        } else
        {
            // TODO: ADD SCENE MANAGER
        }
    }
}
