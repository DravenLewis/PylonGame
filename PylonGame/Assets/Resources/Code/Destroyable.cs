using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

    public float hp = 2000;
    public float resist = 1;

	// Use this for initialization
	void Start () {
        Debug.Log("I Am This Type: " + this);
        
	}
	
	// Update is called once per frame
	void Update () {
        if (this.hp <= 0) {
            Destroy(this.transform.parent.gameObject);
        }
	}

    public void repair(int hp) {
        this.hp += hp;
    }

    public void hit(int hp) {
        this.hp -= hp / resist;
    }
}
