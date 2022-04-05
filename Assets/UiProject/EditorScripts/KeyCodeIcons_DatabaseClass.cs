
using System.Collections.Generic;
using UnityEngine;


 
[System.Serializable]
public class KeyIcon
{ 

    [Header("Кнопка")]
    public KeyCode keyCode;  
     

    [Header("Иконка")]
    public Sprite icon;  
     
         
}

[CreateAssetMenu(fileName = "New keyCode Icons")]
public class KeyCodeIcons_DatabaseClass : ScriptableObject
{
      

    [Header("Иконки")]
    public List<KeyIcon> listVarz = new List<KeyIcon>();
   
    public Sprite GetIconFromKeyCode(KeyCode key)
    {
        for (var i=0; i< listVarz.Count; i++)
        {
            if (listVarz[i].keyCode == key)
            {
                return listVarz[i].icon;
            }
        }

        return null;
    }

}
