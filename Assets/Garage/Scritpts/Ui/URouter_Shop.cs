using CarTunr;
using Garage.Plugins;
using System.Collections.Generic;
using UnityEngine;
using LuaApi;
using UnityEngine.Serialization;

namespace Garage.Scritpts
{



    public class URouter_Shop : URouter_BaseClass
    {

        struct SliderElementData
        {
            public GameObject gameObject;
            public int index;
            internal CarConfigBase car;
            internal BilbordKeySelector bilbord;
            public UItemCarusel uItemCarusel;
        }

        List<SliderElementData> listSliderElementData = new List<SliderElementData>();

        [Header("*Child class")]
         
         

        [Header("Карусель")] 
        public SliderController MySliderController = new SliderController();
        public DetalCaruselController detalCaruselController;
        public BilbordKeySelector pref_bilbordKeySelector;

        [Header("Итем карусели")]
        public UItemCarusel prefabUItemCarusel;

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



            MySliderController.Max = 2;
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
            myGarageCamera.ShowListCar(0);

            if (listSliderElementData[MySliderController.Index].car)
            {
              
                CarConfigBase thecar = listSliderElementData[MySliderController.Index].car;

                MathToEngineResponseStruct response = engineMath.MathToEngineLevelList(thecar.engineTuneLevel);
                xrRenderWin.RenderUpdgradeData(response);
                xrRenderWin.Show();
                xrRenderWin.ResizeHorizontal(false);
            }

            detalCaruselController.GoTo(MySliderController.Index);

            foreach (SliderElementData item in listSliderElementData)
            {
                if(item.index== MySliderController.Index)
                {
              
                    item.gameObject.SetActive(true);
                    item.gameObject.transform.position += new Vector3(0, 0.061f, 0f);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }


        public override void Close()
        {
            
            carsListSpawn.gameObject.SetActive(false);
            base.Close();
            xrRenderWin.Hide();
            tempCarsDir.SetActive(false);
        }

        bool isInit = false;
        void InitMe()
        {
            if (isInit) return;
            isInit = true;



            tempCarsDir = new GameObject();
            tempCarsDir.name = "CAR_SHOP_DIR";

            Transform _spawnPoint = carsListSpawn.Find("SpawOtherCar").GetChild(0);

            for (int i = 0; i < projectDatabaseClass.carSpawnerSystem.brends.Count; i++) {

                
                CarConfigBase theCar = projectDatabaseClass.carSpawnerSystem.brends[i];

                SliderElementData E = new SliderElementData();
                E.bilbord = Instantiate(pref_bilbordKeySelector.gameObject).GetComponent<BilbordKeySelector>();

                theCar.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;

                 
                E.gameObject = Instantiate(theCar.gameObject);
                E.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;

                E.car = theCar.gameObject.GetComponent<CarConfigBase>();

                E.gameObject.transform.position = _spawnPoint.position;
                E.gameObject.transform.rotation  = _spawnPoint.rotation;

                foreach (Transform child in E.gameObject.transform.GetDescendants())
                {
                    if (child.GetComponent<RCC_WheelCollider>())
                    {
                        Destroy(child.GetComponent<RCC_WheelCollider>());
                    }
                    if (child.GetComponent<RCC_Chassis>())
                    {
                        Destroy(child.GetComponent<RCC_Chassis>());
                    }
                }

                print("spawn shop car " + E.gameObject.name);


                E.gameObject.SetActive(false);

                E.gameObject.transform.SetParent(tempCarsDir.transform);


                E.bilbord.transform.SetParent(E.gameObject.transform);
                E.bilbord.transform.position = E.gameObject.transform.position + new Vector3(0, 1.1f, 0);
                E.bilbord.SetSelect(true);
                E.bilbord.SetTitle(theCar.carInfo.name);

                E.index = i;

                GameObject itemSlider = Instantiate(prefabUItemCarusel.gameObject) as GameObject;
                E.uItemCarusel = itemSlider.GetComponent<UItemCarusel>();
                E.uItemCarusel.gameObject.name = i.ToString() + " _XXXXXXX"; 
                E.uItemCarusel.SetSprite(projectDatabaseClass.brends[i]);
                E.uItemCarusel.SetCurent(false);
                E.uItemCarusel.SetPrice(theCar.carInfo.price);
                E.uItemCarusel.SetSelect(false);
                E.uItemCarusel.SetTitle(theCar.carInfo.name); 
                detalCaruselController.AddItem(itemSlider, i);


                listSliderElementData.Add(E);
            }

            //  print("Open cars");
            // myGarageCamera.SetFocusPointThrough("Hood");
            MySliderController.Max = projectDatabaseClass.carSpawnerSystem.brends.Count;
            MySliderController.Index = 0;
            Next(0);

        }

        public override void Open(string data)
        {
            base.Open(data);
            InitMe();

            tempCarsDir.SetActive(true);
            Next(0);

            fastSound.Play("open"); 
            if (!carConfigBase)
            {
                print("Erorr link!");
                return;
            }

            
            
        }

    


        public override bool RouterAction(UWinBaseClass window, string action)
        {
           
            if (!base.RouterAction(window, action)) return false;
      

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

               // print(listSliderElementData[MySliderController.Index].car.carInfo.price);
                if (!projectDatabaseClass.dataStore.Data.Money.Take(listSliderElementData[MySliderController.Index].car.carInfo.price))
                {
                    modalWindow.ModalBuild("ok", "Не хватает денег!", "");
                    return false;
                }

              
                    garageController.AddCar(listSliderElementData[MySliderController.Index].car.gameObject.name);
             
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