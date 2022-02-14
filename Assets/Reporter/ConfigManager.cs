using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class ConfigManager
{
	public static string Root {
		get {
			if (Application.isMobilePlatform) {
				return Application.persistentDataPath + "/config";
			}
			else {
				return Application.streamingAssetsPath + "/config";
			}
		}
	}
	public static void Write(string name, string config){
		string path = Root + "/" + name + ".txt";
		if (!Directory.Exists(Root)) Directory.CreateDirectory(Root); 
		File.WriteAllText(path, config);
	}
	public static void Write(string name, object obj){

		string path = Root + "/" + name + ".txt";
		string config = JsonUtility.ToJson(obj);
		if (!Directory.Exists(Root)) Directory.CreateDirectory(Root); 
		File.WriteAllText(path, config);
	}
	public static string Read(string name){
		string path = Root + "/" + name + ".txt"; 
		return File.ReadAllText(path);
	}
	public static T Read <T> (string name) {
		string path = Root + "/" + name + ".txt";
		string config = File.ReadAllText(path);
		return JsonUtility.FromJson < T > (config);
	}
	public static string ReadOrCreat(string name, string config) {
		string path = Root + "/" + name + ".txt";
		if (File.Exists(path)) {
			return File.ReadAllText(path);
		} else {
			if (!Directory.Exists(Root)) Directory.CreateDirectory(Root); 
			File.WriteAllText(path, config);
			return config;
		}
	}
	public static T ReadOrCreat<T>(string name, T obj) {
		string path = Root + "/" + name + ".txt";
		if (File.Exists(path)) {
			string config = File.ReadAllText(path);
			return JsonUtility.FromJson<T>(config);
		} else {
			string config = JsonUtility.ToJson(obj);
			if (!Directory.Exists(Root)) Directory.CreateDirectory(Root); 
			File.WriteAllText(path, config);
			return obj;
		}
	}
	public static bool Has(string name) {
		string path = Root + "/" + name + ".txt";
		return File.Exists(path);
	}
	public static string[] GetConfigs() {
		if (Directory.Exists(Root)) {
			string[] files = Directory.GetFiles(Root, "*.txt");
			return files.Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();
		}
			return new string[0];
	}
}