using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Pylon.Code.System
{
    public class GlobalVars
    {

        public const int MODE_BUILD = 0;
        public const int MODE_ATTACK = 1;

        public static int gameTickTime = 0;
        public static int mode = MODE_ATTACK;

        public static void load()
        {
            Prefabs.load();
        }

        public static void tick()
        {
            gameTickTime += 1;
        }

        public static bool countMod(int val) {
            return gameTickTime % val == 0;
        }

        public class Prefabs {

            public static ParticleSystem MaterialParticleSystem = null;


            public static void load() {
                Prefabs.MaterialParticleSystem = Resources.Load("Models/Particles/Particle System", typeof(ParticleSystem)) as ParticleSystem;
            }
        }
    }
}