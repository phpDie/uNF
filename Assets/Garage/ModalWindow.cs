using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Garage.Scritpts;


public class ModalWindow : URouter_BaseClass
{

    public Text _Title;
    public Text _Descr;

    [HideInInspector]
    public bool isOpen = false;

    string IndOpen;

    public UnityEvent<bool,string> unityEvent = new UnityEvent<bool, string>();

    private bool ReusltResponce; 

    public void ModalBuild(string Ind, string Title, string Descr = "", bool isDoubleVariant = false)
    {
        base.Init();
        fastSound.Play("open");
        IndOpen = Ind;
        isOpen = true;
        _Title.text = Title;
        _Descr.text = Descr;
        _Descr.gameObject.SetActive((Descr.Length > 1));
        

        Open("");
    }
     
    public void _callunityEvent(bool _res, string ind)
    {

    }

    public override void Open(string data)
    { 
        base.Open(data);
    }


    public override void Close()
    {
        base.Close();
    }

    void Start()
    {
        Init();

        unityEvent.AddListener(_callunityEvent);
    }

    public override bool RouterAction(UWinBaseClass window, string action)
    {

        //if (!base.RouterAction(window, action)) return false;
        if (action != "Yes" && action != "No")
        {
            return false;
        }

        fastSound.Play("click");
       // print("UNBLOCK");
        ReusltResponce = (action == "Yes");
       

        isOpen = false;
        unityEvent.Invoke(ReusltResponce, IndOpen);
        Close();
        return true;
    }

}
