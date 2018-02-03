using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour {

    private Rigidbody arrow_body = null;
    private MeshCollider collider = null;

    // Use this for initialization
    void Start () {
        this.arrow_body = this.GetComponent<Rigidbody>();
        this.collider = this.GetComponent<MeshCollider>();
	}
	
	// Update is called once per frame
	void Update () {
       
	}


    void OnCollisionEnter(Collision collision) {
        if (this.arrow_body != null && this.collider != null){
            this.arrow_body.isKinematic = true;
            transform.parent = collision.transform;
            this.transform.Translate(0.03f * -Vector3.forward);
            Destroy(this.collider);
        }
    }
}
