using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Code.System;

public class BuildingBehavior : MonoBehaviour {

    public GameObject material_wall = null;
    public float maxRange = 10;
    public float minRange = 1;

    public Camera PlayerCamera = null;
    public bool isActive = false;
    public bool disabled = false;

    private GameObject o = null;
    public int MaxCoolDown = 100;
 
    private float scrollWheel = 0;

    // place mode is LEFT TRIGGER or LEFT PRESS
    // MOUSE SCROLL IS THE ANGLE

    // Use this for initialization
    void Start () {
        // Instantiate(this.material_wall, PlayerCamera.transform.position, new Quaternion(-90, 0, 0, 0));
        o = (Instantiate(this.material_wall, new Vector3(0, 0, 0), new Quaternion()) as GameObject);
        o.layer = 2;
        o.transform.Find("default").transform.gameObject.layer = 2; // pull the lower to the same
        o.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

        if (GlobalVars.mode == GlobalVars.MODE_ATTACK) {
            this.setEnabled(false);
        }


        if (Input.GetMouseButton(0) && !this.isActive && GlobalVars.countMod(this.MaxCoolDown) && this.disabled == false) {
            this.isActive = true;
            o.SetActive(true);
            this.scrollWheel = 0;
        }

        if (Input.GetMouseButton(1) && this.isActive && GlobalVars.countMod(this.MaxCoolDown) && this.disabled == false)
        {
            this.isActive = false;
            o.SetActive(false);
            this.scrollWheel = 0;
        }

        if (Input.GetMouseButton(2)) {
            this.scrollWheel += Input.mouseScrollDelta.y;
        }

        if (this.isActive && this.PlayerCamera != null)
        {
            System.Object[] hit = Tools.getRayFromCursor(PlayerCamera, minRange,maxRange);

            if ((bool)(hit[Tools.HIT_HAPPENED]) == true)
            {
                if (o != null)
                {
                    if (o.activeSelf != true)
                    {
                        o.SetActive(true);
                    }

                    Vector3 hitLocation = new Vector3();
                    hitLocation.x = (float)(hit[Tools.HIT_LOCATION_X]);
                    hitLocation.y = (float)(hit[Tools.HIT_LOCATION_Y]) + 2.5f;
                    hitLocation.z = (float)(hit[Tools.HIT_LOCATION_Z]);


                    
                    o.transform.position = hitLocation;
                    o.transform.eulerAngles = new Vector3(-90,this.PlayerCamera.transform.eulerAngles.y + this.scrollWheel, 0);

                    if (Input.GetMouseButton(0) && GlobalVars.countMod(this.MaxCoolDown)) {
                        // create wall
                        GameObject b = (Instantiate(this.material_wall,Vector3.zero,Quaternion.identity) as GameObject);
                        b.SetActive(false);
                        b.transform.position = hitLocation;
                        b.transform.eulerAngles = new Vector3(-90, this.PlayerCamera.transform.eulerAngles.y + this.scrollWheel, 0);
                        b.SetActive(true);
                        // set active false
                        o.SetActive(false);
                        this.isActive = false;
                        this.scrollWheel = 0;
                        // exit the function
                        return;
                    }
                }
            }
            else {
                if (o != null) {
                    o.SetActive(false);
                }
            }
        }
    }

    public void setEnabled(bool enabled) {
        this.disabled = !enabled;
        o.SetActive(false);
        this.isActive = false;
        this.scrollWheel = 0;
    }
}
