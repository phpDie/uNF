using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class BinUKey : MonoBehaviour
{ 
    public BinUKeyCallbackActionEvent myCallbackActionEvent;

    [FormerlySerializedAs("MyKeyCodeDB")] public KeyCodeIcons_DatabaseClass myKeyCodeDB;
    [FormerlySerializedAs("BtnData")] public BtnElementDataStruct btnData;
     
    private Button button;
 

    public void Render()
    {


        Transform bind = gameObject.transform.Find("bind");
        gameObject.transform.Find("Title").GetComponent<Text>().text = btnData.title;
         
        Sprite myIcon = myKeyCodeDB.GetIconFromKeyCode(btnData.keybordKey);


        bind.Find("_Key").gameObject.SetActive(myIcon == null);
        bind.Find("_KeyImg").gameObject.SetActive(myIcon != null);

        if (myIcon)
        {
            bind.Find("_KeyImg").GetComponent<Image>().sprite = myIcon;
        }
        else
        {
            bind.Find("_Key").GetComponent<Text>().text = btnData.keybordKey.ToString();
            
        }
       
    }

    public void SetBtnFromDataStruct(BtnElementDataStruct data)
    {
        btnData = data;
        Render();
    }

     
     
    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(ActionCall);

        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.localScale = new Vector3(1, 1, 1)*0.5f;
    }

    void ActionCall()
    {
        //print("Click btn" + BtnData.title);
        myCallbackActionEvent.Invoke(btnData.ind);

        gameObject.transform.Find("bind").transform.LeanScale(new Vector3(1, 1, 1) * 1.15f, 0.2f).setEasePunch();
    }


    void Update()
    {
        if (Input.GetKeyDown(btnData.keybordKey)  )
        {
            //print("Btn click");
            ActionCall();
        }

    }
}
