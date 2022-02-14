using UnityEngine;
using System.Collections.Generic;
using System;
using Cysharp.Threading.Tasks;

public class TaskCrossManager : MonoBehaviour
{
    static TaskCrossManager _instance;
    public static TaskCrossManager instance{
        get{
            if (_instance == null)
            {
                GameObject go = new GameObject("TaskCrossManager");
                _instance = go.AddComponent<TaskCrossManager>();
            }
            return _instance;
        }
    }
    Dictionary<string,Func<string,string>> taskEvents = new Dictionary<string, Func<string, string>>();
    public void Register(string key,Func<string,string> func)
    {
        taskEvents[key] = func;
    }
    public void Remove(string key)
    {
        if(taskEvents.ContainsKey(key))
        {
            taskEvents.Remove(key);
        }
    }
    Queue<TaskCrossHandler> handles = new Queue<TaskCrossHandler>();
    void Update(){
        for(;;)
        {
            if(handles.Count == 0) break;
            TaskCrossHandler handler = handles.Dequeue();
            if(taskEvents.ContainsKey(handler.Key))
            {
                string returns = taskEvents[handler.Key].Invoke(handler.Args);
                handler.Callback(returns);
            }
        }
    }

    public UniTask<string> Call(string key,string args)
    {
        TaskCrossHandler handler = new TaskCrossHandler();
        UniTask<string> task = handler.Call(key,args);
        handles.Enqueue(handler);
        return task;
    }


    private class TaskCrossHandler
    {
        public UniTaskCompletionSource<string> Completion { get; private set; }
        public string Key { get;  private set; }
        public string Args { get;  private set; }
        // Methods
        public TaskCrossHandler() { }
        public UniTask<string> Call(string key, string args){
            if(Completion != null)
            {
                Completion.TrySetResult("");
            }
            Key = key;
            Args = args;
            Completion = new UniTaskCompletionSource<string>();
            return Completion.Task;
        }
        public void Callback(string returns){
            if(Completion != null)
            {
                Completion.TrySetResult(returns);
                Completion = null;
            }
        }

    }

}