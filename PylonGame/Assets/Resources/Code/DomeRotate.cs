using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeRotate : MonoBehaviour {

    public float rotateSpeed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.eulerAngles = new Vector3(
            0,
            this.transform.eulerAngles.y + rotateSpeed,
            0
        ); 
	}
}
