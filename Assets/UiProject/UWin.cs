using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UWin : UWinBaseClass
{ 
    [FormerlySerializedAs("BtnList")] public List<BtnElementDataStruct> btnList = new List<BtnElementDataStruct>();

    private void OnEnable()
    {
        
    }

    public override void Init()
    {

        base.Init();
    }


    public override void Start()
    {

        base.Start();

        MyBtnConstructor.ElementsList.Clear();

        btnList.ForEach((item) =>
        {
            MyBtnConstructor.New().Data = item;
        });

        Init();
    }

 

    
}
