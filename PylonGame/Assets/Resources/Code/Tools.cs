using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Tools {

    public static int HIT_LOCATION_X = 0;
    public static int HIT_LOCATION_Y = 1;
    public static int HIT_LOCATION_Z = 2;
    public static int HIT_GAMEOBJECT = 3;
    public static int HIT_HAPPENED   = 4;

    public static System.Object[] getRayFromCursor(Camera c, float minDistance, float maxDist) {
        System.Object[] Objects = new System.Object[5];
        RaycastHit hit;
        Ray ray = c.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance <= maxDist && hit.distance >= minDistance)
            {
                Objects[Tools.HIT_GAMEOBJECT] = hit.transform.gameObject;
                Objects[Tools.HIT_LOCATION_X] = hit.point.x;
                Objects[Tools.HIT_LOCATION_Y] = hit.point.y;
                Objects[Tools.HIT_LOCATION_Z] = hit.point.z;
                Objects[Tools.HIT_HAPPENED] = true;
            }
            else{
                Objects[Tools.HIT_HAPPENED] = false;
            }
        }
        else {
            Objects[Tools.HIT_GAMEOBJECT] = null;
            Objects[Tools.HIT_LOCATION_X] = 0;
            Objects[Tools.HIT_LOCATION_Y] = 0;
            Objects[Tools.HIT_LOCATION_Z] = 0;
            Objects[Tools.HIT_HAPPENED] = false;
        }
        return Objects;
    }

    private static bool contains(GameObject[] src, GameObject target) {
        foreach (GameObject e in src) {
            if (e == target) {
                return true;
            }
        }
        return false;
    }
}