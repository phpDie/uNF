using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using DataStoreCollections;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "New DataStore", menuName = "DataStore/Create DataStore")]
public class DataStore : ScriptableObject
{

    public SaveData Data = new SaveData();


    [Header("Key")]
    public string UnicalKey = "Player1";


    [Header("Debug")]
    public bool DEBUG_SHOW_CHANGE = false;


    public void Load()
    {
        string _datj = PlayerPrefs.GetString(UnicalKey, "null");
        if (_datj == "null")
        {
            Debug.Log("no save");
            return;
        }


        Dictionary<string, string> listData = JsonConvert.DeserializeObject<Dictionary<string, string>>(_datj);

        Data.Load(listData); 
    }

    public void Save()
    {
        Dictionary<string, string> listData = Data.ToJson();

        string _dat = JsonConvert.SerializeObject(listData);
        PlayerPrefs.SetString(UnicalKey , _dat);
        //_dat = _dat.Replace("{[", "\n {[");
       // Debug.Log(_dat);
        /*
        foreach (var item in listData)
        {
            Debug.Log(item.Value);
            PlayerPrefs.SetString(UnicalKey + "_" + item.Key, item.Value);
        } 
        */
    }

    void CallChange(string IndKey)
    {
        if (DEBUG_SHOW_CHANGE)
        {
            Debug.Log("[DataStore] Change data: " + IndKey);
        }
    }



    public DataStore()
    {

        Data.Change.AddListener(CallChange);

    }

}