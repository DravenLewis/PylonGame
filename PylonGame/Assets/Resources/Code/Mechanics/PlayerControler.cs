using System;
using System.Collections.Generic;
using UnityEngine;
using Code.System;


public class PlayerControler : MonoBehaviour {

    /// <summary>
    /// Data Structure meant to be a Wrapper Around the Unity Game Object 
    /// That will allow special verisons of the same game object to be expressed
    /// as individual gameobjects. 
    /// </summary>
    public class PlayerGameObject {
        private List<GameObject> InternalizedGameObjectList;
        private int DefaultIndex;
        private String name;
        private Dictionary<System.Object,System.Object> VariableDictionary;

        public PlayerGameObject(GameObject @object, String name)
        {
            this.InternalizedGameObjectList = new List<GameObject>();
            this.DefaultIndex = 0;
            this.InternalizedGameObjectList.Insert(this.DefaultIndex,@object);
            this.name = name;
            this.VariableDictionary = new Dictionary<object, object>();
        }

        public void AddAlternate(int index, GameObject @object)
        {
            this.InternalizedGameObjectList.Insert(index, @object);
        }

        public GameObject Get() {
            return this.InternalizedGameObjectList.ToArray()[this.DefaultIndex];
        }

        public GameObject GetAlternate(int at)
        {
            return this.InternalizedGameObjectList.ToArray()[at];
        }

        public void Remove(int at)
        {
            // do not allow the default form to be overwritten
            if (at == this.DefaultIndex) { return; }
            this.InternalizedGameObjectList.RemoveAt(at);
        }

        public void SetName(String name) {
            this.name = name;
        }

        public String GetName() {
            return this.name;
        }

        public PlayerGameObject GetAsAlternate(int at) {
            return new PlayerGameObject(this.GetAlternate(at), this.GetName());
        }

        public static PlayerGameObject GetEmpty() {
            return new PlayerGameObject(null,"NULL");
        }
    }

    // Player Hold Item Code
    public const int ITEM_CROSSBOW  = 0;
    public const int ITEM_ARROW     = 1;
    public const int ITEM_SWORD     = 2;
    public const int ITEM_NULL_0    = 3;
    public const int ITEM_NULL_1    = 4;
    public const int ITEM_NULL_2    = 5;
    public const int ITEM_NULL_3    = 6;
    public const int ITEM_NULL_4    = 7;
    public const int ITEM_NULL_5    = 8;
    public const int ITEM_NULL_6    = 9;

    // Player Hold Item List
    private PlayerGameObject[] HoldablePrefabs = new PlayerGameObject[10];

    // Player Building Item Code
    private PlayerGameObject[] BuildableMaterials = new PlayerGameObject[10];

    // Hold Item Layout Code
    public float HandScroll = 0.0f;
    public float HandHeight = 0.0f;
    public float HandDepth =  0.0f;
    // And The Rotations that come with them
    public float DefualtForwardRotation = 0.0f;
    public float MaxSwing = 20;
    public float SwingSpeed = 1;

    // Arrow Spawn Location
    public GameObject ArrowSpawnLocation = null;

    // private members for the swinging code
    private bool Swinging = false;
    private float SwingIndex = 0.0f;

    // Render GameItem
    private PlayerGameObject InternalHoldObject = PlayerGameObject.GetEmpty();
    private PlayerGameObject InternalBufferObject = PlayerGameObject.GetEmpty();
    private String InternalItemName = "NULL";

    // Internal Build Items
    private GameObject InternalBuildingMaterial = null;

    // State handling code
    private int GlobalVarsLastMode = 0;

    // Interface into the Building Script
    private Component BuildingScript = null;


    public class CrossBowVars {
        public static bool IsShooting = false;
        public static int ShootTime = 0;
        public static int MaxShootTime = 100;
        public static int CrossBowCharge = 1;
        public static int CrossBowChargeMax = 20;
        public static void HandleShootVars(PlayerControler c) {

            if (ShootTime <= 0 && IsShooting == true){
                c.SetHand(c.HoldablePrefabs[PlayerControler.ITEM_CROSSBOW].GetAsAlternate(1));
                if (ShootTime < 0) ShootTime = 0;
                CrossBowCharge = 1;
                IsShooting = false;
            }
            else if (IsShooting == true && ShootTime > 0){
                    c.SetHand(c.HoldablePrefabs[PlayerControler.ITEM_CROSSBOW]);
            }

            if (ShootTime > 0){
                ShootTime--;
            }

            if (CrossBowCharge > CrossBowChargeMax) {
                CrossBowCharge = CrossBowChargeMax;
            }
        }

        public static void Shoot() {
            ResetShootTime();
            IsShooting = true;
        }

        public static void ResetShootTime() {
            ShootTime = MaxShootTime;
        }

        public static bool Ready() {
            return ShootTime == 0 && IsShooting == false;
        }
    }

    // PC CONTROLS
    // the index of the key name is the array location 
    // in the main controller
    // eg. indexOf("1") = 0

    public string[] ControlableHotbarKeys = {
        "1","2","3","4","5","6","7","8","9","0"
    };



    public void Start() {
        // Load the prefabs for holding
        this.HoldablePrefabs[PlayerControler.ITEM_CROSSBOW] = new PlayerGameObject(Resources.Load("Models/CrossBow/p_CrossBowNext", typeof(GameObject)) as GameObject,"CrossBow");
        this.HoldablePrefabs[PlayerControler.ITEM_CROSSBOW].AddAlternate(1,Resources.Load("Models/CrossBow/p_CrossBowLoaded", typeof(GameObject)) as GameObject);
        this.HoldablePrefabs[PlayerControler.ITEM_ARROW   ] = new PlayerGameObject(Resources.Load("Models/CrossBow/p_CrossBowBoltNF", typeof(GameObject)) as GameObject,"CrossBow Bolt");
        this.HoldablePrefabs[PlayerControler.ITEM_SWORD   ] = new PlayerGameObject(Resources.Load("Models/Sword/Sword", typeof(GameObject)) as GameObject, "Sword");
        this.HoldablePrefabs[PlayerControler.ITEM_NULL_0  ] = PlayerGameObject.GetEmpty();
        this.HoldablePrefabs[PlayerControler.ITEM_NULL_1  ] = PlayerGameObject.GetEmpty();
        this.HoldablePrefabs[PlayerControler.ITEM_NULL_2  ] = PlayerGameObject.GetEmpty();
        this.HoldablePrefabs[PlayerControler.ITEM_NULL_3  ] = PlayerGameObject.GetEmpty();
        this.HoldablePrefabs[PlayerControler.ITEM_NULL_4  ] = PlayerGameObject.GetEmpty();
        this.HoldablePrefabs[PlayerControler.ITEM_NULL_5  ] = PlayerGameObject.GetEmpty();
        this.HoldablePrefabs[PlayerControler.ITEM_NULL_6  ] = PlayerGameObject.GetEmpty();

        // Load the building prefabs
        this.BuildableMaterials[0] = PlayerGameObject.GetEmpty();
        this.BuildableMaterials[1] = PlayerGameObject.GetEmpty();
        this.BuildableMaterials[2] = PlayerGameObject.GetEmpty();
        this.BuildableMaterials[3] = PlayerGameObject.GetEmpty();
        this.BuildableMaterials[4] = PlayerGameObject.GetEmpty();
        this.BuildableMaterials[5] = PlayerGameObject.GetEmpty();
        this.BuildableMaterials[6] = PlayerGameObject.GetEmpty();
        this.BuildableMaterials[7] = PlayerGameObject.GetEmpty();
        this.BuildableMaterials[8] = PlayerGameObject.GetEmpty();
        this.BuildableMaterials[9] = PlayerGameObject.GetEmpty();

        this.BuildingScript = GetComponent<BuildingBehavior>();

        this.SetHand(this.HoldablePrefabs[PlayerControler.ITEM_CROSSBOW].GetAsAlternate(1));
    }


    public void Update() {

        if (this.GlobalVarsLastMode != GlobalVars.mode) {
            this.PreformPreModeExecutionSetup();
        }

        switch (GlobalVars.mode){
            case GlobalVars.MODE_ATTACK:
                this.ActionPreformAttack();
                break;
            case GlobalVars.MODE_BUILD:
                this.ActionPreformBuild();
                break;
            default:
                GlobalVars.mode = GlobalVars.MODE_ATTACK;
                break;
        }

        // When we are done updating, we set some flags as to track events.
        this.GlobalVarsLastMode = GlobalVars.mode;
    }

    public void ActionPreformAttack() {
        for (int i = 0; i < this.ControlableHotbarKeys.Length; i++){
            if (Input.GetKeyDown(this.ControlableHotbarKeys[i])){
                if (this.HoldablePrefabs[i].Get() != null){
                    this.PreformPreModeExecutionSetup();
                    this.SetHand(this.HoldablePrefabs[i]);
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (this.InternalHoldObject.GetName() == this.HoldablePrefabs[PlayerControler.ITEM_CROSSBOW].GetName()) {
               
                if (CrossBowVars.Ready()) {

                    CrossBowVars.Shoot();

                    Transform arrow_transform = this.ArrowSpawnLocation.transform;
                    GameObject arrow = Instantiate(this.HoldablePrefabs[PlayerControler.ITEM_ARROW].Get(),
                        arrow_transform.position,
                        arrow_transform.rotation
                    );
                    Rigidbody arrow_rb = arrow.GetComponent<Rigidbody>();
                    if (arrow_rb != null)
                    {
                        arrow_rb.MoveRotation(arrow_transform.rotation);
                        arrow_rb.MovePosition(arrow_transform.position);

                        float max = 100 * CrossBowVars.CrossBowCharge;
                        Debug.Log(max);
                        Debug.Log(CrossBowVars.CrossBowCharge);
                        arrow_rb.AddForce(Camera.main.transform.forward * max);
                    }
                }
            }
        }

        if (Input.GetMouseButton(0)){
            if (GlobalVars.countMod(30)) {
                CrossBowVars.CrossBowCharge ++;
                Debug.Log(CrossBowVars.CrossBowCharge);
            }
        }

        if (Input.GetMouseButton(1)) {
            if (this.Swinging == false && this.SwingIndex == 0) {
                this.Swing();
            }
        }

        CrossBowVars.HandleShootVars(this);
        this.HandleItemHolding();
        // Swing Vars
    }


    /// <summary>
    /// TODO Add the building script here.
    /// </summary>
    public void ActionPreformBuild() {
        for (int i = 0; i < this.ControlableHotbarKeys.Length; i++) { 
            if (Input.GetKeyDown(this.ControlableHotbarKeys[i])) {
                if (this.HoldablePrefabs[i].Get() != null) {
                    
                }
            }
        }
    }

    // make this a reset
    public void PreformPreModeExecutionSetup() {
        // do setup here like destroy the current hold item 
    }

    public void Swing() {
        if (this.GlobalVarsLastMode == GlobalVars.MODE_ATTACK) {
            this.Swinging = true;
        }
    }

    // Damage Item
    public void DamageItem(int hp) {
        if (this.GlobalVarsLastMode == GlobalVars.MODE_ATTACK) {
            Camera currentCamera = Camera.main;
            System.Object[] hit = Tools.getRayFromCursor(currentCamera, 0, 10);
            if ((bool)hit[Tools.HIT_HAPPENED] == true)
            {
                GameObject o = (hit[Tools.HIT_GAMEOBJECT] as GameObject);
                ParticleSystem particleSystem = GlobalVars.Prefabs.MaterialParticleSystem; 
                if (particleSystem != null)
                {
                    ParticleSystem s = (ParticleSystem)Instantiate(particleSystem, new Vector3((float)hit[Tools.HIT_LOCATION_X], (float)hit[Tools.HIT_LOCATION_Y], (float)hit[Tools.HIT_LOCATION_Z]), Quaternion.identity);
                    s.GetComponent<Renderer>().material = (hit[Tools.HIT_GAMEOBJECT] as GameObject).GetComponent<Renderer>().material;
                }
                Component destroy_script = destroy_script = o.GetComponent<Destroyable>();
                if (destroy_script != null)
                {
                    (destroy_script as Destroyable).hit(hp);
                }
            }
        }
    }

    // Item Switching Code
    public void SetHand(PlayerGameObject o) {
        this.PreformPreModeExecutionSetup();
        if (this.InternalHoldObject.Get() != null) Destroy(this.InternalHoldObject.Get());
        this.InternalBufferObject = o;
        this.InternalHoldObject = new PlayerGameObject((Instantiate(this.InternalBufferObject.Get(), Vector3.zero, Quaternion.identity)),o.GetName());
        this.InternalItemName = o.GetName();
    }

    public void SetBuildingMaterial(int index = 0) {
        this.InternalBuildingMaterial = this.BuildableMaterials[0].Get();
        this.PreformPreModeExecutionSetup();
    }

    public void HandleItemHolding() {
        Camera gameCamera = Camera.main;
        if (this.Swinging)
        {
            if (this.SwingIndex < this.MaxSwing){
                SwingIndex += this.SwingSpeed;
            }
            else if (SwingIndex >= MaxSwing){
                this.Swinging = false;
                if (this.BuildingScript != null){
                    (this.BuildingScript as BuildingBehavior).setEnabled(true);
                }
                this.DamageItem(5);
            }
        }
        else{
            if (SwingIndex > 0){
                SwingIndex -= this.SwingSpeed;
            }

            if (SwingIndex < 0){
                SwingIndex = 0;
            }
        }
        if (!this.InternalHoldObject.Get().activeSelf){
            this.InternalHoldObject.Get().SetActive(true);
        }

        // set the location... Infront of the camera (if this works then we should see the sword)
        this.InternalHoldObject.Get().transform.position = gameCamera.transform.position +
            (gameCamera.transform.forward * this.HandDepth) +  // local Z
            (gameCamera.transform.right * this.HandScroll) + // local X
            (gameCamera.transform.up * this.HandHeight); // local Y

        this.InternalHoldObject.Get().transform.eulerAngles = new Vector3(
            gameCamera.transform.eulerAngles.x +
            this.DefualtForwardRotation +
            this.SwingIndex,

            gameCamera.transform.eulerAngles.y +
            this.SwingIndex,

            this.InternalBufferObject.Get().transform.eulerAngles.z +
            this.SwingIndex +
            (UnityEngine.Random.Range(-10, 10) * (this.Swinging ? 1 : 0))
        );
        this.InternalHoldObject.Get().transform.RotateAroundLocal(this.InternalHoldObject.Get().transform.forward, this.InternalBufferObject.Get().transform.eulerAngles.z);
        this.InternalHoldObject.Get().transform.RotateAroundLocal(this.InternalHoldObject.Get().transform.right, this.InternalBufferObject.Get().transform.eulerAngles.x);
        this.InternalHoldObject.Get().transform.RotateAroundLocal(this.InternalHoldObject.Get().transform.up, this.InternalBufferObject.Get().transform.eulerAngles.y);
    }
}