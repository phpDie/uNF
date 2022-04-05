using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LuaApi;
using System.Threading;
 
namespace CarParserPlugin
{
     
#if UNITY_EDITOR
     


        [CustomEditor(typeof(CarParser))]
    public class CarParser_Editor : Editor
    {
         
        private  void OnEnable()
        {
           Debug.Log("CarParser_Editor enabled");
            CarParser_DataBase _bd = Resources.Load<CarParser_DataBase>("CarParser_DataBase");
            if (!_bd) return;

            CarParser current = (CarParser)target;
            current.carParser_DataBase = _bd;
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            CarParser current = (CarParser)target;
            if (GUILayout.Button("�������������� �����"))
            {

                
                if (PrefabUtility.GetPrefabParent(current.gameObject) != null)
                {
                    Debug.Log("[CarParser] ������ ������������ ���� ����� ������ ��� ����� �������� ��������, �� ����������...");
                    return;
                }

                current.FixMe();
            }
        }

    }
#endif
}