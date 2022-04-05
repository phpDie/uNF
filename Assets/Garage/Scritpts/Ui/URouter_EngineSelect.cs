using Garage.Plugins;
using LinkerGarage;
using PowerParametrs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garage.Scritpts
{

 

    public class URouter_EngineSelect : URouter_BaseClass
    {

        struct SliderElementData
        {
            public GameObject gameObject;
            public UItemCarusel uItemCarusel;  
            public string ind;
            public int index;
            public int price;
            public bool isCurentInstall;
            public ItemTunePowerStruct theTune;
        }

          List<SliderElementData> listSliderElementData = new List<SliderElementData>();

        [Header("*Child class")]
 

        
        public XrRenderWin xrRenderWin;
         


        [Header("Карусель")]
        public DetalCaruselController detalCaruselController;
        public SliderController MySliderController = new SliderController();

        [Header("Итем карусели")]
        public UItemCarusel prefabUItemCarusel;

        string SELECT_IND;
        ItemTunePowerStruct SELECT_TUNE;
         
        void ShowPreview(int id)
        {
                 print("EngineMath use");
            Dictionary<string, int> copy = new Dictionary<string, int>(carConfigBase.engineTuneLevel);
            copy[SELECT_IND] = id;

            MathToEngineResponseStruct response = engineMath.GetMathEngineToLevel_Delta(carConfigBase.engineTuneLevel, copy);

            xrRenderWin.RenderUpdgradeData(response, true);
            UpdSlider();
        }


        void SelectFromCarusel(int id)
        {
            if (MySliderController.Index == id) return;

            print("SelectFromCarusel");
            print(id);
            MySliderController.Index = id;
            ShowPreview(MySliderController.Index);
        }

        bool isBaseInit = false;
        void baseInit()
        {
            if (isBaseInit) return;
            isBaseInit = true;

            if (!detalCaruselController.simpleScrollSnap)
            {
                detalCaruselController.Init();
            }
            detalCaruselController.simpleScrollSnap.OnPanelSelecting.AddListener(SelectFromCarusel);


            
        }

        void GenerateSlider()
        {
             
            MySliderController.Max = SELECT_TUNE.maxLevel - 1;

            for (var i = 0; i < SELECT_TUNE.maxLevel; i++)
            {

                SliderElementData E = new SliderElementData();

                E.gameObject = Instantiate(prefabUItemCarusel.gameObject) as GameObject;
                E.gameObject.name = i.ToString();


                ItemTunePowerStruct theTune = projectDatabaseClass.indEngineTune.list[i];
                E.theTune = theTune;



                E.uItemCarusel = E.gameObject.GetComponent<UItemCarusel>();
                E.uItemCarusel.SetSprite(projectDatabaseClass.brends[i]);
                E.uItemCarusel.SetCurent(false); 
                E.uItemCarusel.SetSelect(false);
                E.uItemCarusel.SetBackgroundColor(projectDatabaseClass.indEngineTune.colorFromLevel[i]);


                E.uItemCarusel.SetTitle("LEVEL " + i.ToString());

                if (i == 0)
                {
                    E.uItemCarusel.SetTitle("Заводской вариант");
                }

                if (!carConfigBase.engineTuneLevel.ContainsKey(SELECT_IND))
                {
                    print("НЕ НАЙДЕН КЛЮЧ " + SELECT_IND);
                    print("engineTuneLevel count: " + carConfigBase.engineTuneLevel.Count.ToString());
                    continue;
                }
                E.isCurentInstall = (carConfigBase.engineTuneLevel[SELECT_IND] == i);

                E.price = 0;
                if (i > 0 && !E.isCurentInstall)
                {
                     
                    E.price = Mathf.RoundToInt(carConfigBase.carInfo.price * theTune.priceForLevel*i);
                }

                if (Linker.GetGarageController().car.IseetVisualPart(SELECT_IND, i))
                {
                    E.price = 0;
                }

                E.uItemCarusel.SetPrice(E.price);

                detalCaruselController.AddItem(E.gameObject, i);

                listSliderElementData.Add(E);
            }

            MySliderController.Index = carConfigBase.engineTuneLevel[SELECT_IND];
            Next(0);

        }
         
        void Start()
        {
            Init();
            baseInit();
             
        }

        void UpdSlider()
        {
            //print(listSliderElementData.Count);
            for (var i = 0; i < listSliderElementData.Count; i++)
            {
                SliderElementData _select = listSliderElementData[i];
                _select.uItemCarusel.SetSelect((MySliderController.Index == i));
            }
        }

        void Next(int increment)
        {

            MySliderController.Next(increment);
            ShowPreview(MySliderController.Index);
            detalCaruselController.GoTo(MySliderController.Index);
           
        }

      

        public override void Close()
        {     print("EngineMath use");

            MathToEngineResponseStruct response = engineMath.MathToEngineLevelList(carConfigBase.engineTuneLevel);
            xrRenderWin.RenderUpdgradeData(response);
            xrRenderWin.Show();
            xrRenderWin.ResizeHorizontal(false);

            for (var i = 0; i < listSliderElementData.Count; i++)
            {
                SliderElementData _select = listSliderElementData[i];
                Destroy(_select.gameObject);
            }
            listSliderElementData.Clear();
            detalCaruselController.ClearAllItems();
            base.Close();
        }

        public override void Open(string data)
        {
            base.Open(data);
            baseInit();
            SELECT_IND = data;
            SELECT_TUNE = projectDatabaseClass.indEngineTune.list.Find(item => item.ind == SELECT_IND);
          //  print(SELECT_TUNE.ind + " select!");
            GenerateSlider();


            fastSound.Play("open");
        }

        public override bool RouterAction(UWinBaseClass window, string action)
        {
            if (!base.RouterAction(window, action)) return false;

            if (action == "Left")
            {
                fastSound.Play("next");
                Next(-1); 
            }
            if (action == "Right")
            {
                fastSound.Play("next");
                Next(1);
            }

            if (action == "Enter")
            {
                 

                if (listSliderElementData[MySliderController.Index].price > 0){
                    if (!Linker.GetGarageController().car.IseetVisualPart(SELECT_IND, MySliderController.Index))
                    {
                        if (!projectDatabaseClass.dataStore.Data.Money.Take(listSliderElementData[MySliderController.Index].price))
                        {
                            modalWindow.ModalBuild("ok", "Не хватает денег!", "");

                            
                            return false;
                        }

                        Linker.GetGarageController().car.AddVisualPart(SELECT_IND, MySliderController.Index);
                    }
                }

                fastSound.Play("install");

                carConfigBase.engineTuneLevel[SELECT_IND] = MySliderController.Index;

                Close();
                navigationWindowsBack.Open();
            }
            if (action == "Exit")
            {
                Close();
                navigationWindowsBack.Open();
            }

            return true;
        }

    }
}