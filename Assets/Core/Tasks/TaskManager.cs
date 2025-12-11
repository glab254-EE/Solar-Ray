using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [field:SerializeField]
        public List<GameObject> ToRemove;
    }
    [field:SerializeField]
    private List<GameplayTask> tasks = new();
    [field:SerializeField]
    private int NextScene =-1;
    public string currentTaskMessage {get;internal set;}= "";
    private int CurrentScene = -1;
    private void Awake()
    {
        if (CurrentScene != -1 && CurrentScene != SceneManager.GetActiveScene().buildIndex)
        {
            return;
        }
        ContinueTask(0);
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
    }
    private void OnDestroy()
    {
        tasks = new();
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
            foreach(GameObject gameObject in nexttask.ToRemove)
            {
                if (gameObject != null)
                {
                    Destroy(gameObject);
                }
            }
        } else
        {
            if (NextScene != -1)
            {
                GameScenesManager.LoadScene(NextScene);
            }
        }
    }
}
