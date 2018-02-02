using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItem : MonoBehaviour {

    public Camera gameCamera = null;
    public GameObject holdObject = null;
    public bool active = false;
    public ParticleSystem part = null;

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

	// Use this for initialization
	void Start () {

        building_script = this.GetComponent<BuildingBehavior>();

        if (holdObject != null) {
            this.switchObject(holdObject);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(1) && this.swinging == false && this.swingIndex == 0)
        {
            this.swing();
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

            this.internalObject.transform.eulerAngles = new Vector3(this.gameCamera.transform.eulerAngles.x + this.forwardRotation + this.swingIndex, this.gameCamera.transform.eulerAngles.y + this.swingIndex,this.holdObject.transform.eulerAngles.z + this.swingIndex + (Random.Range(-10,10) * (this.swinging ? 1 : 0)));
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
        holdObject = o;
        internalObject = (Instantiate(this.holdObject,Vector3.forward,Quaternion.identity) as GameObject);
        this.internalObject.transform.localScale += new Vector3(-0.75f, -0.75f, -0.75f);
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
