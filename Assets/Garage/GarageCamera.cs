using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Cinemachine;
using LinkerGarage;

namespace Garage
{
    public class GarageCamera : MonoBehaviour
    {
        public Transform carsListSpawn;
        public Tune_DatabaseClass myTuneDatabaseClass;
      public GarageController garageController;

        [HideInInspector]
        public GameObject car
        {
            get
            {
                return garageController.car.gameObject;
            }
        }

        public GameObject pointsList;
        public Camera myCamera;
        [FormerlySerializedAs("MyVirtualCamera")] public CinemachineVirtualCamera myVirtualCamera;
        [FormerlySerializedAs("MyCinemachineFollowZoom")] public CinemachineFollowZoom myCinemachineFollowZoom;
        [FormerlySerializedAs("MyGmFocus")] public GameObject myGmFocus;

        public Transform floorGarage;

        public CinemachineVirtualCamera cinemachineCameraCarsList;
        [FormerlySerializedAs("CurrentPoint")] public Transform currentPoint;

        public bool isMovedPlaying = false;

        float timer = 0;

        void Init()
        {
            garageController = Linker.GetGarageController();
         
             
            myGmFocus = new GameObject();
            myGmFocus.transform.position = car.transform.position + new Vector3(0, 2, 3);

            for (var i = 0; i < myTuneDatabaseClass.detalList.Count; i++)
            {
                VisualDetal _the = myTuneDatabaseClass.detalList[i];
                if (!pointsList.transform.Find(_the.ind))
                {
                    print("Не найден Точка инд детали " + myTuneDatabaseClass.detalList[i].ind);
                    continue;
                }

                GameObject go = pointsList.transform.Find(_the.ind).gameObject;
              //  print(go + " = " + _the.name);
                go.GetComponent<BilbordKeySelector>().SetTitle(_the.name);

            }
        }

        void Start()
        {
            if (!myGmFocus) Init();
        }

        bool isFull = false;
        public Transform GetPointCamera(string ind)
        {
            Transform isset = pointsList.transform.Find("Full");
            if (pointsList.transform.Find(ind))
            {
                isset = pointsList.transform.Find(ind);
            }

            return isset;
        }



        public Transform GetNearestDetal(Vector3 currentPosition, Transform ignored)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity; 

            for (var i = 0; i < pointsList.transform.childCount; i++)
            {
            
                Transform potentialTarget = pointsList.transform.GetChild(i);
                if(potentialTarget== ignored)
                {
                    continue;
                }

                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }

            return bestTarget;
        }

        public void SetDetalSelector(string ind)
        {
            for(var i=0;i< pointsList.transform.childCount; i++)
            {
                GameObject go = pointsList.transform.GetChild(i).gameObject;
                if (go.name == ind)
                {
                    go.GetComponent<BilbordKeySelector>().SetSelect(true);
                }
                else
                {
                    go.GetComponent<BilbordKeySelector>().SetSelect(false);
                }
            }

        }

        public void ShowListCar(int id)
        {
            Transform  _to = carsListSpawn.Find("SpawOtherCar").GetChild(id);

            cinemachineCameraCarsList.m_LookAt = _to;
            cinemachineCameraCarsList.m_Follow = carsListSpawn.Find("CamPoint");
            cinemachineCameraCarsList.gameObject.SetActive(true);
            
            
        }

        public Transform SetFocusPointThrough(string ind)
        {

            cinemachineCameraCarsList.gameObject.SetActive(false);

            if (!myGmFocus) Init();
            //print("SetFocusPoint: " + Ind);
            Transform isset = GetPointCamera(ind);

            Vector3 offsetFromCar = new Vector3(0, -1, 0);

            Vector3 lookVector =  (car.transform.position + offsetFromCar  - isset.position).normalized;


            Vector3 posTo = car.transform.position + offsetFromCar + lookVector * -4.1f;


            float h = 0.5f;
             //h = -0.16f;
            if (posTo.y < floorGarage.position.y - h)
            {
                // print("Down fix");
                posTo += new Vector3(0, Mathf.Abs(posTo.y -floorGarage.position.y - h   )  , 0);
            }
            /*
        float H = 0.76f;
        if (posTo.y < Car.transform.position.y- H)
        {
            print("Down fix");
            print(Mathf.Abs(posTo.y - Car.transform.position.y - H));

            posTo += new Vector3(0, Mathf.Abs(posTo.y - Car.transform.position.y - H) + 0.1f, 0);
        }
        */
            //MyGmFocus.transform.position = Car.transform.position + LookVector * -5;
            myGmFocus.transform.LeanMove(posTo, 0.25f);
            // MyVirtualCamera.m_LookAt = MyGmFocus.transform;
            // MyVirtualCamera.m_Follow = isset;

            myVirtualCamera.m_LookAt = isset;
            myVirtualCamera.m_Follow = myGmFocus.transform;

            float f = Random.Range(1.7f, 2.9f)*1.5f;
            myCinemachineFollowZoom.m_Width = f;

            //isMovedPlaying = true;
            currentPoint = isset;
            return isset;
        }

        public void SetFocusPoint(string ind)
        {
            print("SetFocusPoint: " + ind);
            Transform isset = GetPointCamera(ind);
         
            myGmFocus.transform.position = isset.position + isset.forward * 7;
            myVirtualCamera.m_LookAt = myGmFocus.transform;
            myVirtualCamera.m_Follow = isset; 
        }



        public void GetPointFromMoveVector(Vector3 move, float forwardMixed)
        {
            
            move = myCamera.transform.TransformDirection( new Vector3(move.x,0, move.z))+ new Vector3(0, move.y, 0);

            Vector3 _forwMix = (myCamera.transform.forward.normalized * forwardMixed);
            move = move + new Vector3(_forwMix.x,0,_forwMix.z);

            Vector3 resultPos = currentPoint.position + move;

           // print(Vector3.Distance(currentPoint.position, resultPos ));
            Debug.DrawRay(currentPoint.position, resultPos- currentPoint.position, Color.red, 3.2f);

            Transform point = GetNearestDetal(resultPos, currentPoint);
            if (!point)
            {
                print("Error search point");
                return;
            }

            SetDetalSelector(point.name);
            currentPoint = SetFocusPointThrough(point.name);
        }
    

    }
}
