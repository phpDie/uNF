using Garage.Plugins;
using System.Collections.Generic;
using LinkerGarage;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garage.Scritpts
{

 

    public class URouterVisualSelect : URouter_BaseClass
    {

        struct SliderElementData
        {
            public GameObject gameObject;
            public UItemCarusel uItemCarusel;
            public VisualDetal visualDetal;
            public bool isCurentInstall;
            public int price;
            public int index;
        }

          List<SliderElementData> listSliderElementData = new List<SliderElementData>();

        [Header("*Child class")]
           

        [Header("Карусель")]
        public DetalCaruselController detalCaruselController;
        public SliderController MySliderController = new SliderController();

        [Header("Итем карусели")]
        public UItemCarusel prefabUItemCarusel;

        string currentVisualTuneInd = "BumperForward";
        // public List<string> DetalList = new List<string>();

        void ShowPreview(int id)
        {
            visualTuneHelperSelection.SetTuneIndTo(currentVisualTuneInd, id);
            UpdSlider();

            fastSound.Play("next");
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
        {
            visualTuneHelperSelection.ResetTune_ToData();
            detalCaruselController.ClearAllItems();
            base.Close();
        }

        public override void Open(string data)
        {
            base.Open(data);
            baseInit();
            detalCaruselController.Init();
            currentVisualTuneInd = data;

          //  print(currentVisualTuneInd);

            listSliderElementData.Clear();

            fastSound.Play("open");
            //print("Open " + Data);
            detalCaruselController.ClearAllItems();
            int dCount = visualTuneHelperSelection.GetTuneIndCount(currentVisualTuneInd);
            // print("detal count" + dCount.ToString());

            // print("Cur select" + visualTuneHelperSelection.DataVisual[currentVisualTuneInd].ToString());
            MySliderController.Max = dCount - 1;

            for (var i = 0; i < dCount; i++)
            {
                
                SliderElementData E = new SliderElementData();

                E.gameObject = Instantiate(prefabUItemCarusel.gameObject) as GameObject;
                E.gameObject.name = i.ToString();


                VisualDetal? visualDetal = projectDatabaseClass.tuneDatabaseClass.GetDataFromKey(currentVisualTuneInd);
                E.visualDetal = visualDetal.Value;

                 

                E.isCurentInstall = (visualTuneHelperSelection.DataVisual[currentVisualTuneInd] == i);

                
                E.price = 0;
                if (i > 0 && !E.isCurentInstall)
                {
                    E.price = Mathf.RoundToInt(carConfigBase.carInfo.price * visualDetal.Value.priceCoff);
                }


                

                if(Linker.GetGarageController().car.IseetVisualPart(currentVisualTuneInd, i))
                {
                    E.price = 0;
                }


                E.uItemCarusel = E.gameObject.GetComponent<UItemCarusel>();
                
                if (projectDatabaseClass.brends.Count >= i - 1)
                {
                    E.uItemCarusel.SetSprite(projectDatabaseClass.brends[i]);
                }
                else {
                    E.uItemCarusel.SetSprite(projectDatabaseClass.brends[i - projectDatabaseClass.brends.Count]);
                }
                
                E.uItemCarusel.SetCurent(E.isCurentInstall);
                E.uItemCarusel.SetPrice(E.price);
                E.uItemCarusel.SetSelect(false);

                if (i == 0)
                {
                    E.uItemCarusel.SetTitle("Заводская");
                }
                
                detalCaruselController.AddItem(E.gameObject, i);
                
              //  listSliderElementData[i] = E;
                listSliderElementData.Add(E);
            }

            MySliderController.Index = visualTuneHelperSelection.DataVisual[currentVisualTuneInd];
            Next(0);
            // print("OPEN DATA seletct: " + Data);
        }

        public override bool RouterAction(UWinBaseClass window, string action)
        {
            if (!base.RouterAction(window, action)) return false;

            if (action == "Left")
            {
                Next(-1);
            }
            if (action == "Right")
            {
                Next(1);
            }

            if (action == "Enter")
            {

                if (!Linker.GetGarageController().car.IseetVisualPart(currentVisualTuneInd, MySliderController.Index))
                {


                    if (!projectDatabaseClass.dataStore.Data.Money.Take(listSliderElementData[MySliderController.Index].price))
                    {
                        modalWindow.ModalBuild("ok", "Не хватает денег!", "");
                        return false;
                    }

                    Linker.GetGarageController().car.AddVisualPart(currentVisualTuneInd, MySliderController.Index);
                }


                fastSound.Play("install");
                visualTuneHelperSelection.DataVisual[currentVisualTuneInd] = MySliderController.Index;

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