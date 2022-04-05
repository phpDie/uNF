using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LinkerGarage
{
   
    class Linker
    {
        public static GarageController GetGarageController()
        {
            return GameObject.Find("GarageController").GetComponent<GarageController>();
        } 

    }
}
