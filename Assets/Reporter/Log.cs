using System;
using UnityEngine;

public class Log{
    public string condition;
    public string stackTrace;
    public string type;
    public long time;
    public Log(){}
    public Log(string condition,string stackTrace,LogType type)
    {
        this.condition =condition;
        this.stackTrace = stackTrace;
        this.type = type.ToString();
        this.time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }
}