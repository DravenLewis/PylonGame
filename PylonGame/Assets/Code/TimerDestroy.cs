using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour {

    public int initialTime = 100;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (initialTime > 0) {
            initialTime--;
        }
        if (initialTime <= 0) {
            Destroy(transform.root.gameObject);
        }
	}
}
