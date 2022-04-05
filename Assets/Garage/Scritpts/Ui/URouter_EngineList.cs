using Garage.Plugins;
using PowerParametrs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garage.Scritpts
{

 

    public class URouter_EngineList : URouter_BaseClass
    {

        struct SliderElementData
        {
            public GameObject gameObject;
            public UItemCarusel uItemCarusel;  
            public string ind;
            public int index;
            public ItemTunePowerStruct theTune;
        }

          List<SliderElementData> listSliderElementData = new List<SliderElementData>();

        [Header("*Child class")]
         

        

        [Header("Карусель")]
        public DetalCaruselController detalCaruselController;
        public SliderController MySliderController = new SliderController();

        [Header("Итем карусели")]
        public UItemCarusel prefabUItemCarusel;

        public XrRenderWin xrRenderWin; 



        public GameObject engineObj;
        void ShowPreview(int id)
        {
            UpdSlider();
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
            _defEngineY = engineObj.transform.position.y;


            if (!detalCaruselController.simpleScrollSnap)
            {
                detalCaruselController.Init();
            }
            detalCaruselController.simpleScrollSnap.OnPanelSelecting.AddListener(SelectFromCarusel);


            int dCount = projectDatabaseClass.indEngineTune.list.Count;
           MySliderController.Max = dCount -1;

            for (var i = 0; i < dCount; i++)
            {

                SliderElementData E = new SliderElementData();

                E.gameObject = Instantiate(prefabUItemCarusel.gameObject) as GameObject;
                E.gameObject.name = i.ToString();


                ItemTunePowerStruct theTune=  projectDatabaseClass.indEngineTune.list[i];
                
  E.theTune = theTune;

                E.ind = theTune.ind;




                E.uItemCarusel = E.gameObject.GetComponent<UItemCarusel>();
                E.uItemCarusel.SetSprite(projectDatabaseClass.brends[i]);
                E.uItemCarusel.SetCurent(false);
                E.uItemCarusel.SetPrice(0);
                E.uItemCarusel.SetSelect(false);

                int currentLevelInCar = carConfigBase.GetComponent<CarTunr.CarConfigBase>().engineTuneLevel[theTune.ind];
                
                E.uItemCarusel.SetBackgroundColor(projectDatabaseClass.indEngineTune.colorFromLevel[currentLevelInCar]);

                
                E.uItemCarusel.SetTitle(theTune.name.ToUpper());

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
            if (true)
            {
                return;
            }
            GameObject Bonnet =  visualTuneHelperSelection.GetTuneGameObject_FromInd("Bonnet");

            if (val)
            {
                engineObj.transform.LeanMoveY(_defEngineY + 0.34f, 0.45f).setDelay(0.04f).setEaseOutQuint();
                Bonnet.transform.LeanRotateX(-70f, 0.5f).setEaseOutQuint();
                myGarageCamera.SetFocusPointThrough("Bonnet");
            }
            else {

                engineObj.transform.LeanMoveY(_defEngineY , 0.3f).setEaseOutQuint();
                Bonnet.transform.LeanRotateX(0f, 0.36f).setEaseOutBounce().setDelay(0.02f);
                myGarageCamera.SetFocusPointThrough("BumperRight");
            }
            
        }

        public override void Close()
        {

            xrRenderWin.ResizeHorizontal(false);
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

            xrRenderWin.Show();
            xrRenderWin.ResizeHorizontal(true);

            fastSound.Play("open");
            //print("count " + listSliderElementData.Count.ToString());

            for (var i = 0; i < listSliderElementData.Count; i++)
            {
              //  print(i);
                string _ind = listSliderElementData[i].ind;
                // print(_ind);
             //   print("x");
               // if (!carConfigBase.engineTuneLevel.ContainsKey(_ind)) continue;
                int curLevel = carConfigBase.engineTuneLevel[_ind];
                listSliderElementData[i].uItemCarusel.SetBackgroundColor(projectDatabaseClass.indEngineTune.colorFromLevel[curLevel]);
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
                

                Close();
                navigationWindowsNext.Open(projectDatabaseClass.indEngineTune.list[MySliderController.Index].ind);
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