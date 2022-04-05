using PowerParametrs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XrRenderWin : MonoBehaviour
{
    [Header("База данных")]
    public Project_DatabaseClass projectDatabaseClass;
    public GameObject Element;

    struct XrElementData
    {
        public GameObject Element;
        public BaseEngineAttributeStruct attr;
        public Text valText;
        public Text titleText;
        public Image spinIcon;
    }

    Dictionary<BaseAttribute, XrElementData> myAttribute = new Dictionary<BaseAttribute, XrElementData>();

    Vector2 origSize;
    RectTransform myPanel;

    void Init()
    {
        myPanel = gameObject.GetComponent<RectTransform>();
        origSize = myPanel.sizeDelta;

        for (int i = 0; i < projectDatabaseClass.indEngineTune.engineAttribute.Count; i++)
        {
            BaseEngineAttributeStruct the = projectDatabaseClass.indEngineTune.engineAttribute[i];

            XrElementData xrElementData = new XrElementData();

            GameObject E = Instantiate(Element, gameObject.transform);
            E.name = the.ind.ToString();

            xrElementData.titleText = E.transform.Find("_title").GetComponent<Text>();
            xrElementData.titleText.text = the.name;

            xrElementData.attr = the;
            xrElementData.Element = E;
            xrElementData.valText = E.transform.Find("_val").GetComponent<Text>();
            xrElementData.spinIcon = E.transform.Find("_icon").GetComponent<Image>();

            xrElementData.valText.text = "0 " + the.ed;


            myAttribute.Add(the.ind, xrElementData);
        }

        Element.SetActive(false);
    }
    void Start()
    {
        if (!myPanel) Init();
    }

    
    void ResetSpinColored()
    {
        foreach (var item in myAttribute)
        {
            item.Value.spinIcon.gameObject.SetActive(false);
            item.Value.valText.color = new Color32(255,255,255,240);
            item.Value.spinIcon.rectTransform.LeanScale(new Vector3(1, 1, 1)*0.00f, 0.15f).setEaseInElastic();
        }
    }

    public void RenderData(Dictionary<BaseAttribute, float> valList)
    {
        if (!myPanel) Init();

        ResetSpinColored();
        Show();
        foreach (var item in valList)
        {
//            print(item.Key);
            if (!myAttribute.ContainsKey(item.Key))
            {
                continue;
            }
            XrElementData the = myAttribute[item.Key];
            float _val = item.Value;
            _val = Mathf.Round(_val / the.attr.ceilTo) * the.attr.ceilTo;
            the.valText.text = _val + " " + the.attr.ed;
            the.spinIcon.gameObject.SetActive(false);
        }
    }

    public Color32 colorBad;
    public Color32 colorGood;

    public void RenderUpdgradeData(MathToEngineResponseStruct data, bool showDeltaResult = false )
    {
        
        RenderData(data.powerUpgrade);
        /*
        print("\n mcur:");
        foreach (var item in data.levels)
        {
            print(item.Key + " = " + item.Value.ToString());
        }
        */

        if (!showDeltaResult)
        {
            return;
        }

            foreach (var item in data.isUpgrade)
        {
          

            XrElementData the = myAttribute[item.Key]; 
          //  the.titleText.text = the.attr.name + " | " + data.levels[item.Key.ToString()];


            if (item.Value == MathToEngineEqualsStruct.none)
            {
                the.spinIcon.gameObject.SetActive(false);
                continue;
            }

            the.spinIcon.gameObject.SetActive(true);
            the.spinIcon.rectTransform.LeanScale(new Vector3(1, 1, 1), 0.12f).setEaseInElastic();

            if (item.Value == MathToEngineEqualsStruct.upgrade)
            {
                the.valText.color = colorGood;
                the.spinIcon.color = colorGood;
                the.spinIcon.rectTransform.LeanRotateZ(0, 0.15f).setEaseInElastic(); 
            }
            else
            {
                the.valText.color = colorBad;
                the.spinIcon.color = colorBad;
                the.spinIcon.rectTransform.LeanRotateZ(180, 0.15f).setEaseInElastic();
                
                //the.spinIcon.rectTransform.LeanScale(180, 0.15f);
            }
             
        }
        // foreach (in myAttribute)
    }

    public void ResizeHorizontal(bool val)
    {
        //myPanel.LeanScale
       // print(origSize);
       // print(origSize * new Vector2(0.5f, 1));
        if (val)
        {
            myPanel.LeanScale(new Vector3(1f, 1,1)*0.7f, 0.16f).setEaseInSine();
        }
        else {
            myPanel.LeanScale( new Vector3(1f, 1,1), 0.12f).setEaseInSine();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
