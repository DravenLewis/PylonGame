using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Code.System;

public class HoldItem : MonoBehaviour {
    public GameObject spawnLocation = null;

    public Camera gameCamera = null;
    public GameObject holdObject = null;
    public bool active = false;
    public ParticleSystem part = null;

    private GameObject[] objects = new GameObject[4];
    public int currentObject = 3;
    //################################################
    public float depthOffset = 0;
    public float scrollOffset = 0;
    public float heightOffset = 0;
    

    public float forwardRotation = 0;
    public float maxSwing = 20;
    public float swingSpeed = 1;
    //################################################


    private GameObject internalObject = null;
    private float swingIndex = 0;
    private bool swinging = false;

    private Component building_script = null;

    private int shootTime = 0;
    private bool shoot = false;

    // Use this for initialization
    void Start() {
        // init the array
        this.objects[0] = (Resources.Load("Models/CrossBow/p_CrossBowNext",typeof(GameObject)) as GameObject);
        this.objects[1] = (Resources.Load("Models/CrossBow/p_CrossBowBoltNF",typeof(GameObject)) as GameObject);
        this.objects[2] = (Resources.Load("Models/Sword/Sword",typeof(GameObject)) as GameObject);
        this.objects[3] = (Resources.Load("Models/CrossBow/p_CrossBowLoaded", typeof(GameObject)) as GameObject);


        //this.objects[0] = (Resources.Load("CrossBowNext",) as GameObject);
        //this.objects[1] = (Resources.Load("CrossBowBolt") as GameObject);
        //this.objects[2] = (Resources.Load("Sword") as GameObject);
        // ===================================================================



        building_script = this.GetComponent<BuildingBehavior>();

        this.switchObject(this.objects[this.currentObject]);


	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("tab") && GlobalVars.countMod(10)) {

            if (shoot == true) {
                shoot = false;
                shootTime = 0;
            }

            this.currentObject++;
            if (this.currentObject > this.objects.Length - 1){
                this.currentObject = 0;
            }
            this.switchObject(this.objects[this.currentObject]);
        }

        // DONE TO HERE =========================================


        if (Input.GetMouseButton(0) && GlobalVars.countMod(5) && (this.currentObject == 3 || this.currentObject == 0)){
            if (this.shootTime == 0)
            {
                Transform arrow_transform = this.spawnLocation.transform;


                // ===================================================================================
                GameObject arrow = Instantiate(
                    this.objects[1],
                    arrow_transform.position,
                    arrow_transform.rotation
                );

                // ===================================================================================
                Rigidbody arrow_rb = arrow.GetComponent<Rigidbody>();
                if (arrow_rb != null)
                {
                    arrow_rb.MoveRotation(arrow_transform.rotation);
                    arrow_rb.MovePosition(arrow_transform.position);
                    //arrow_rb.velocity = gameCamera.transform.forward * 1000;
                    arrow_rb.AddForce(gameCamera.transform.forward * 1000);
                }

                if (GlobalVars.countMod(20))
                {
                    this.switchObject(this.objects[3]);
                }

                shootTime = 100;
            }
            
        }

        if (Input.GetMouseButton(1) && this.swinging == false && this.swingIndex == 0)
        {
            this.swing();
        }

        if (this.shootTime <= 0 && shoot == true)
        {
            this.switchObject(this.objects[3]);
            if (this.shootTime < 0) this.shootTime = 0;
            shoot = false;
        }
        else {
            if (shoot = true)
            {
                this.switchObject(this.objects[0]);
            }
        }

        if (this.shootTime > 0) {
            shootTime--;
        }

        if (this.active == true) {

            if (this.swinging)
            {
                if (swingIndex < maxSwing)
                {
                    swingIndex += this.swingSpeed;
                }
                else if(swingIndex >= maxSwing)
                {
                    this.swinging = false;
                    if (this.building_script != null)
                    {
                        (this.building_script as BuildingBehavior).setEnabled(true);
                    }
                    this.damage(5);
                }
            }
            else {
                if (swingIndex > 0) {
                    swingIndex -= this.swingSpeed;
                }

                if (swingIndex < 0) {
                    swingIndex = 0;
                }
            }

            if (!this.internalObject.activeSelf) {
                this.internalObject.SetActive(true);
            }

            // set the location... Infront of the camera (if this works then we should see the sword)
            this.internalObject.transform.position = this.gameCamera.transform.position +
                (this.gameCamera.transform.forward * this.depthOffset) +  // local Z
                (this.gameCamera.transform.right * this.scrollOffset) + // local X
                (this.gameCamera.transform.up * this.heightOffset); // local Y

            this.internalObject.transform.eulerAngles = new Vector3(
                this.gameCamera.transform.eulerAngles.x +
                this.forwardRotation +
                this.swingIndex,

                this.gameCamera.transform.eulerAngles.y +
                this.swingIndex,

                this.holdObject.transform.eulerAngles.z +
                this.swingIndex +
                (Random.Range(-10, 10) * (this.swinging ? 1 : 0))
            );

            //Vector3 relativeUp = gameCamera.transform.TransformDirection(Vector3.up);
            //Vector3 relativeRight = gameCamera.transform.TransformDirection(Vector3.right);
            //Vector3 objectRelativeUp = this.internalObject.transform.InverseTransformDirection(relativeUp);
            //Vector3 objectRelativeRight = this.internalObject.transform.InverseTransformDirection(relativeRight);

            //Quaternion rotateBy =  Quaternion.AngleAxis(this.holdObject.transform.rotation.x / gameObject.transform.localScale.x, objectRelativeUp) * 
            //Quaternion.AngleAxis(-this.holdObject.transform.rotation.y / gameObject.transform.localScale.x, objectRelativeRight);

            this.internalObject.transform.RotateAroundLocal(this.internalObject.transform.forward,this.holdObject.transform.eulerAngles.z);
            this.internalObject.transform.RotateAroundLocal(this.internalObject.transform.right, this.holdObject.transform.eulerAngles.x);
            this.internalObject.transform.RotateAroundLocal(this.internalObject.transform.up, this.holdObject.transform.eulerAngles.y);

            Debug.Log("Hold Object Model Z = " + this.holdObject.transform.eulerAngles.z);
        }
	}

    public void toggleActive() {
        this.active = !this.active;
    }

    public void swing() {
        if (this.building_script != null) {
            (this.building_script as BuildingBehavior).setEnabled(false);
        }
        this.swinging = true;
    }

    public void switchObject(GameObject o) {
        if (internalObject != null) {
            Destroy(internalObject);
        }
        holdObject = o;
        internalObject = (Instantiate(this.holdObject,Vector3.forward,Quaternion.identity) as GameObject);
        //this.internalObject.transform.localScale += new Vector3(-0.75f, -0.75f, -0.75f);
        // internalObject.SetActive(false);
    }

    public void damage(int hp) {
        Camera current = Camera.main;
        Debug.Log("Current Camera: " + current);
        System.Object[] hit = Tools.getRayFromCursor(current,0,10);
        if ((bool)hit[Tools.HIT_HAPPENED] == true) {
            GameObject o = (hit[Tools.HIT_GAMEOBJECT] as GameObject);
            Debug.Log("Hit! : " + o.name);


            // maybe put this in the destory area
            ParticleSystem particleSystem = this.part;
            Debug.Log("LOADING PARTICLES");
            if (particleSystem != null) {
                ParticleSystem s = (ParticleSystem) Instantiate(particleSystem,new Vector3((float) hit[Tools.HIT_LOCATION_X],(float) hit[Tools.HIT_LOCATION_Y],(float) hit[Tools.HIT_LOCATION_Z]),Quaternion.identity);
                s.GetComponent<Renderer>().material = (hit[Tools.HIT_GAMEOBJECT] as GameObject).GetComponent<Renderer>().material;
                Debug.Log("Created");
            }

            Component destroy_script = destroy_script = o.GetComponent<Destroyable>();
            if (destroy_script != null) {
                Debug.Log("is Destroyable");
                (destroy_script as Destroyable).hit(hp);
                Debug.Log("did: " + hp + " damage");
            }
        }
    }
}
