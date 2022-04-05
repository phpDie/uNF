using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


[System.Serializable]
public class UWinCallbackActionEvent : UnityEvent<UWinBaseClass, string>
{

}
 

public  class UWinBaseClass : MonoBehaviour
{
 

    [Header("Кнопки")]
    private List<GameObject> currentButtons = new List<GameObject>();

    [FormerlySerializedAs("Bottom")] [Header("Внутренние блоки")]
    public GameObject bottom;


    [FormerlySerializedAs("MyCallbackActionEvent")] [HideInInspector]
    public UWinCallbackActionEvent myCallbackActionEvent;
    Animator myAnimator;
    AudioSource myAudioSource;
    

    public BtnConstructorClass MyBtnConstructor = new BtnConstructorClass();
  
    [FormerlySerializedAs("InstanceBtn")] public BinUKey instanceBtn;

    
    public int Add(int num1, int num2)
    {
        return num1 + num2;
    }

    //The second Add method has a sugnature of
    //"Add(string, string)". Again, this must be unique.
    public string Add(string str1)
    {
        
        return str1 ;
        
    }


    public void Kill()
    {
        Add(1,2);
        //Do something fun
    }


    private void OnDisable()
    {

         
    }
    private void OnEnable()
    {
        if (myAnimator)
        {
            print("MyAnimator");
            
            myAnimator.Play(0);
        }
        if (myAudioSource)
        {
            print("MyAudioSource");
            myAudioSource.Play();
        }

      
        //Init();
    }


    private void ClearButtons()
    {
        for (int i = 0; i < currentButtons.Count; i++)
        {
            Destroy(currentButtons[i]);
        }
        currentButtons.Clear();
    }


    private void CallActionFromBtn(string ind)
    {
        //print("MyCallbackActionEvent " + Ind);
        myCallbackActionEvent.Invoke(this, ind);
    }


    private void GenerateButtons()
    { 
        for (int i = 0; i < MyBtnConstructor.ElementsList.Count; i++)
        {

            GameObject parent = bottom;


            BtnElementDataStruct data = MyBtnConstructor.ElementsList[i].Data;

           GameObject clone = Instantiate(instanceBtn.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
            BinUKey btn = clone.GetComponent<BinUKey>();
            clone.transform.SetParent(parent.transform);
            btn.SetBtnFromDataStruct(data);

            currentButtons.Add(clone);

            btn.myCallbackActionEvent.AddListener(CallActionFromBtn); 
        } 
    }
    
    void clearTemplateChildrenBtns(Transform inner)
    {
        //print("clearTemplateChildrenBtns");
        for (var i = 0; i < inner.childCount; i++) {
            if (inner.GetChild(i).gameObject.GetComponent<BinUKey>())
            {
                Destroy(inner.GetChild(i).gameObject);
            }
        }
    }


    public virtual void Start()
    {
        myAudioSource = gameObject.AddComponent<AudioSource>();

        if (gameObject.GetComponent<Animator>())
        {
            myAnimator = gameObject.GetComponent<Animator>();
            myAnimator.StopPlayback();
        }


        bottom = this.gameObject.transform.Find("Bottom").gameObject;
    }

    public virtual void Init()
    {
       
      //  print("Init base win class"); 

        clearTemplateChildrenBtns(bottom.transform);
        ClearButtons();
         
        GenerateButtons();

        
    }


 

}
