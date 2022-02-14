using UnityEngine;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class ReporterMonoBehaviour : MonoBehaviour
{
    #region unity
    void Start()
    {
        TaskCrossManager.instance.Register("scene", new Func<string, string>(this.OnScene));
        TaskCrossManager.instance.Register("log", new Func<string, string>(this.OnLog));
        TaskCrossManager.instance.Register("config", new Func<string, string>(this.OnConfig));
        TaskCrossManager.instance.Register("saveconfig", new Func<string, string>(this.OnSaveConfig));
        Application.logMessageReceived += new Application.LogCallback(this.CaptureLog);
        StartServer();
    }
    private void OnDestroy()
    {
        StopServer();
    }
    #endregion

    #region http
    private HttpListener listener;
    private void StartServer()
    {
        try
        {
            if (this.listener == null)
            {
                this.listener = new HttpListener();
                this.listener.Prefixes.Add("http://+:8080/");
                this.listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                this.listener.Start();
                UniTask task = this.Receive();
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private async UniTask Receive()
    {
        for (; ; )
        {
            try
            {
                HttpListenerContext context = await listener.GetContextAsync();
                //context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                //context.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
                //context.Response.AppendHeader("Access-Control-Allow-Headers", " Origin, X-Requested-With, Content-Type, Accept");
                //context.Response.AppendHeader("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
                //context.Response.AppendHeader("Content-Type", "application/json;charset=utf-8");
                //context.Response.AppendHeader("Access-Control-Max-Age", "2592000");
                
                string local = Uri.UnescapeDataString(context.Request.Url.LocalPath);
                if (local == "/scene")
                    await WebApi.RequestScene(context);
                else if (local == "/log")
                    await WebApi.RequestLog(context);
                else if (local == "/config")
                    await WebApi.RequestConfig(context);
                else
                    await WebApi.RequestFiles(context);
                context.Response.Close();

                if (listener == null) break;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                break;
            }
        }
    }

    private void StopServer()
    {
        try
        {
            if (this.listener != null)
            {
                this.listener.Close();
                this.listener = null;
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private Dictionary<string, string> ParseQuery(string query)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(query) && (query.IndexOf("?") == 0))
        {
            char[] trimChars = new char[] { '?' };
            char[] separator = new char[] { '&' };
            string[] strArray = query.TrimStart(trimChars).Split(separator);
            foreach (string str in strArray)
            {
                char[] chArray3 = new char[] { '=' };
                string[] strArray3 = str.Split(chArray3);
                if (strArray3.Length == 2)
                {
                    dictionary[strArray3[0]] = strArray3[1];
                }
            }
        }
        return dictionary;
    }

    #endregion

    #region scene
    private string OnScene(string args) {
        return Scene2Json().ToString();
    }
    private JSONObject Scene2Json()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootGameObjects = activeScene.GetRootGameObjects();
        JSONArray array = new JSONArray();
        for (int i = 0; i < rootGameObjects.Length; i++)
        {
            Transform root = rootGameObjects[i].transform;
            JSONArray child = this.EachChildren(root);

            JSONObject obj = new JSONObject();
            obj.Add(root.name, child);

            array.Add(obj);
        }
        JSONObject json = new JSONObject();
        json.Add(activeScene.name, array);
        return json;
    }
    private JSONArray EachChildren(Transform root)
    {
        JSONArray array = new JSONArray();
        for (int i = 0; i < root.transform.childCount; i++)
        {
            Transform child = root.transform.GetChild(i);
            JSONArray children = EachChildren(child);
            JSONObject obj = new JSONObject();
            obj.Add(child.name, children);
            array.Add(obj);
        }
        return array;
    }

    #endregion

    #region log
    private void CaptureLog(string condition, string stacktrace, LogType type)
    {
        Log item = new Log(condition, stacktrace, type);
        logs.Add(item);
    }
    private List<Log> logs = new List<Log>();
    private string OnLog(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return LogArray2Json(this.logs).ToString();
        }
        Dictionary<string, string> dictionary = this.ParseQuery(query);
        if (dictionary.ContainsKey("started") && !dictionary.ContainsKey("ended"))
        {
            long started = long.Parse(dictionary["started"]);
            Log[] array = this.logs.Where(x => { return x.time > started; }).ToArray();
            return LogArray2Json(array).ToString();
        }
        if (!dictionary.ContainsKey("started") && dictionary.ContainsKey("ended"))
        {
            long ended = long.Parse(dictionary["ended"]);
            Log[] array = logs.Where(x => { return x.time < ended; }).ToArray();
            return LogArray2Json(array).ToString();
        }
        if (dictionary.ContainsKey("started") && dictionary.ContainsKey("ended"))
        {
            long started = long.Parse(dictionary["started"]);
            long ended = long.Parse(dictionary["ended"]);
            Log[] array = this.logs.Where(x => { return x.time > started && x.time < ended; }).ToArray();
            return LogArray2Json(array).ToString();
        }
        return LogArray2Json(this.logs).ToString();
    }
    public JSONNode LogArray2Json(ICollection<Log> logs)
    {
        JSONArray array = new JSONArray();
        foreach (Log log in logs)
        {
            JSONObject aItem = new JSONObject();
            aItem.Add("type", log.type);
            aItem.Add("condition", log.condition);
            aItem.Add("stackTrace", log.stackTrace);
            aItem.Add("time", log.time);
            array.Add(aItem);
        }
        return array;
    }

    #endregion

    #region config
    private string OnConfig(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            string[] configs = ConfigManager.GetConfigs();
            JSONArray array = new JSONArray();
            foreach (string str in configs)
            {
                array.Add(str);
            }
            return array.ToString();
        }
        Dictionary<string, string> dictionary = this.ParseQuery(query);
        if (dictionary.ContainsKey("config"))
        {
            string name = dictionary["config"];
            if (ConfigManager.Has(name))
            {
                return ConfigManager.Read(name);
            }
            return ("not config " + name);
        }
        return "not param config";
    }

    private string OnSaveConfig(string arg)
    {
        if (!string.IsNullOrEmpty(arg))
        {
            JSONNode json = JSON.Parse(arg);
            foreach (KeyValuePair<string, JSONNode> node in json)
            {
                ConfigManager.Write(node.Key, node.Value.ToString());
            }
            return string.Join(",", json.ToString());
        }
        return "post not data";
    }

    #endregion
}