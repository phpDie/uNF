using PowerParametrs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BalanceStruct{
    [Header("Индификатор")] public string ind;
    [Header("Название в ui")]  public string name;
    //[Header("Длинна в каждую сторону")]public int len;
    [Header("Название слева")]public string left;
    [Header("Название справа")]public string right;
}


[CreateAssetMenu(fileName = "New IndEngineTun_DatabaseClass")]
public class IndEngineTune_DatabaseClass : ScriptableObject
{

    [Header("База тюнинга движка")]
    public List<ItemTunePowerStruct> list = new List<ItemTunePowerStruct>();

    [Header("База балансов")]
    public List<BalanceStruct> balanceList = new List<BalanceStruct>();


    [Header("Базавые параметры двигла")]
    public List<BaseEngineAttributeStruct> engineAttribute = new List<BaseEngineAttributeStruct>();



    public List<Color32> colorFromLevel= new List<Color32>(5);
     


}
