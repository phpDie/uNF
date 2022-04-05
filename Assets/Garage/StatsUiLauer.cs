using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI; 

public class StatsUiLauer : MonoBehaviour
{

    public Text MoneyText;
    
    public DataStore dataStore;
     
    void Start()
    {
        dataStore.Data.Money.Change.AddListener(ChangeSelectedStats);
        dataStore.Data.Expa.Change.AddListener(ChangeSelectedStats);
      //  dataStore.Data.Level.Set(135);
     //   dataStore.Data.Money.Set(5);


        dataStore.Data.Change.AddListener(UpdEvAny);

        ChangeSelectedStats();

    }

    void UpdEvAny(string x)
    {

       // print("any key upd " + x);
            
    }


    void ChangeSelectedStats()
    {
        MoneyText.text = dataStore.Data.Money.Get() + "$";
        gameObject.GetComponent<FastSound>().Play("Money");
    }

 
}
