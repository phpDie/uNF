using Garage.Plugins;
using PowerParametrs;
using System.Collections.Generic;
using UnityEngine;
using LuaApi;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Garage.Scritpts
{

 

    public class URouter_Balance: URouter_BaseClass
    {

        struct SliderElementData
        {
            public GameObject gameObject;
            public UItemCarusel uItemCarusel;  
            public string ind;
            public int index;
            public int val;
            public BalanceStruct balanceStruct;
            public List<GameObject> list;
        }

        List<SliderElementData> listSliderElementData = new List<SliderElementData>();

        [Header("*Child class")]
         

        

        [Header("Карусель")]
        public DetalCaruselController detalCaruselController;
        public SliderController MySliderController = new SliderController();

        [Header("Итем карусели")]
        public GameObject balanceElementRow;

        public XrRenderWin xrRenderWin;


        void ShowPreview(int id)
        {
            UpdSlider();
            /*
            foreach (SliderElementData E in listSliderElementData)
            {
                RectTransform panel = E.gameObject.GetComponent<RectTransform>();
                if(MySliderController.Index == id)
                {
                    panel.LeanAlpha(1f, 0.19f);
                }
                else
                {
                    panel.LeanAlpha(0.2f, 0.19f);
                }
                
            }
             */


        }

        void SelectFromCarusel(int id)
        {
            if (MySliderController.Index == id) return;

            //  print("SelectFromCarusel");
            // print(id);
            MySliderController.Index = id;
            ShowPreview(MySliderController.Index);

        }

        void SetBalanceSelect(string ind, int val)
        {
            //print("SetBalanceSelect " + ind + " to " + val.ToString());

            foreach (SliderElementData E in listSliderElementData)
            {
                if (E.balanceStruct.ind != ind) continue;

                for (int j = -5; j <= 5; j++)
                {
                    Transform _circle = E.gameObject.transform.Find("Panel").Find(j.ToString());

                    //Image icon =  _circle.gameObject.GetComponent<Image>();
                    RectTransform icon =  _circle.gameObject.GetComponent<RectTransform>(); 
                    if (j == val)
                    {
                        icon.LeanAlpha(1f, 0.08f);
                        icon.LeanScale(new Vector3(1,1,1)*1f, 0.15f);
                    }
                    else
                    {
                        icon.LeanAlpha(0.3f, 0.08f);
                        icon.LeanScale(new Vector3(1, 1, 1) * 0.6f, 0.08f);
                    }
                    // E.list[j] = _circle;
                }

            }
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

          //  print("baseInit base init");

            int dCount = projectDatabaseClass.indEngineTune.balanceList.Count;
            MySliderController.Max = dCount -1;
             
            for (var i = 0; i < dCount; i++)
            {

                SliderElementData E = new SliderElementData();

                E.balanceStruct = projectDatabaseClass.indEngineTune.balanceList[i];

                E.gameObject = Instantiate(balanceElementRow) as GameObject;
                E.gameObject.name = i.ToString();

                E.gameObject.transform.SetTextIneerElement("_title", E.balanceStruct.name);
                E.gameObject.transform.SetTextIneerElement("_left", E.balanceStruct.left);
                E.gameObject.transform.SetTextIneerElement("_right", E.balanceStruct.right);
                E.val = 0;
                Transform _elementCircle = E.gameObject.transform.Find("Panel").Find("element_Item");

                E.list =  new List<GameObject>();
                for (int j =-5; j <= 5; j++)
                {
                    GameObject _circle = Instantiate(_elementCircle.gameObject) as GameObject;
                    _circle.name = j.ToString();
                    _circle.transform.SetParent(E.gameObject.transform.Find("Panel"));
                   // E.list[j] = _circle;
                }

                

                Destroy(_elementCircle.gameObject); 


                detalCaruselController.AddItem(E.gameObject, i);

                SetBalanceSelect(E.balanceStruct.ind, 0);

                listSliderElementData.Add(E);
            }


            Destroy(balanceElementRow);
           // print("baseInit base init end");
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
                //SliderElementData _select = listSliderElementData[i];
              //  _select.uItemCarusel.SetSelect((MySliderController.Index == i));
            }
        }

        void BalancNext(int increment)
        {
            BalanceStruct balanceStruct  = listSliderElementData[MySliderController.Index].balanceStruct;
            string CurrentInd = balanceStruct.ind;

            SliderElementData E = listSliderElementData[MySliderController.Index];

            carConfigBase.DataBalance[CurrentInd] += increment;
            // E.val = Mathf.Max(-5, E.val);
            //E.val = Mathf.Min(5, E.val);
            carConfigBase.DataBalance[CurrentInd] = Mathf.Clamp(carConfigBase.DataBalance[CurrentInd], -5, 5);
            SetBalanceSelect(CurrentInd, carConfigBase.DataBalance[CurrentInd]);

        }

        void Next(int increment)
        {

           // print("balance next");
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

            base.Close();
        }

        public override void Open(string data)
        {
            base.Open(data);
            baseInit();


            baseInit();

            setOpenBonet(true);
 
            fastSound.Play("open");


            foreach (SliderElementData E in listSliderElementData)
            {
                SetBalanceSelect(E.balanceStruct.ind, carConfigBase.DataBalance[E.balanceStruct.ind]);
            }


            // print("OPEN DATA seletct: " + Data);
        }

        public override bool RouterAction(UWinBaseClass window, string action)
        {
            if (!base.RouterAction(window, action)) return false;

            if (action == "Left")
            {
                fastSound.Play("next");
                BalancNext(-1);
            }
            if (action == "Right")
            {
                fastSound.Play("next");
                BalancNext(1);
            }

            if (action == "Up")
            {
                fastSound.Play("next");
                Next(-1);
            }
            if (action == "Down")
            {
                fastSound.Play("next");
                Next(1);
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