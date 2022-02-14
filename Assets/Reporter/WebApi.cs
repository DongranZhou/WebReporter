

using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class WebApi
{
    public static string Root
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/web";
            }
            else
            {
                return Application.streamingAssetsPath + "/web";
            }
        }
    }

    public static async UniTask RequestScene(HttpListenerContext context)
    {
        string json = await TaskCrossManager.instance.Call("scene", context.Request.Url.Query);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.ContentLength64 = bytes.Length;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 200;
        context.Response.OutputStream.Write(bytes, 0, bytes.Length);
    }

    public static async UniTask RequestLog(HttpListenerContext context)
    {
        string json = await TaskCrossManager.instance.Call("log", context.Request.Url.Query);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.ContentLength64 = bytes.Length;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 200;
        context.Response.OutputStream.Write(bytes, 0, bytes.Length);
    }

    public static async UniTask RequestConfig(HttpListenerContext context)
    {
        if (context.Request.HttpMethod == "GET")
        {
            string json = await TaskCrossManager.instance.Call("config", context.Request.Url.Query);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.ContentLength64 = bytes.Length;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }
        else if (context.Request.HttpMethod == "POST")
        {
            string input = await AsRequestString(context.Request);
            string json = await TaskCrossManager.instance.Call("saveconfig", input);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.ContentLength64 = bytes.Length;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }
    }
    public static async UniTask RequestFiles(HttpListenerContext context)
    {

        string local = Uri.UnescapeDataString(context.Request.Url.LocalPath);
        if (string.IsNullOrEmpty(local) || local == "/")
            local = "/index.html";

        string file = Root + local;
        if (File.Exists(file))
        {
            using (FileStream stream = new FileStream(file, FileMode.Open))
            {
                context.Response.ContentLength64 = stream.Length;
                context.Response.ContentType = MimeMapping.GetMimeType(file);
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.StatusCode = 200;

                await stream.CopyToAsync(context.Response.OutputStream);
            }
        }
    }
    static async UniTask<string> AsRequestString(HttpListenerRequest request)
    {
        long length = request.ContentLength64;
        byte[] bytes = new byte[length];
        int count = await request.InputStream.ReadAsync(bytes,0,bytes.Length);
        return request.ContentEncoding.GetString(bytes,0,count);
    }
}
