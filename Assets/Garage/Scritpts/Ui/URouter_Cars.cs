using CarTunr;
using Garage.Plugins;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garage.Scritpts
{



    public class URouter_Cars : URouter_BaseClass
    {

        struct SliderElementData
        {
            public GameObject gameObject;
            public int index;
            internal CarConfigBase car;
            internal BilbordKeySelector bilbord;
        }

        List<SliderElementData> listSliderElementData = new List<SliderElementData>();

        [Header("*Child class")]
         
         

        [Header("Карусель")] 
        public SliderController MySliderController = new SliderController();

        public BilbordKeySelector pref_bilbordKeySelector;

        [HideInInspector]public GameObject tempCarsDir;
        public Transform carsListSpawn;
        public XrRenderWin xrRenderWin;


        private void Awake()
        {
            carsListSpawn.gameObject.SetActive(false);
        }
        void Start()
        {
            Init();


            carsListSpawn.gameObject.SetActive(true);



            MySliderController.Max = 3;
            MySliderController.Index = 0;
            Next(0);

            if (gameObject.activeSelf)
            {
                Open("");
            }
        }


        void Next(int increment)
        {

            MySliderController.Next(increment);
            myGarageCamera.ShowListCar(MySliderController.Index);

            if (listSliderElementData[MySliderController.Index].car)
            {
                CarConfigBase thecar = listSliderElementData[MySliderController.Index].car;

                MathToEngineResponseStruct response = engineMath.MathToEngineLevelList(thecar.engineTuneLevel);
                xrRenderWin.RenderUpdgradeData(response);
                xrRenderWin.Show();
                xrRenderWin.ResizeHorizontal(false);
            }
            else
            {
                xrRenderWin.Hide();
            }

            foreach (SliderElementData item in listSliderElementData)
            {
                if(item.index== MySliderController.Index)
                {
                    item.bilbord.SetSelect(true);
                }
                else
                {
                    item.bilbord.SetSelect(false);
                }
            }
        }


        public override void Close()
        {
            listSliderElementData.Clear();
            carsListSpawn.gameObject.SetActive(false); 
            xrRenderWin.Hide();
            Destroy(tempCarsDir);
            base.Close();
        }

        bool isInit = false;
        void InitMe()
        {
            if (isInit) return;
            isInit = true;

        }

        public override void Open(string data)
        {
            base.Open(data);
            InitMe();

            tempCarsDir = new GameObject();
            
            for (int i = 0; i < 4; i++)
            {

                Transform _spawnPoint = carsListSpawn.Find("SpawOtherCar").GetChild(i);
                SliderElementData E = new SliderElementData();
                E.bilbord = Instantiate(pref_bilbordKeySelector.gameObject).GetComponent<BilbordKeySelector>();

                E.bilbord.transform.SetParent(_spawnPoint);
                E.bilbord.transform.position = _spawnPoint.position + new Vector3(0, 0f, 0);
                E.bilbord.SetSelect(false);
                E.bilbord.SetTitle("КУПИТЬ МАШИНУ");

                E.index = i;
                if (i < garageController.GetCountCars())
                {
                  
                    CarConfigBase _car = garageController.SpawnMyCarNumber(i, _spawnPoint);
                    _car.transform.SetParent(tempCarsDir.transform);
                     
                   
                    E.car = _car;
                  
                    E.bilbord.transform.position = _spawnPoint.position + new Vector3(0, 1f, 0); 
                    E.bilbord.SetTitle(_car.carInfo.name);
                    E.bilbord.SetTitle(_car.gameObject.name);

                }
                else
                {

                }


                listSliderElementData.Add(E);
            }

          //  print("Open cars");
            // myGarageCamera.SetFocusPointThrough("Hood");
            MySliderController.Index =  garageController.dataStore.Data.currentCarId.Get();
            Next(0);

            fastSound.Play("open"); 
            if (!carConfigBase)
            {
                print("Erorr link!");
                return;
            }

            
            
        }

        public override void RouterModalCallback(bool responce, string action)
        {
            if (action == "sell")
            {
                garageController.RemoveCar(MySliderController.Index);
                Close();
                Open("");
            }

        }

        public override bool RouterAction(UWinBaseClass window, string action)
        {
           
            if (!base.RouterAction(window, action)) return false;

            if (action == "sell")
            {
                if (!listSliderElementData[MySliderController.Index].car)
                {
                    print("no car");
                    // modalWindow.ModalBuild("ok", "Нет тачки!", "");
                    return false;
                }


                if (garageController.dataStore.Data.cars.Count < 2)
                {
                    modalWindow.ModalBuild("ok", "Нельзя продать единственную машину!", "");
                }

                modalWindow.ModalBuild("sell", "Вы действительно хотите продать эту машину?", "Продать за "
                    + Mathf.Round(listSliderElementData[MySliderController.Index].car.carInfo.price / 2).ToString()
                    + "$"
                    );

                fastSound.Play("next");
                return false;
            }


            if (action == "Left")
            {
                fastSound.Play("next");
                Next(1);
            }
            if (action == "Right")
            {
                fastSound.Play("next");
                Next(-1);
            }

            if (action == "Enter")
            {
                if(MySliderController.Index-1< garageController.GetCountCars())
                {
                    garageController.SetCurrentCar(MySliderController.Index);
                }
                else
                {
                    print("open shop");
                    Close();
                    navigationWindowsNext.Open();
                    return true;
                }

                Close();
                navigationWindowsBack.Open();
                //MySliderController.Index 
                return false;
                
                   
            }
            if (action == "Exit")
            {
                Close();
                navigationWindowsBack.Open();
                return false;
            }

        

            return true;
        }

    }
}