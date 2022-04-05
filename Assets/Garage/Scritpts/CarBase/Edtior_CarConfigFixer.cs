using UnityEditor;
using UnityEngine;
using CarTunr; 
using UnityEngine.UIElements;
using PowerParametrs;

namespace Garage.Plugins
{
#if UNITY_EDITOR
    [CustomEditor(typeof(CarConfigBase))]
    public class Edtior_CarConfigFixer : Editor
    {
        CarConfigBase сarConfigBase;

        public override void OnInspectorGUI()
        {
           // Debug.Log("OnInspectorGUI");
            DrawDefaultInspector();
            
            if(GUILayout.Button("Дефалтнутся"))
            {

            }
        }
 
        void FixBase()
        {
            for (int i=0;i< сarConfigBase.indEngineTune_DatabaseClass.engineAttribute.Count; i++)
            {
                BaseEngineAttributeStruct the = сarConfigBase.indEngineTune_DatabaseClass.engineAttribute[i];

                
                if (!сarConfigBase.engineAttribute.Exists(item => item.ind == the.ind))
                {
                    EngineBaseAttibute power = new EngineBaseAttibute();
                    power.ind = the.ind;
                    power._editorName =   the.name;
                    power.val = the.defVal;
                    сarConfigBase.engineAttribute.Add(power); 
                }

              //  int J = carConfigBase.engineAttribute.FindIndex(item => item.ind == the.ind);
              //  EngineBaseAttibute _power = carConfigBase.engineAttribute[J];
                 

              //  carConfigBase.engineAttribute[J]._editorName= the.name + "  " + _power.val.ToString() + " " + the.ed;
                //Debug.Log(_power._editorName);

            }
//             
        }

        void OnEnable()
        {
            сarConfigBase = (CarConfigBase)target;
            FixBase();
           // Debug.Log("x");
        }

    }
#endif
}
