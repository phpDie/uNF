using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaApi;

namespace LuaApi
{
    public static class LuaApi
    {
        /// <summary>
        /// Получить список чилдов. 
        /// </summary>
        public static List<Transform> GetChildren(this Transform car)
        {
            List<Transform> _childList = new List<Transform>();
            for (int i = 0; i < car.childCount; i++)
            {
                _childList.Add(car.GetChild(i));

            }
            return _childList;
        }

        private static void AddDescendantsWithTag(Transform parent, List<Transform> list)
        {
            foreach (Transform child in parent)
            {
                list.Add(child);
                AddDescendantsWithTag(child, list);
            }
        }

        public static List<Transform> GetDescendants(this Transform car)
        {
            List<Transform> _childList = new List<Transform>();
            AddDescendantsWithTag(car, _childList);
            return _childList;
        }

        public static void SetTextIneerElement(this Transform parent, string ind, string val)
        {
            if (!parent.Find(ind)) return;

            parent.Find(ind).GetComponent<Text>().text = val;
        }
        
    }
}