using System.Collections;
using System.Collections.Generic;
using Garage;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Audio;


[CreateAssetMenu(fileName = "New Main Project Setting")]
public class Project_DatabaseClass : ScriptableObject
{

    [Header("Ссылка на систему бд спавна тачек")]
    public CarSpawnerSystem_DatabaseClass carSpawnerSystem;

    [Header("Ссылка на тьюн базу")]
    public Tune_DatabaseClass tuneDatabaseClass;


    [Header("Ссылка на карту клавиш")]
    public KeyCodeIcons_DatabaseClass keyCodeIconsDatabaseClass;


    [Header("Ссылка базу тюнинга")]
    public IndEngineTune_DatabaseClass indEngineTune;


    [Header("Иконки брендов тюнинга")]
    public List<Sprite> brends = new List<Sprite>();
     

    [Header("Ссылка на датастор")]
    public DataStore dataStore;

    [Header("Ссылка на микшер")]
    public AudioMixer audioMixer;
}
