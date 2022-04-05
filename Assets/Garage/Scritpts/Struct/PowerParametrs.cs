using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowerParametrs
{
    [System.Serializable]
    public enum BaseAttribute
    {
        horsepower,
        maxSpeed,
        peakRpm,
        weight,
        helath,
        potenctialGrip,
        potenctialDrift, 
    }


    [System.Serializable]
    public struct EngineBaseAttibute
    {
        [HideInInspector]
        public string _editorName;
        public BaseAttribute ind;
        public float val;
    }

    


    [System.Serializable]
    public struct BaseEngineAttributeStruct
    {
        [SerializeField]
        public string name;
        public BaseAttribute ind; 
        public string ed;   
        public int ceilTo;   
        public float defVal;   
        public bool isSpinUp;    
    }


    [System.Serializable]
    public struct ItemTunePowerStruct
    {
        public string ind;
        public string name;
        public bool arrowUp;
        public Color32 color;

        [Range(0.01f, 1f)]
        public float priceForLevel;

        [Range(1, 5)]
        public int maxLevel;

        public List<BoosterTunePowerStruct> boostForLevel;
    }

    [System.Serializable]
    public struct BoosterTunePowerStruct
    {
        public BaseAttribute ind;

        [Header("На сколько бустится параметр")]
        [Range(-0.3f, 0.3f)]
        public float boostCofficient; 
    }

    /*
    public class InitTunePowerStruct
    {
       public List<ItemTunePowerStruct> InitTuneKeys2(List<ItemTunePowerStruct> data)
        {
            foreach (var theEnum in Enum.GetValues(typeof(ItemTunePowerStruct)))
            {
                bool isset = false;
                for (int i=0;i< data.Count; i++)
                {
                    
                    if(Enum.Equals(data[i].ind, theEnum))
                    {
                        isset = true;
                        break;
                    }
                }

                if (isset) continue;

                
            }
             

            return data;
        }
        
    }
    */
     

    /*
    public class CarEngineStruct : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
    */
}