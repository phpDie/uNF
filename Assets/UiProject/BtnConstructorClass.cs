using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class BinUKeyCallbackActionEvent : UnityEvent<string>
{

}

public enum WinTo
{
    Bottom,
    Top,
    Center,
}

[Serializable]
public struct BtnElementDataStruct
{
    public string ind;
    public string title;
    [FormerlySerializedAs("KeybordKey")] public KeyCode keybordKey;
    //public KeyCode GamepadKey; 
    [FormerlySerializedAs("Parent")] public WinTo parent;
}

public class BtnElement
{
     
    public BtnElementDataStruct Data = new BtnElementDataStruct(); 
    
    public BtnElement SetParent(WinTo val)
    {
        Data.parent = val;
        return this;
    }

    public BtnElement SetKey(KeyCode val)
    {
        Data.keybordKey = val;
        return this;
    }

    public BtnElement SetGamepadKey(KeyCode val)
    {
        Data.keybordKey = val;
        return this;
    }

    public BtnElement SetTitle(string val)
    {
        Data.title = val;
        return this;
    }
    public BtnElement SetInd(string val)
    {
        Data.ind = val;
        return this;
    }

}


public class BtnConstructorClass 
{ 

   public List<BtnElement> ElementsList = new List<BtnElement>();


    public BtnElement New()
    {
        BtnElement myElement = new BtnElement();
        ElementsList.Add(myElement); 
        return myElement;
    }


    public void print()
    {
        MonoBehaviour.print("call print btns");
        for (int i = 0; i < ElementsList.Count; i++)
        {
            //ElementsList[i].Data.title
            //ElementsList[i]

            MonoBehaviour.print("\n");
            MonoBehaviour.print(ElementsList[i].Data.ind);
            MonoBehaviour.print(ElementsList[i].Data.title);
            MonoBehaviour.print(ElementsList[i].Data.keybordKey);
        }
    }
}
