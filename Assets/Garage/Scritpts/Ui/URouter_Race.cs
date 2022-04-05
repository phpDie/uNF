using Garage.Plugins;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Garage.Scritpts
{



    public class URouter_Race : URouter_BaseClass
    {

       

      
        void Start()
        {
            Init();
             
        }
         


        public override void Close()
        {
            base.Close(); 
        }

        public override void Open(string data)
        {
            base.Open(data);
              
        }

        public override void RouterModalCallback(bool responce, string action)
        {
            if (!base.RouterAction(null, action)) return;

          
            if (action == "exitFromRace" && responce)
            {
                modalWindow.ModalBuild("load", "", "load...");

                SceneManager.LoadScene("GarageSceneMain");
            }
             
        }

        public override bool RouterAction(UWinBaseClass window, string action)
        { 
            if (!base.RouterAction(window, action)) return false;
          
            if (action == "Enter")
            {
                fastSound.Play("next");
              
            }
           
            if (action == "Exit")
            { 
                modalWindow.ModalBuild("exitFromRace", "Вы хотите выйти из гонки?", "", true);
                return false;
            } 

            return true;
        }

    }
}