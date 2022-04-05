using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CarTunr;
using Cinemachine;
using Garage.Plugins;

public class RaceManager : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;
    public DataStore dataStore;
    public Transform pointSpawner;

    public Project_DatabaseClass project_DatabaseClass;


    [HideInInspector] public CarConfigBase car;
    [HideInInspector] public CarSpawnerSystem_DatabaseClass carSpawnerSystem_DatabaseClass;


    bool isInitCar = false;
    public CarConfigBase GetCurrentCar()
    {
        if (isInitCar)
        {
            return car;
        }
        isInitCar = true;


        string _carInd = dataStore.Data.GetCurrentCar().Get().carInd;
        carSpawnerSystem_DatabaseClass = project_DatabaseClass.carSpawnerSystem;

        car = carSpawnerSystem_DatabaseClass.SpawnCarFromInd(_carInd, false);

        car.gameObject.transform.position = pointSpawner.position;
        car.gameObject.transform.rotation = pointSpawner.rotation;
        car.transform.SetParent(gameObject.transform.parent);
        
         

        cinemachineVirtualCamera.Follow = car.gameObject.transform;
        cinemachineVirtualCamera.LookAt = car.gameObject.transform;
      

        //dataStore.Data.myCar = new DataStoreCollections.myCarData(car, "myCar");
        dataStore.Load();

      
        car.SetFromJson(dataStore.Data.GetCurrentCar().Get());

        EngineMath engineMath = car.gameObject.GetComponent<EngineMath>();
        MathToEngineResponseStruct _resEngine = engineMath.MathToEngineLevelList(car.engineTuneLevel);

        foreach (var item in _resEngine.powerUpgrade) { 
          //  print(item.Key.ToString() + " = " + item.Value.ToString());
        }
        car.GetComponent<EngineConverter>().ReadEngineLevels();

        car.GetComponent<RCC_CarControllerV3>().enabled = true;
        car.gameObject.SetActive(true);

        if (!car.visualTuneHelperSelection)
        {
            print("Error car.visualTuneHelperSelection");
            car.visualTuneHelperSelection = car.gameObject.GetComponent<VisualTuneHelperSelection>();
            if (!car.visualTuneHelperSelection)
            {
                print("BUG ULTRA!");
                print(car);
            }
        }
        car.visualTuneHelperSelection.initConfigs();
        car.visualTuneHelperSelection.ResetTune_ToData();

        car.GetComponent<RCC_CarControllerV3>().enabled = true;
        car.GetComponent<RCC_CarControllerV3>().canControl = true;

        return car;
    }


    public void RaceExit()
    {
        SceneManager.LoadScene("GarageSceneMain");

    }


    void OnEnable()
    {
       // print("OnEnable RACE MANAGER");
        GetCurrentCar();

    }

}
