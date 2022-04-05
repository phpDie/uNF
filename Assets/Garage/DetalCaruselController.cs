using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DetalCaruselController : MonoBehaviour
{
    [FormerlySerializedAs("PrefabItem")] public GameObject prefabItem;




    [HideInInspector]
    public SimpleScrollSnap simpleScrollSnap;

   
    
    public int Max
    {
        get
        {
            return simpleScrollSnap.NumberOfPanels;
        }
    }

    public int Index
    {
        get
        {
            return simpleScrollSnap.GetNearestPanel();
        }
    }
     

    public int GoTo(int curent)
    {

        //print("GoTo " + Curent.ToString());
        simpleScrollSnap.GoToPanel(curent);
        return Index;
    }

    public int Next(int plus)
    {
        int curent = Index + plus;
        if (curent < 0) curent = Max;
        if (curent < Max) curent = 0;

        simpleScrollSnap.GoToPanel(curent);
        return curent;
    }
     
    void ExampleWork()
    {

        
        simpleScrollSnap.GoToNextPanel();
        simpleScrollSnap.GetNearestPanel();
        simpleScrollSnap.RemoveFromBack();
    }

    List<GameObject> listElements = new List<GameObject>();

  public  void AddItem(GameObject panel, int index)
    {
      //  print(panel);
       // print(index);
        if (!simpleScrollSnap)
        {
            Init();
            print("ERROR simpleScrollSnap!");
            //return;
        }
        simpleScrollSnap.Add(panel, index);
        listElements.Add(panel);
    }

  public  void ClearAllItems()
    {
       // print("ClearAllItems: " + Max.ToString());
        for (var i = 0; i < Max; i++)
        {
            simpleScrollSnap.Remove(i);
        }

        if (Max > 0)
        {
            simpleScrollSnap.RemoveFromBack();
        }
        if (Max > 0)
        {
            for (var i = 0; i < Max; i++)
            {
                simpleScrollSnap.Remove(i);
            }
        }
      //  print("ClearAllItemsPost: " + Max.ToString());


        for (var i = 0; i < listElements.Count; i++)
        {
            Destroy(listElements[i]); 
        }
            
    }

    void AutoAdding(int count)
    { 

        for (var i = 0; i < count; i++)
        {
            GameObject e = Instantiate(prefabItem);
            simpleScrollSnap.Add(e, i);

        }
    }

    bool isInit = false;

    public void Init()
    {
        if (isInit) return;
        isInit = true;
            simpleScrollSnap = gameObject.GetComponent<SimpleScrollSnap>();

        if (!simpleScrollSnap)
        {
            print("ERROR simpleScrollSnap INIT!");
        }

    }

      void Start()
    {
        Init();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
