using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaApi;
using System.Threading;

namespace CarParserPlugin
{
    [System.Serializable]
    public struct ReplacerMaterial
    {
        public string name;
        public Material material;
    }

    [CreateAssetMenu(menuName = "NFS/Add db CarParser_DataBase", fileName = "CarParser_DataBase", order =500)]
    public class CarParser_DataBase : ScriptableObject
    {

        [SerializeField]
        public List<ReplacerMaterial> listMaterials = new List<ReplacerMaterial>();
         

        void addDefMat(string name)
        {
            ReplacerMaterial m = new ReplacerMaterial();
            m.name = name;
            listMaterials.Add(m);
        }

        public CarParser_DataBase()
        { 
            addDefMat("Body");
            addDefMat("Misc");
            addDefMat("Window");
        } 
    }
}