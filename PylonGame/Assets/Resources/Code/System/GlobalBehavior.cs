using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace Pylon.Code.System
{
    class GlobalBehavior : MonoBehaviour
    {
        public void Start()
        {
            GlobalVars.load();
        }

        public void Update()
        {
            GlobalVars.tick();
        }
    }
}

