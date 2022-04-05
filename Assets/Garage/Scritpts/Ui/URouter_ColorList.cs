using Garage.Plugins;
using PowerParametrs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garage.Scritpts
{

 

    public class URouter_ColorList : URouter_BaseClass
    {

        struct SliderElementData
        {
            public GameObject gameObject;
            public UItemCarusel uItemCarusel;  
            public string ind;
            public int index; 
        }

          List<SliderElementData> listSliderElementData = new List<SliderElementData>();

        [Header("*Child class")]
         

        

        [Header("Карусель")]
        public DetalCaruselController detalCaruselController;
        public SliderController MySliderController = new SliderController();

        [Header("Итем карусели")]
        public UItemCarusel prefabUItemCarusel;

        public XrRenderWin xrRenderWin; 

        
        void ShowPreview(int id)
        {
            UpdSlider();
            visualTuneHelperSelection.SetColor(id);
        }


        void SelectFromCarusel(int id)
        {
            if (MySliderController.Index == id) return;

          //  print("SelectFromCarusel");
           // print(id);
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


            int dCount = projectDatabaseClass.tuneDatabaseClass.colorList.Count;
           MySliderController.Max = dCount -1;
             
            for (var i = 0; i < dCount; i++)
            {

                SliderElementData E = new SliderElementData();

                E.gameObject = Instantiate(prefabUItemCarusel.gameObject) as GameObject;
                E.gameObject.name = i.ToString();
                 




                E.uItemCarusel = E.gameObject.GetComponent<UItemCarusel>();
                E.uItemCarusel.SetSprite(null);
                E.uItemCarusel.SetCurent(false);
                E.uItemCarusel.SetPrice(0);
                E.uItemCarusel.SetSelect(false);

               
                E.uItemCarusel.SetBackgroundColor(projectDatabaseClass.tuneDatabaseClass.colorList[i]);


                E.uItemCarusel.SetTitle("Цвет");

                detalCaruselController.AddItem(E.gameObject, i);

                
                listSliderElementData.Add(E);
            }

           
            Next(0);
        }

        float _defEngineY;
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

        void setOpenBonet(bool val)
        {
            

       
        }

        public override void Close()
        {
             
            setOpenBonet(false);
            visualTuneHelperSelection.ResetTune_ToData();
            base.Close();
        }

        public override void Open(string data)
        {
            base.Open(data);
            baseInit();


            baseInit();

            setOpenBonet(true);
 
            fastSound.Play("open");
         
            for (var i = 0; i < listSliderElementData.Count; i++)
            {
            }

        
            // print("OPEN DATA seletct: " + Data);
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
                visualTuneHelperSelection.colorId = MySliderController.Index;

               Close();
                navigationWindowsBack.Open();

            }
            if (action == "Exit")
            {
               // xrRenderWin.Hide();
                Close();
                navigationWindowsBack.Open();
            }

            return true;
        }

    }
}