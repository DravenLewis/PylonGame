using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Pylon.Code.Mechanics;
using Pylon.Code.System;

namespace Pylon.Code.Mechanics.Building
{
    public class BlockBuild : MonoBehaviour
    {

        private Component PlayerController = null;

        public bool Building = false;
        public int CURRENT_BUILD_MODE = 0;

        public void Start() {
            this.PlayerController = GetComponent<PlayerControler>();
        }

        public void Update() {
            this.CURRENT_BUILD_MODE = GlobalVars.mode;
        }


        // allow the player controller to controll the building
        public void OnBuild() {

            Debug.Log("On Build Called");

            if (this.PlayerController != null) {
                if (Input.GetMouseButtonDown(0)) { // left
                    if (this.Building == false)
                    {
                        this.Building = true;
                    }
                    else if (this.Building == true) {
                        this.PlaceMaterial((this.PlayerController as PlayerControler).GetBuildingMaterial(),Camera.main,0,20);
                        this.Building = false;
                    }
                }

                if (Input.GetMouseButtonDown(1)) { // right
                    if (this.Building == true) {
                        this.Building = false;
                    }
                }
            }
        }

        public void PlaceMaterial(PlayerControler.PlayerGameObject o, Camera c, int minDistance, int maxDistance) {
            object[] hit = Tools.getRayFromCursor(c,minDistance,maxDistance);
            if ((bool)hit[Tools.HIT_HAPPENED] == true) {
                GameObject colide = (GameObject) hit[Tools.HIT_GAMEOBJECT];
                Component ScriptBuildControl = GetComponent<BuildControl>();
                if (ScriptBuildControl != null) {
                    // this object is a buildable object
                    Vector3 placeVector = Vector3.zero;
                    switch ((ScriptBuildControl as BuildControl).CurrentSide) {
                        case BuildControl.CubeSide.BLOCK_SIDE_X:
                            placeVector = colide.transform.right * 1.25f;
                            break;
                        case BuildControl.CubeSide.BLOCK_SIDE_Y:
                            placeVector = colide.transform.up * 1.25f;
                            break;
                        case BuildControl.CubeSide.BLOCK_SIDE_Z:
                            placeVector = colide.transform.forward * 1.25f;
                            break;
                        case BuildControl.CubeSide.BLOCK_SIDE_NEGATIVE_X:
                            placeVector = colide.transform.right * -1.25f;
                            break;
                        case BuildControl.CubeSide.BLOCK_SIDE_NEGATIVE_Y:
                            placeVector = colide.transform.up * -1.25f;
                            break;
                        case BuildControl.CubeSide.BLOCK_SIDE_NEGATIVE_Z:
                            placeVector = colide.transform.forward * -1.25f;
                            break;
                    }

                    Instantiate(o.Get(),placeVector,colide.transform.rotation);
                }
                else
                {
                    // this means we hit a normal mesh and not a buildable object
                    Instantiate(o.Get(), new Vector3((float)hit[Tools.HIT_LOCATION_X], (float)hit[Tools.HIT_LOCATION_Y], (float)hit[Tools.HIT_LOCATION_Z]), c.transform.rotation);
                }
            }
        }
    }
}
