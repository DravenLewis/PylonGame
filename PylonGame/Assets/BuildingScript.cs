using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour {

    Material m;
    List<GameObject> go = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        foreach (GameObject o in go) {
            Material mat;
            mat = o.GetComponent<Renderer>().material;
            mat.SetColor("_Color", new Color(1, 1, 1, 1f));
            o.GetComponent<Renderer>().material = mat;
        }


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            GameObject o = hit.transform.gameObject;
            m = o.GetComponent<Renderer>().material;
            m.SetColor("_Color",new Color(0,0,0,0.1f));
            o.GetComponent<Renderer>().material = m;
            go.Add(o);
        }
	}
}
