using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WindowsController : MonoBehaviour
{

    [FormerlySerializedAs("Win_Alert")] public UWinBaseClass winAlert;
    [FormerlySerializedAs("Win_VisualList")] public UWinBaseClass winVisualList;


    void RouterAction(UWinBaseClass window, string action)
    {
        if (window == winAlert)
        {
            //if(Action=="Yes")Win_Alert.gameObject.SetActive(false);
            winAlert.gameObject.SetActive(false);
            return;
        }


        if (winAlert.gameObject.activeSelf) return;



        if (window == winVisualList)
        {
            if(action== "Exit")
            {
                winAlert.gameObject.SetActive(true);
            }
            
            return;
        }


       

        print("Не понятный тип окна");
    }

    void Start()
    {
        winVisualList.myCallbackActionEvent.AddListener(RouterAction);
        winAlert.myCallbackActionEvent.AddListener(RouterAction);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
