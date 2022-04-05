using Garage.Plugins;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garage.Scritpts
{



    public class URouter_GarageMenu : URouter_BaseClass
    {

        struct SliderElementData
        {
            public GameObject gameObject;
            public UItemCarusel uItemCarusel;
            public int index;
            public string ind;
        }

        List<SliderElementData> listSliderElementData = new List<SliderElementData>();

        [Header("*Child class")]

        public URouter_BaseClass win_Balance;
        public URouter_BaseClass win_Visual;
        public URouter_BaseClass win_Engine;
        public URouter_BaseClass win_Color;
        public URouter_BaseClass win_Cars;
        public URouter_BaseClass win_Shop;


        [Header("Карусель")]
        public DetalCaruselController detalCaruselController;
        public SliderController MySliderController = new SliderController();

        [Header("Итем карусели")]
        public UItemCarusel prefabUItemCarusel;




        public XrRenderWin xrRenderWin;

        void UpdSlider()
        {
            //print(listSliderElementData.Count);
            for (var i = 0; i < listSliderElementData.Count; i++)
            {
                SliderElementData _select = listSliderElementData[i];
                _select.uItemCarusel.SetSelect((MySliderController.Index == i));
            }
        }
        void SelectFromCarusel(int id)
        {
            if (MySliderController.Index == id) return;

            print("SelectFromCarusel");
            print(id);
            MySliderController.Index = id;

            UpdSlider();
        }


        int _indexerItemMenus = -1;
        public void addMenuItem(string ind, string Title, Sprite _sprite = null)
        {
            _indexerItemMenus++;

            SliderElementData E = new SliderElementData();
            E.gameObject = Instantiate(prefabUItemCarusel.gameObject) as GameObject;
            E.gameObject.name = _indexerItemMenus.ToString() + " _Islider";

            E.uItemCarusel = E.gameObject.GetComponent<UItemCarusel>();
            //   E.uItemCarusel.SetSprite(_sprite);
            E.uItemCarusel.SetCurent(false);
            E.uItemCarusel.SetPrice(-1);
            E.uItemCarusel.SetSelect(false);
            E.uItemCarusel.SetBackgroundColor(new Color32(0,0,0,240));


            E.uItemCarusel.SetTitle(Title);
            E.ind = ind;

            detalCaruselController.AddItem(E.gameObject, _indexerItemMenus);
            listSliderElementData.Add(E);
        }

        void Start()
        {
            Init();

            detalCaruselController.Init();
            detalCaruselController.simpleScrollSnap.OnPanelSelecting.AddListener(SelectFromCarusel);

            //listSliderElementData.Clear();


            addMenuItem("visual", "Визуалка");
            addMenuItem("engine", "Двигатель");
            addMenuItem("race", "В гонку");
            addMenuItem("color", "Краска");
            addMenuItem("balance", "Баланс");
            addMenuItem("shop", "Shop");
            // addMenuItem("cars", "Мои машины");




            MySliderController.Max = _indexerItemMenus;
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

            detalCaruselController.GoTo(MySliderController.Index);

            UpdSlider();
        }


        public override void Close()
        {
            base.Close();
            xrRenderWin.Hide();
        }

        public override void Open(string data)
        {
            base.Open(data);


            myGarageCamera.SetFocusPointThrough("Hood");

            fastSound.Play("open");
            if (!carConfigBase)
            {
                print("Erorr link!");
                return;
            }

            // print("EngineMath use");
            MathToEngineResponseStruct response = engineMath.MathToEngineLevelList(carConfigBase.engineTuneLevel);
            xrRenderWin.RenderUpdgradeData(response);
            xrRenderWin.Show();
            xrRenderWin.ResizeHorizontal(false);

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

                string _select = listSliderElementData[MySliderController.Index].ind;

                if (_select == "balance")
                {
                    Close();
                    win_Balance.Open();
                    return false;
                }
                if (_select == "visual")
                {
                    Close();
                    win_Visual.Open();
                    return false;
                }

                if (_select == "color")
                {
                    Close();
                    win_Color.Open();
                    return false;
                }

                if (_select == "shop")
                {
                    Close();
                    win_Shop.Open();
                    return false;
                }

                if (_select == "engine")
                {
                    Close();
                    win_Engine.Open();
                    return false;
                }
                if (_select == "race")
                {
                    garageController.RaceStart();
                    modalWindow.ModalBuild("ok", "Загрузка гонки", "load...");
                    return false;
                }
                modalWindow.ModalBuild("ok", "Этот режим недоступен!", "");
                return false;


                //Close();                navigationWindowsBack.Open();
            }
            if (action == "Exit")
            {
                garageController.CurrentCarSave();
                win_Cars.Open();
                Close();
                return false;
            }



            return true;
        }

    }
}