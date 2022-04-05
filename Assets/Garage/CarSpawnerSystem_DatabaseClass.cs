using System.Collections;
using System.Collections.Generic;
using CarTunr;
using UnityEngine;

[CreateAssetMenu(fileName = "New CarSpawnerSystem_DatabaseClass")]
public class CarSpawnerSystem_DatabaseClass : ScriptableObject
{


    [Header("База тачек")]
    public List<CarConfigBase> brends = new List<CarConfigBase>();

    // Start is called before the first frame update
    void Start()
    {

    }


    public CarConfigBase GetCarInstanceFromInd(string ind)
    {
        if (!brends.Find(item => item.gameObject.name == ind))
        {
           Debug.Log("Не найдена тачка: " + ind);
            return null;
        }

        CarConfigBase car = brends.Find(item => item.gameObject.name == ind);

     //   car.GetComponent<RCC_CarControllerV3>().enabled = false;
        return car;
    }

    public int TEstt = 1;

    public CarConfigBase SpawnCarFromInd(string ind, bool isGarageMode)
    {
        CarConfigBase _car = GetCarInstanceFromInd(ind);
        if (!_car) return null;
       // _car.gameObject.SetActive(false); 

        GameObject car = Instantiate(_car.gameObject, null);
        car.gameObject.SetActive(false);

        
        //_car.GetComponent<RCC_CarControllerV3>().enabled = false;

        if (isGarageMode)
        { 
            car.GetComponent<Rigidbody>().isKinematic = true; 
        }

        car.GetComponent<CarConfigBase>().carInd = ind;

        return car.GetComponent<CarConfigBase>();
    }
}