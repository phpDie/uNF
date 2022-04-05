using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaApi;
using System.Threading;
 

namespace CarParserPlugin
{

    public class PartCarType
    {

        public string ind;
        public bool isCarbonRemove;
        public PartCarType(string _ind, bool _isCarbonRemove = true)
        {
            ind = _ind;
            isCarbonRemove = _isCarbonRemove;
        }
    }


    public class CarParser : MonoBehaviour
    {
        public void CarbonRemove(Transform obj)
        {
            foreach (Transform inchild in obj.GetChildren())
            {
                if (inchild.name.IndexOf("Carbon") >= 0)
                {
                    DestroyImmediate(inchild.gameObject);

                }
            }
        }

        private int numberSuka;

        public List<PartCarType> listPart = new List<PartCarType>();

        [HideInInspector] public Transform car;



        public float HealthPoint { get; set; }
        

        public CarParser_DataBase carParser_DataBase;
        

        public void Kill()
        {
            //Do something fun
        }

        void Start() {
            FixMe();
        }

        public void FixMe()
        {
 

            car = gameObject.transform;
            //listPart.Add(new PartCarType("Frontbumper"));
            listPart.Add(new PartCarType("Frontbumper"));
            listPart.Add(new PartCarType("Exhaust"));
            listPart.Add(new PartCarType("Diffusor", false)); 
           // listPart.Add(new PartCarType("Fender"));
            listPart.Add(new PartCarType("Hood"));
            listPart.Add(new PartCarType("Rearbumper"));
            listPart.Add(new PartCarType("Sideskirt"));
            listPart.Add(new PartCarType("Splitter", false)); 
            listPart.Add(new PartCarType("Spoiler", false));

            Transform tuneDir = new GameObject().transform;
            tuneDir.SetParent(car);
            tuneDir.name = "VISUAL_TUNE";



            foreach (Transform child in car.Find("Base").GetChildren())
            {
                if (child.name.IndexOf("Base_Windowwrap") > -1)
                {

                    DestroyImmediate(child.gameObject);
                    continue;
                }
                if (child.name.IndexOf("Base_Carbon") > -1)
                {
                    //Destroy(child.gameObject);
                    DestroyImmediate(child.gameObject);
                    continue;
                } 

            }



            foreach (Transform child in car.GetChildren())
            {
                if (child.name.IndexOf("Canard") > -1)
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }
                if (child.name.IndexOf("_Tiled") > -1)
                {
                    child.gameObject.SetActive(false);
                    //DestroyImmediate(child.gameObject);
                    continue;
                }
                if (child.name.IndexOf("Wheel_") > -1)
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }
                if (child.name.IndexOf("Licenceplate_") > -1)
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }

                    if (child.name.IndexOf("Fender_") > -1 && child.name.IndexOf("Stock") == -1)
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }
                    if (child.name.IndexOf("Brakelight_") > -1 && child.name.IndexOf("Stock") == -1)
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }
                if (child.name.IndexOf("Mirror_") > -1 && child.name.IndexOf("Stock") == -1)
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }
                if (child.name.IndexOf("Rearfender_") > -1 && child.name.IndexOf("Stock") == -1)
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }
                if (child.name.IndexOf("Frontlight") > -1 && child.name.IndexOf("Stock") == -1)
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }
                if (child.name.IndexOf("Trunk") > -1 && child.name.IndexOf("Stock") == -1)
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }
                //Frontlight
            }

            foreach (Transform child in car.GetDescendants())
            {
                if (child.name.IndexOf("Lightglass") > -1)
                {
                    DestroyImmediate(child.gameObject);
                    continue;
                }

            }
            foreach (Transform child in car.GetChildren())
            {
                if (child.name.IndexOf("Rearfender_") > -1) CarbonRemove(child);
                if (child.name.IndexOf("Mirror_") > -1) CarbonRemove(child);
                if (child.name.IndexOf("Trunk_") > -1) CarbonRemove(child);

            }

            for (int i = 0; i < car.childCount; i++)
            {
                Transform child = car.GetChild(i);
                child.name = child.name.Replace("*", "");
                child.name = child.name.Replace("\"", "");
                child.name = child.name.Replace("_", "_");
            }

            foreach (PartCarType part in listPart)
            {
                Transform catDir = new GameObject().transform;
                catDir.SetParent(tuneDir);
                catDir.name = part.ind;
                //catDir.gameObject.SetActive(tr);

                int counter = 0;




                bool isStockIsset = false;
                foreach (Transform child in car.GetChildren())
                {
                    if (child.name.IndexOf(part.ind) >= 0)
                    {
                        if (child.childCount == 0) continue;
                        if (part.isCarbonRemove) CarbonRemove(child);

                        
                        //print(child.name + " ++| " + child.name.IndexOf(part.ind).ToString());
                        child.SetParent(catDir);

                        if (child.name.IndexOf("_Stock") >= 0 && part.ind != "Spoiler")
                        {
                            child.gameObject.SetActive(true);
                            child.name = "0";
                            isStockIsset = true;
                        }
                        else {
                            counter += 1;
                            child.name = counter.ToString();
                            child.gameObject.SetActive(false);
                        }
                    }
                }

                if (!isStockIsset)
                {
                    Transform _stock = new GameObject().transform;
                    _stock.SetParent(catDir);
                    _stock.name = "0";
                }


            }


            ReplaceMaterials();


            //new Thread( ReplaceMaterials).Start();

            /*
            Thread t = new Thread(delegate ()
            {
                print("start");

                Thread.Sleep(1160);
                //Thread.d
            });
            t.Start();

        */




        }

        void ReplaceMaterials()
        {
            foreach (Transform child in car.GetDescendants())
            {



                if (!child.GetComponent<Renderer>()) continue;
                Renderer meshRenderer = child.GetComponent<Renderer>();

                //print(meshRenderer.materials.Length);
                if (!meshRenderer.sharedMaterial) continue;
                Material mat = meshRenderer.sharedMaterial;

                foreach (ReplacerMaterial _rep in carParser_DataBase.listMaterials)
                {
                    if (mat.name == _rep.name + " (Instance)" || mat.name == _rep.name )
                    {
                        meshRenderer.sharedMaterial = _rep.material;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}