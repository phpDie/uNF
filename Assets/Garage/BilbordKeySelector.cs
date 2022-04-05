using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BilbordKeySelector : MonoBehaviour
{

    public Image textDiv;
    public Text textText;
    public Image gear;
    public bool isEnabled = true;
    public string title;




    // Start is called before the first frame update
    void Start()
    {
        //Gear.rectTransform.LeanRotateZ(new Vector3(0, 0, 360), 1).loopType = LeanTweenType.
        gear.rectTransform.LeanRotateZ(90, 1f).setEaseLinear().setLoopCount(-1);

        textText.text = title;
        SetSelect(false);
    }


    public void SetTitle(string val)
    {
        title = val;
        textText.text = title;
    }


    public void SetSelect(bool val)
    {
       // if (isEnabled == val) return;
       // print(gameObject.name + " SetSelect " + val.ToString());
       // print(val);

        isEnabled = val;

        if (!isEnabled)
        {
            textDiv.gameObject.SetActive(false);
          //  gear.rectTransform.LeanAlpha(0, 0.325f).setEaseInOutSine();
           // textDiv.rectTransform.LeanAlpha(0.2f, 0.325f).setEaseInOutSine();
           // textText.rectTransform.LeanAlphaText(0, 0.325f).setEaseInOutSine(); 
        }
        else
        {
            textDiv.gameObject.SetActive(true);
          //  gear.rectTransform.LeanAlpha(1, 0.15f).setEaseInOutSine();
          //  textDiv.rectTransform.LeanAlpha(1, 0.15f).setEaseInOutSine();
            //textText.rectTransform.LeanAlphaText(1, 0.15f).setEaseInOutSine(); 
        }

    }

}
