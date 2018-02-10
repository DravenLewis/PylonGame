using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pylon.Code.Mechanics.Building
{
    public class BuildControl : MonoBehaviour
    {
        public enum CubeSide{
            BLOCK_SIDE_X = 0,
            BLOCK_SIDE_Y = 1,
            BLOCK_SIDE_Z = 2,
            BLOCK_SIDE_NEGATIVE_X = 3,
            BLOCK_SIDE_NEGATIVE_Y = 4,
            BLOCK_SIDE_NEGATIVE_Z = 5
        }

        public CubeSide CurrentSide = CubeSide.BLOCK_SIDE_Z;
    }
}
