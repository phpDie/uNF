using CarTunr;
using DataStoreCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class GarageController : MonoBehaviour
{

    public DataStore dataStore;
    public Transform pointSpawner;

    public Project_DatabaseClass project_DatabaseClass; 


    [HideInInspector] public CarConfigBase car;
    [HideInInspector] public CarSpawnerSystem_DatabaseClass carSpawnerSystem_DatabaseClass;


    bool isInitCar = false;


    public int GetCountCars()
    {
        return dataStore.Data.cars.Count;
    }

    public CarConfigBase SpawnMyCarNumber(int id, Transform spawnTransform = null)
    {
        if (dataStore.Data.cars.Count < id-1)
        {
            print("no car");
            return null;
        }
    //    print("SpawnMyCarNumber " + id.ToString());
         
        carSpawnerSystem_DatabaseClass = project_DatabaseClass.carSpawnerSystem;
        string _carInd = dataStore.Data.cars[id].Get().carInd;
        //car = carSpawnerSystem_DatabaseClass.SpawnCarFromInd("Volva_Car_PF", true);
        CarConfigBase  _car = carSpawnerSystem_DatabaseClass.SpawnCarFromInd(_carInd, true);

        if (spawnTransform)
        {
            _car.gameObject.transform.position = spawnTransform.position;
            _car.gameObject.transform.rotation = spawnTransform.rotation;
        }

        _car.transform.SetParent(gameObject.transform.parent);
         
        _car.SetFromJson(dataStore.Data.cars[id].Get());
        _car.FixEmptyData();

        _car.GetComponent<RCC_CarControllerV3>().enabled = true;
        _car.gameObject.SetActive(true);

        return _car;
    }

    public void RemoveCar(int id)
    {
        int curCarId = dataStore.Data.currentCarId.Get();
        dataStore.Data.cars.RemoveAt(id);

        if (curCarId == id)
        {
            dataStore.Data.currentCarId.Set(0);
        }
        GetCurrentCar();
    }

    public void AddCar(string ind)
    { 
        DataSaveCar testCar = new DataSaveCar();
        testCar.carInd = ind;
        myCarData _newmyCarData = new myCarData(testCar, "myCar");
        dataStore.Data.cars.Add(_newmyCarData);
        int cur = dataStore.Data.cars.FindIndex(item => item == _newmyCarData);
        
        SetCurrentCar(cur);
    }

    public void SetCurrentCar(int id)
    {
      //  print("SetCurrentCar");
        Destroy(car.gameObject);
        car = null;
        dataStore.Data.currentCarId.Set(id);
        GetCurrentCar();
    }

    public CarConfigBase GetCurrentCar()
    {
        if (car)
        {
            return car;
        }
    //    print("GetCurrentCar " + dataStore.Data.currentCarId.Get().ToString());

        car = SpawnMyCarNumber(dataStore.Data.currentCarId.Get(), pointSpawner);
        /*
        carSpawnerSystem_DatabaseClass = project_DatabaseClass.carSpawnerSystem;
        string _carInd =  dataStore.Data.GetCurrentCar().Get().carInd;
        //car = carSpawnerSystem_DatabaseClass.SpawnCarFromInd("Volva_Car_PF", true);
        car = carSpawnerSystem_DatabaseClass.SpawnCarFromInd(_carInd, true);

        car.gameObject.transform.position = pointSpawner.position;
        car.gameObject.transform.rotation = pointSpawner.rotation;
        car.transform.SetParent(gameObject.transform.parent);
        car.FixEmptyData();
        print("car set active");
        car.GetComponent<RCC_CarControllerV3>().enabled = true;
        car.gameObject.SetActive(true);
        */

        return car;
    }


    public void CurrentCarSave()
    {
        print("CurrentCarSave " + dataStore.Data.currentCarId.Get().ToString());
        int currentCarId = project_DatabaseClass.dataStore.Data.currentCarId.Get();
        project_DatabaseClass.dataStore.Data.cars[currentCarId] = new myCarData(car.GetJson(), "myCar");
        project_DatabaseClass.dataStore.Save();
    }


    public void RaceStart()
    {
        CurrentCarSave();
        //project_DatabaseClass.dataStore.Data.myCar = new myCarData(car, "myCar");
        //project_DatabaseClass.dataStore.Save();
        SceneManager.LoadScene("Race");

    }

     
    void Awake()
    {
        carSpawnerSystem_DatabaseClass = project_DatabaseClass.carSpawnerSystem;
        dataStore.Data.MakeDefCars();
        GetCurrentCar();

    }
     
}