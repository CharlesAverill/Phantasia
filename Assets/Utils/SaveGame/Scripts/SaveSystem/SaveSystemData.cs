using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SaveData {

	public string Key {get;set;}
	public string Value {get;set;}

	public SaveData(){}

	public SaveData(string key, string value)
	{
		this.Key = key;
		this.Value = value;
	}
}

[System.Serializable]
public class DataState {

	public List<SaveData> items = new List<SaveData>();

	public DataState(){}

	public void AddItem(SaveData item)
	{
		items.Add(item);
	}
}

public class SerializatorBinary {

	public static void SaveBinary(DataState state, string dataPath)
	{
		BinaryFormatter binary = new BinaryFormatter();
		FileStream stream = new FileStream(dataPath, FileMode.Create);
		binary.Serialize(stream, state);
		stream.Close();
	}

	public static DataState LoadBinary(string dataPath)
	{
		BinaryFormatter binary = new BinaryFormatter();
		FileStream stream = new FileStream(dataPath, FileMode.Open);
		DataState state = (DataState)binary.Deserialize(stream);
		stream.Close();
		return state;
	}
}

