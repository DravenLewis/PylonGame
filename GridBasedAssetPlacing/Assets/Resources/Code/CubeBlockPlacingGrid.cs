using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBlockPlacingGrid : MonoBehaviour {

    public int GridSize = 2;
    public GameObject CubePrefab = null;

    private GameObject CubePlacePrefab = null;

    private float MouseX = 0;
    private float MouseY = 0;
    private float MouseLX = 0;
    private float MouseLY = 0;

	// Use this for initialization
	void Start () {
        if (this.CubePrefab != null) {
            this.CubePlacePrefab = Instantiate(this.CubePrefab,Vector3.zero,Quaternion.identity);
            this.CubePlacePrefab.SetActive(false);
            //this.CubePlacePrefab.transform.localScale = new Vector3(0.25f,0.25f,0.25f);
            this.CubePlacePrefab.layer = 2;
        }

        this.MouseLX = this.MouseX = Input.mousePosition.x;
        this.MouseLY = this.MouseY = Input.mousePosition.y;
	}


    void Update() {
        this.MouseLX = this.MouseX;
        this.MouseLY = this.MouseY;
        this.MouseX = Input.mousePosition.x;
        this.MouseY = Input.mousePosition.y;


    }

    bool isDeltaMouse() {
        return (this.MouseX != this.MouseLX) || (this.MouseY != this.MouseLY);
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (this.CubePrefab != null)
        {
            object[] hitData = Tools.getRayFromCursor(Camera.main, 1, 200);
            if ((bool)hitData[Tools.HIT_HAPPENED])
            {
                
                Vector3 hitLocation = new Vector3(
                    this.SnapToGrid(this.GridSize, (float)hitData[Tools.HIT_LOCATION_X]) + (2 * ((float)hitData[Tools.HIT_LOCATION_X]) > 0 ? 1 : -1),
                    this.SnapToGrid(this.GridSize, (float)hitData[Tools.HIT_LOCATION_Y]) + (2 * ((float)hitData[Tools.HIT_LOCATION_Y]) > 0 ? 1 : -1),
                    this.SnapToGrid(this.GridSize, (float)hitData[Tools.HIT_LOCATION_Z]) + (2 * ((float)hitData[Tools.HIT_LOCATION_Z]) > 0 ? 1 : -1)
                );
                
                /*
                Vector3 hitLocation = new Vector3(
                   (float)hitData[Tools.HIT_LOCATION_X] - 1,
                   (float)hitData[Tools.HIT_LOCATION_Y] + 1,
                   (float)hitData[Tools.HIT_LOCATION_Z]
               );
               */
                if (this.CubePlacePrefab.activeSelf == false)
                {
                    this.CubePlacePrefab.SetActive(true);
                }
                else
                {
                    this.CubePlacePrefab.transform.position = hitLocation;/*new Vector3(
                        (float)hitData[Tools.HIT_LOCATION_X],
                        (float)hitData[Tools.HIT_LOCATION_Y],
                        (float)hitData[Tools.HIT_LOCATION_Z]
                    );*/

                    this.CubePlacePrefab.transform.eulerAngles = new Vector3(
                        0,
                        SnapToGrid(45, Camera.main.transform.eulerAngles.y - 90),
                        0
                    );
                }

                if (Input.GetMouseButtonDown(0) == true)
                {

                    Debug.Log("Rotation: " + (Camera.main.transform.eulerAngles.y - 90));
                    Quaternion q = new Quaternion(0, Camera.main.transform.rotation.y, 0, Camera.main.transform.rotation.w);
                    Vector3 CameraEuler = new Vector3(
                        0,
                        SnapToGrid(45, Camera.main.transform.eulerAngles.y - 90),
                        0
                    );
                    Instantiate(this.CubePrefab, hitLocation, Quaternion.Euler(CameraEuler));
                    return;
                }

                if (Input.GetMouseButtonDown(1) == true) {
                    this.CubePlacePrefab = null;
                    this.CubePrefab = null;
                    return;
                }

            }
            else
            {
                this.CubePlacePrefab.SetActive(false);
            }
        }
        else {
            object[] hitData = Tools.getRayFromCursor(Camera.main, 1, 200);
            if ((bool)hitData[Tools.HIT_HAPPENED]) {
                if (((GameObject)hitData[Tools.HIT_GAMEOBJECT]).name.ToLower() != "plane")
                {
                    this.CubePlacePrefab = Instantiate((GameObject)hitData[Tools.HIT_GAMEOBJECT], Vector3.zero, Quaternion.identity);
                    this.CubePrefab = Instantiate((GameObject)hitData[Tools.HIT_GAMEOBJECT], Vector3.zero, Quaternion.identity);
                    this.CubePlacePrefab.SetActive(false);
                    //this.CubePlacePrefab.transform.localScale = new Vector3(0.25f,0.25f,0.25f);
                    this.CubePlacePrefab.layer = 2;
                }   
            }
        }
	}




    // bind the point to a grid
    public float SnapToGrid(float gridSize, float value) {
        float snapCandidate = gridSize * Mathf.Floor(value / gridSize);
        if (value - snapCandidate < gridSize){
            return snapCandidate;
        }else {
            return -1;
        }
    }


}
