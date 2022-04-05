using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStoreCollections;
using UnityEngine.Events;
using CarTunr;
using Newtonsoft.Json;

namespace DataStoreCollections
{

    /*public class SaveData
       {
           public number<int> Money = new number<int>(130);
           public number<float> Expa = new number<float>(1.5f); 
           //public DataStoreContruction.intData Expa = new DataStoreContruction.intData(16000);
           public intData Level = new intData(16000);
       }
       */



   


    public class myCarData: ASaveAbstractClass<DataSaveCar>
    { 

        public void Save()
        {
            base.Set(Val);
        }

        public void Set(DataSaveCar val)
        {
            base.Set(val);
        }

        public void FromJson(string val)
        {
            Val = JsonConvert.DeserializeObject<DataSaveCar>(val);
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(Val, Formatting.None);
         //   return Val.GetJson();
        }

        public myCarData(DataSaveCar val  , string MyChangeEventInd = "")
            : base(val, MyChangeEventInd)
        {

        }
    }


    public class SaveData
    { 
        public DataStoreCollectionsEvent Change = new DataStoreCollectionsEvent();


        public intData Money = new intData(16000, "Money"); 
        public intData currentCarId = new intData(1, "currentCarId"); 

        public intData Expa = new intData(1, "Expa"); 
        //public number<float> Expa = new number<float>(1.5f, "Expa");  
        public intData Level = new intData(1, "Level");
       // public myCarData myCar = new myCarData(new CarConfigBase(), "myCar");
        public List<myCarData> cars = new List<myCarData>();

        
        public void MakeDefCars()
        {
            cars = new List<myCarData>();
            DataSaveCar testCar = new DataSaveCar();
            testCar.carInd = "Volva_Car_PF";
            cars.Add(new myCarData(testCar, "myCar"));

            testCar = new DataSaveCar();
            testCar.carInd = "Skyline_Car_PF";
            cars.Add(new myCarData(testCar, "myCar"));
             
            Debug.Log("MakeDefCars!! Создан дефалтный набор тачек! ");
        }

        public myCarData GetCurrentCar()
        {
           
            if(cars.Count==0)MakeDefCars(); 

            return cars[currentCarId.Get()];
        }

        public void Load(Dictionary<string, string> data)
        {
            Money.FromJson(data["Money"]);
            Expa.FromJson(data["Expa"]);
            Level.FromJson(data["Level"]);
           // myCar.FromJson(data["myCar"]);
            cars = new List<myCarData>();

            List<string> carOut = JsonConvert.DeserializeObject<List<string>>(data["cars"]);
            foreach (string item in carOut)
            {
                myCarData _car = new myCarData(new DataSaveCar(), "myCar");
                _car.FromJson(item);
                cars.Add(_car);
            }

        }

        public Dictionary<string, string> ToJson()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data["Money"] = Money.GetJson();
            data["Expa"] = Expa.GetJson();
            data["Level"] = Level.GetJson();
          //  data["myCar"] = myCar.GetJson();



            List<string> carOut = new List<string>(); 
            foreach (myCarData item in cars)
            {
                carOut.Add(item.GetJson());
            }

            data["cars"] = JsonConvert.SerializeObject(carOut, Formatting.None);

            return data;
        }

        public SaveData()
        { 
            Money.callUpdate = Change;
            Expa.callUpdate = Change;
            Level.callUpdate = Change;
            currentCarId.callUpdate = Change;
            
         
            

            foreach (myCarData item in cars)
            {
                item.callUpdate = Change;
            }
            //  myCar.callUpdate = Change; 
        }
    }
}
 