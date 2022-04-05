using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Garage.Plugins;
using PowerParametrs;
using Newtonsoft.Json;
using CarTunr;

namespace CarTunr
{
    /*
    public class carSaveItem
    {
        public CarConfigBase  carConfigBase;

        public string carInd = "VolvaTestet";

        public Dictionary<string, Dictionary<int, bool>> visualPartByed = new Dictionary<string, Dictionary<int, bool>>();
        

        public bool IseetVisualPart(string cat, int id)
        {
            if (!visualPartByed.ContainsKey(cat)) return false;
            if (!visualPartByed[cat].ContainsKey(id)) return false; 
            return true;
        }

        public void AddVisualPart(string cat, int id)
        {
            if (!visualPartByed.ContainsKey(cat))
            {
                visualPartByed[cat] = new Dictionary<int, bool>(); 
            }
            
            visualPartByed[cat][id] = true;
           // Debug.Log(visualPartByed.ToString());
        }

        public string GetJson()
        {
            return "xdata";
        }
    }
    */

    public enum Marka{
        Nissan,
        Ford,
        Mustang,
        Volvo,
    }

    [System.Serializable]
    public struct carGarageItemStruct
    {
        public int price;
        public string name;
        public Marka marka;
    }

    public struct carTuneEngineStruct
    {
        public int turbo;
        public int air;
    }

    public class CarConfigBase : MonoBehaviour
    {

      

        [HideInInspector]public string carInd;

        public IndEngineTune_DatabaseClass indEngineTune_DatabaseClass;

        public VisualTuneHelperSelection visualTuneHelperSelection;

        [SerializeField]
        public carGarageItemStruct carInfo;
         


        [Header("Базавые параметры двигла")]
        [SerializeField]
        public List<EngineBaseAttibute> engineAttribute = new List<EngineBaseAttibute>();

        

        [SerializeField]
        public Dictionary<string,int> engineTuneLevel= new Dictionary<string, int>();

        public Dictionary<string, int> DataBalance = new Dictionary<string, int>();


        // Start is called before the first frame update
        public void FixEmptyData()
        {
            if (DataBalance == null)
            {
            //    Debug.LogWarning("Пустой DataBalance!");
                DataBalance = new Dictionary<string, int>();
            }

            foreach (BalanceStruct item in indEngineTune_DatabaseClass.balanceList)
            {
                if (DataBalance.ContainsKey(item.ind)) continue;
                DataBalance[item.ind] = 0;
            }

            

            if (visualPartByed == null)
            {
                visualPartByed = new Dictionary<string, Dictionary<int, bool>>();
            }

            if (engineTuneLevel==null)
            {
                engineTuneLevel = new Dictionary<string, int>();
            }
            for (int i = 0; i < indEngineTune_DatabaseClass.list.Count; i++)
            {
                ItemTunePowerStruct e = indEngineTune_DatabaseClass.list[i];
                if (engineTuneLevel.ContainsKey(e.ind))
                {
                    continue;
                }
                engineTuneLevel[e.ind] = 0;
            }
        }
        void Start()
        {
            FixEmptyData();
        }

        // Update is called once per frame
        void Update()
        {

        }


        public Dictionary<string, Dictionary<int, bool>> visualPartByed = new Dictionary<string, Dictionary<int, bool>>();


        public bool IseetVisualPart(string cat, int id)
        {
            if (!visualPartByed.ContainsKey(cat)) return false;
            if (!visualPartByed[cat].ContainsKey(id)) return false;
            return true;
        }

        public void AddVisualPart(string cat, int id)
        {
            if (!visualPartByed.ContainsKey(cat))
            {
                visualPartByed[cat] = new Dictionary<int, bool>();
            }

            visualPartByed[cat][id] = true;
        }


        public void SetFromJson(DataSaveCar val)
        {
             
            DataSaveCar dataSaveCar = val;
            engineTuneLevel = dataSaveCar.engineTuneLevel;
            visualPartByed = dataSaveCar.visualPartByed;
            carInd = dataSaveCar.carInd;
          
             
            // print(gameObject.name);

            
            if (dataSaveCar.DataBalance  != null)
            { 
                DataBalance = dataSaveCar.DataBalance;
            }

            if (dataSaveCar.DataVisual != null && visualTuneHelperSelection)
            {
               // print("DataVisual load");
             //   print(dataSaveCar.DataVisual);
                visualTuneHelperSelection.DataVisual = dataSaveCar.DataVisual; 
                visualTuneHelperSelection.colorId = dataSaveCar.colorId; 
            }
             

        //    if (dataSaveCar.DataVisual == null) print("DataVisual null");
            if (!visualTuneHelperSelection) print("visualTuneHelperSelection null");

            if (visualTuneHelperSelection)
            {
                visualTuneHelperSelection.ResetTune_ToData();
            }
        }

        public DataSaveCar GetJson()
        {
            DataSaveCar dataSaveCar = new DataSaveCar(); 
            dataSaveCar.engineTuneLevel = engineTuneLevel;
            dataSaveCar.visualPartByed = visualPartByed;
            dataSaveCar.DataBalance = DataBalance;
            dataSaveCar.carInd  = carInd;
           /* 
            print("GetJson DataBalance count " + DataBalance.Count.ToString());
            foreach(var item in DataBalance)
            {
                print(item.Key + " = " + item.Value.ToString());
            }
            */
            if (visualTuneHelperSelection)
            {
                dataSaveCar.colorId = visualTuneHelperSelection.colorId;
                dataSaveCar.DataVisual = visualTuneHelperSelection.DataVisual; 
            }

          //  string _test = JsonConvert.SerializeObject(dataSaveCar, Formatting.None);
            return dataSaveCar;
        }
    }
}

public struct DataSaveCar
{
    public string carInd;
   // public carGarageItemStruct carInfo;
    //public List<EngineBaseAttibute> engineAttribute;
    public Dictionary<string, int> engineTuneLevel;
    public Dictionary<string, Dictionary<int, bool>> visualPartByed;
    public Dictionary<string, int> DataVisual;
    public Dictionary<string, int> DataBalance;
    public int  colorId; 
}