using Garage.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using CarTunr;

namespace Garage.Scritpts
{
    public class SliderController
    {

        public int Index;
        public int Max = 2;
        public virtual int Next(int increment)
        {

            Index += increment;
            if (Index > Max) Index = 0;
            if (Index < 0) Index = Max;

            return Index;
        }
    }

    


    public class URouter_BaseClass : MonoBehaviour
    {

        [Header("Звуки")]
        public List<ClipSoundStruct> listClip = new List<ClipSoundStruct>();

        [HideInInspector]
        public FastSound fastSound;


        [HideInInspector]public GarageController garageController;

        [Header("Base calss")]
        [Header("Навигация по окнам")]
        public URouter_BaseClass navigationWindowsNext;
        public URouter_BaseClass navigationWindowsBack;


        [Header("Ссылка на модалку")]
        public ModalWindow modalWindow;


        [Header("База данных")]
        public Project_DatabaseClass projectDatabaseClass;

        [Header("Гараж камера")]
        public GarageCamera myGarageCamera;


      
        [HideInInspector] public CarConfigBase carConfigBase {
            get
            {
                return garageController.car;
            }
        }
        [HideInInspector] 
        public VisualTuneHelperSelection visualTuneHelperSelection
        {
            get
            {
                return carConfigBase.visualTuneHelperSelection;
            }
        }

        [HideInInspector] public EngineMath engineMath; 


        [HideInInspector]
        public UWinBaseClass myUWinBaseClass;

      
        private bool modalCanBlocked = true;


        public virtual void RouterModalCallback(bool responce, string action)
        {
            if (!modalCanBlocked) return;
            
        }

        public virtual bool RouterAction(UWinBaseClass window, string action)
        {
            if (modalCanBlocked)
            {
                if (modalWindow.isOpen)
                {
                //    print("modal blocked");
                    return false;
                }

            }
            return true;
        }

        void _CallbackRouterAction(UWinBaseClass window, string action)
        {
            RouterAction(window, action);

       }


        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        public virtual void Open(string data = "")
        {
            Init();
            gameObject.SetActive(true);
        }


        bool isInit = false;
        // Start is called before the first frame update
        public void Init()
        {
            if (isInit) return;
            isInit = true;

                fastSound = gameObject.AddComponent<FastSound>();
            fastSound.listClip = listClip;
            fastSound.init();


            myUWinBaseClass = gameObject.GetComponent<UWinBaseClass>();

            myUWinBaseClass.myCallbackActionEvent.AddListener(_CallbackRouterAction);

            if (gameObject.GetComponent<ModalWindow>())
            {
                modalCanBlocked = false;
            }


            if (modalWindow)
            {
                modalWindow.unityEvent.AddListener(RouterModalCallback);
            }

            if (GameObject.Find("GarageController"))
            {
                garageController = GameObject.Find("GarageController").GetComponent<GarageController>();
             //   carConfigBase = garageController.GetCurrentCar();
                engineMath = carConfigBase.gameObject.GetComponent<EngineMath>();

                
            }

         

        }

    }
}