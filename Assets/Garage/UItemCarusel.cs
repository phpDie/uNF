using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UItemCarusel : MonoBehaviour
{ 

    public void SetTitle(string name)
    {
        gameObject.transform.Find("_title").GetComponent<Text>().text = name;
    }

    public void SetPrice(int val)
    {
        string res = "На скаладе";
        if (val > 0) res = val.ToString() + " $";
        if (val < 0) res = "Не доступно!!!"; 
        gameObject.transform.Find("_price").GetComponent<Text>().text = res;
    }

    public void SetSprite(Sprite val)
    {
        if (!val)
        {
            gameObject.transform.Find("_img").gameObject.SetActive(false);
            return;
        }
        gameObject.transform.Find("_img").GetComponent<Image>().sprite = val;
    }

    public void SetCurent(bool val)
    {
      //  print("SetCurent " + val.ToString());
       gameObject.transform.Find("_curent").gameObject.SetActive(val);
    }


    public void SetBackgroundColor(Color32 val)
    { 
        gameObject.GetComponent<Image>().color = val;
    }

    bool isSelected = true;
    public void SetSelect(bool val)
    {
        
        if (isSelected == val) return;
        isSelected = val;

         


        RectTransform MyTra = gameObject.transform.Find("_select").GetComponent<RectTransform>();
         

        if (isSelected)
        { 
            
            MyTra.LeanAlpha(0.8f, 0.3f).setEaseInCubic();
            MyTra.LeanScale(new Vector3(1, 1, 1) * 0.8f, 0.26f).setEaseInOutQuint();
        }
        else
        {
            
            MyTra.LeanAlpha(0.0f, 0.15f);
           MyTra.LeanScale(new Vector3(1,1,1) * 0.5f, 0.08f).setEaseInOutQuint();

        }
    }

    void Start()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.localScale = new Vector3(1, 1, 1) * 1f;
        //  SetCurent(false);
    }
}
