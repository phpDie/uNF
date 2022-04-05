using PowerParametrs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTunr;

public struct WheelBridgeStruct
{
    public float driftval;
}

public struct EngineBridgeStruct
{ 
    public float highspeedsteerAngle;// = 15f;     // Maximum Steer Angle At Highest Speed.
    public float highspeedsteerAngleAtspeed;// X 
    public float engineTorque;// = 2000f;		// Default Engine Torque.
    //public WheelType _wheelTypeChoise;
    public float maxspeed;// = 220f;
    public float maxAngularVelocity;// = 220f;
    public float weight;// = 220f;
    public float antiRollFrontHorizontal;// = 220f;
}


public class EngineConverter : MonoBehaviour
{

    /*
     highspeedsteerAngleAtspeed = 50. Это скорость от которой работает лимит поворота
     highspeedsteerAngle = 15. Это для вольвы, угол поворота
        
     * */
    // Start is called before the first frame update

    CarConfigBase carConfigBase;
    EngineMath engineMath;
    public Dictionary<BaseAttribute, float> engineAttribute; 

    public void ReadEngineLevels()
    {
        init();

        //print("ReadEngineLevels");
        MathToEngineResponseStruct _resEngine = engineMath.MathToEngineLevelList(carConfigBase.engineTuneLevel);
        engineAttribute = _resEngine.powerUpgrade; 
    }

    bool isInit = false;
    void init()
    {
        if (isInit) return;
        isInit = true;

            carConfigBase = gameObject.GetComponent<CarConfigBase>();
        engineMath = gameObject.GetComponent<EngineMath>();

        ReadEngineLevels();
    }


    void Start()
    {
        init(); 
    }
    public float GetBalance(string key)
    {
        if (true)
        {
          //  return 1f;
        }

        if (!carConfigBase.DataBalance.ContainsKey(key))
        {
           // print("[ERROR BRIGE] Не найден ключ DataBalance " + key.ToString());
            return 1;
        }
        float _val = carConfigBase.DataBalance[key] + 0.001f;
        _val = (_val*2f ) / 10f;
        _val = Mathf.Clamp(_val, -1f, 1f);

        return _val;
    }

    public float GetAttr(BaseAttribute key)
    {
        if (!engineAttribute.ContainsKey(key))
        {
            print("[ERROR BRIGE] Не найден ключ движкка " + key.ToString());
            return 1;
        }

        return engineAttribute[key]; 
    }
    /*
    public float GetAttr(BaseAttribute key)
    {
        if(!carConfigBase.engineAttribute.Exists(item => item.ind == key))
        {
            print("[ERROR BRIGE] Не найден ключ движкка " + key.ToString());
            return 1;
        }
        EngineBaseAttibute res =  carConfigBase.engineAttribute.Find(item => item.ind == key);

        return res.val;
    }
    */

    public WheelBridgeStruct GenerateWheelBrige()
    {
        init();

        WheelBridgeStruct Result = new WheelBridgeStruct();
        Result.driftval = GetBalance("drift");

        return Result;
    }
    public EngineBridgeStruct GenerateBrige()
    {
       // print("GenerateBrige");
        init();

        EngineBridgeStruct Result = new EngineBridgeStruct();

        //точное решения 
        Result.highspeedsteerAngleAtspeed = 36;


        //матеша 
        Result.maxAngularVelocity = 3.5f;
        Result.engineTorque =   Mathf.Lerp(500, 3600, (GetAttr(BaseAttribute.horsepower)-100) / 900);
        Result.maxspeed = GetAttr(BaseAttribute.maxSpeed);
        Result.weight  = GetAttr(BaseAttribute.weight);

        float driftBal = GetBalance("drift");






        Result.highspeedsteerAngleAtspeed = Mathf.Lerp(10f, 15f, Mathf.Abs(driftBal));
        Result.highspeedsteerAngle = Mathf.Lerp(14, 35, Mathf.Abs(driftBal));
        if (driftBal > 0f)
        {
            Result.antiRollFrontHorizontal = Mathf.Lerp(1000, 30000, driftBal);  
         //   Result.highspeedsteerAngleAtspeed = Mathf.Lerp(10f, 15f, Mathf.Abs(driftBal));
             

            Result.maxAngularVelocity = 1.5f + driftBal  * 3.2f; 
        }
        else
        {
            Result.highspeedsteerAngle = Mathf.Lerp(14, 24, Mathf.Abs(driftBal));
            Result.antiRollFrontHorizontal = Mathf.Lerp(3000, 1000, Mathf.Abs(driftBal)); 
           // Result.highspeedsteerAngleAtspeed = Mathf.Lerp(35, 70, Mathf.Abs(driftBal));

            Result.maxAngularVelocity = 1.2f + Mathf.Abs(driftBal) * 1.9f;
          //  Result.highspeedsteerAngle = Mathf.Lerp(13, 35, Mathf.Abs(driftBal));
        }
        //Result.highspeedsteerAngleAtspeed = 10f; 
     //   Result.highspeedsteerAngle = 15f; 
//        Result.antiRollFrontHorizontal = 10f; 

      //    print("DRIFT_BALANE " + driftBal.ToString());



        return Result;
    }
 

}
