using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloatScript : MonoBehaviour {

    public float floatPoint = 32.7f;
    public float bouyancy = 10;


    private bool fog = false;
    private Color fogColor = new Color(0, 0.4f, 0.7f, 0.6f);
    private float fogDensity = 0.04f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.y < this.floatPoint)
        {
            Debug.Log("Floating....");
            this.GetComponent<Rigidbody>().AddForce(this.transform.up * this.bouyancy);

        }

        if (this.transform.position.y < this.floatPoint - 1)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = this.fogColor;
            RenderSettings.fogDensity = this.fogDensity;
        }
        else {
            RenderSettings.fog = false;
        }
	}
}
