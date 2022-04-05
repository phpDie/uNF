using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct VisualDetal
{

    [Header("Индификатор")]
    public string ind;


    [Header("Название")]
    public string name;

    [Header("Коэффициент стоимости 0-0.99")]
    [Range(0.001f, 1f)]
    public float priceCoff;


}

[CreateAssetMenu(fileName = "New keyCode Tune_DatabaseClass")]
public class Tune_DatabaseClass : ScriptableObject
{


    [Header("Визуальные детали")]
    public List<VisualDetal> detalList = new List<VisualDetal>();

    [Header("Цвета тачки")]
    public List<Color32> colorList = new List<Color32>();

    public VisualDetal? GetDataFromKey(string ind)
    {
        for (var i = 0; i < detalList.Count; i++)
        {
            if (detalList[i].ind == ind)
            {
                return detalList[i];
            }
        }
        return null;
    }

}
 