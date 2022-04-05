using LuaApi;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Garage.Plugins
{
   // [ExecuteInEditMode()]
    public class VisualTuneHelperSelection : MonoBehaviour
    {
    
        public int colorId = 0;
        public Dictionary<string, int> DataVisual = new Dictionary<string, int>();
         
        [Header("Конфиг проекта")]
        public Project_DatabaseClass projectDatabaseClass;
        Tune_DatabaseClass myTuneDatabaseClass;

        public Transform GetCategoryFoolder(string ind)
        {
            if (!IssetTuneKey(ind))
            {
                print("Error tune ind");
                print(ind);
                return null;
            }
            Transform cat =  gameObject.transform.Find(ind);

            if (!cat) return null;

            return cat;
        }
     

        public GameObject GetTuneGameObject_FromInd(string ind)
        {
            if (!IssetTuneKey(ind))
            {
                print("Error tune ind");
                return null;
            }
            return gameObject.transform.Find(ind).gameObject;

        }

        public int GetTuneIndCount(string ind)
        {
            if (!IssetTuneKey(ind))
            {
                print("Error tune ind");
                return 0;
            }
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                Transform category = gameObject.transform.GetChild(i);
                if (category.name != ind) continue;
                return category.transform.childCount;
            }
            return 0;
        }

        public void SetColor(int id)
        {
            
            
           // print("set color to " + id.ToString());
            foreach (Transform child in gameObject.transform.GetDescendants())
            {

                if (!child.GetComponent<Renderer>()) continue;
                Renderer meshRenderer = child.GetComponent<Renderer>();

                if (!meshRenderer.sharedMaterial) continue;
                Material mat = meshRenderer.sharedMaterial;
                string _rep = "CarBody";
                if (mat.name != _rep + " (Instance)" && mat.name != _rep) continue;

                meshRenderer.sharedMaterial.color = projectDatabaseClass.tuneDatabaseClass.colorList[id];

            }

         
        }

        public bool SetTuneIndTo(string ind, int id)
        {
            if (!IssetTuneKey(ind))
            {
                print("Error tune ind");
                return false;
            }

            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                Transform category = gameObject.transform.GetChild(i);
                if (category.name != ind) continue;

                for (var j = 0; j < category.childCount; j++)
                {
                    Transform item = category.GetChild(j);
                    item.gameObject.SetActive(item.name == id.ToString()); 
                }

            }

            return true;
        }


        public void ResetToDefultTune()
        { 
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                Transform category = gameObject.transform.GetChild(i);

                for (var j = 0; j < category.childCount; j++)
                {
                    Transform item = category.GetChild(j);
                    item.gameObject.SetActive(item.name == "0");
                }

            }
        }


        GameObject lastObj;
        void Seclecter(GameObject obj)
        {
            if (lastObj == obj) return;
            lastObj = obj;

            print("[VisualTuneHelperSelection] Помогает с редактором!");

            for (var i = 0; i < obj.transform.parent.childCount; i++)
            {
                Transform the = obj.transform.parent.GetChild(i);
                the.gameObject.SetActive(the == obj.transform);

            } 
        }


        public bool IssetTuneKey(string ind)
        {
            if (!myTuneDatabaseClass)
            {
                initConfigs();
                
                
            }

         
            for (var i = 0; i < myTuneDatabaseClass.detalList.Count; i++)
            {
                if (myTuneDatabaseClass.detalList[i].ind == ind)
                {
                    return true;
                }
            }
            return false;
        }

        void UnityLogic()
        {
#if UNITY_EDITOR 
            
            if (EditorApplication.isPlaying) return;
            Debug.Log("test0.1");
            if (!myTuneDatabaseClass) return;
       
            GameObject obj = Selection.activeGameObject;
            Debug.Log("test1");
            if (!obj) return;
            if (!obj.transform.IsChildOf(gameObject.transform)) return;

            if (obj.transform.parent == gameObject.transform) return;
            if (!IssetTuneKey(obj.transform.parent.name)) return;
            Debug.Log("test2");
            Seclecter(obj);
#endif
        }



        public void ResetTune_ToData()
        {
            SetColor(colorId);
            foreach (var item in DataVisual) {
                SetTuneIndTo(item.Key, item.Value);
            }
        }


        public void ResetTune_ToNull()
        {
            foreach (VisualDetal item in myTuneDatabaseClass.detalList)
            {
                DataVisual[item.ind] = 0;
                SetTuneIndTo(item.ind, 0);
            }
        }

        bool _isInit = false;
        public void initConfigs()
        {

           

            
            if (_isInit) return;
            _isInit = true;

                myTuneDatabaseClass = projectDatabaseClass.tuneDatabaseClass;

            if (DataVisual.Count == 0)
            {
                ResetTune_ToNull();
                //foreach (var item in DataVisual) { }
            }

        }

        private void Start()
        {
            initConfigs();

        }

#if UNITY_EDITOR
        void Update()
        {
            if (!myTuneDatabaseClass)
            {
                initConfigs();
            }
            UnityLogic();

        }

    
#endif
        //  UnityLogic();


    }
}
