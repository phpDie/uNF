using UnityEngine;
namespace Garage.Scritpts
{
    public class URouterVisualList : URouter_BaseClass
    {

        

        public override void Open(string data)
        {
            base.Open(data);
            myGarageCamera.pointsList.SetActive(true);
            fastSound.Play("open");
        }

        public override void Close()
        { 
            base.Close();
            myGarageCamera.pointsList.SetActive(false);
        }

        void Start()
        {
            Init(); 

            myGarageCamera.SetDetalSelector("Frontbumper");
            myGarageCamera.SetFocusPointThrough("Frontbumper"); 
        }



        public override bool RouterAction(UWinBaseClass window, string action)
        {
            if (!base.RouterAction(window, action)) return false; 
    
        
            if (action == "Top")
            {
                fastSound.Play("next");

                myGarageCamera.GetPointFromMoveVector(new Vector3(0, 0.25f, 0), 0.01f);

            }
            if (action == "Bottom")
            {
                fastSound.Play("next");
                myGarageCamera.GetPointFromMoveVector(new Vector3(0, -0.25f, 0), 0.01f);

            }
            if (action == "Left")
            {
                fastSound.Play("next");
                myGarageCamera.GetPointFromMoveVector(new Vector3(-3.4f, 0,0), 1f);

            }
            if (action == "Right")
            {
                fastSound.Play("next");
                //modalWindow.ModalBuild("Enter", "Чё как?", "Описание");
                myGarageCamera.GetPointFromMoveVector(new Vector3(2f, 0,0), 0.6f);

            }

            if (action == "Enter")
            { 
                //  print(MyGarageCamera.CurrentPoint.name);
                navigationWindowsNext.Open(myGarageCamera.currentPoint.name);
                Close();
            }

            if (action == "Exit")
            {
                //  print(MyGarageCamera.CurrentPoint.name);
                navigationWindowsBack.Open();
                Close();
            }

            return true;
        }
      

        void Update()
        {
      
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            if (move != Vector3.zero)
            {
                if (myGarageCamera.isMovedPlaying) return;
                // MyGarageCamera.GetPointFromMoveVector(move);
            }
        }
    }
}