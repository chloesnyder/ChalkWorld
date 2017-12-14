using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source code: https://unity3d.college/2016/11/16/steamvr-controller-input/
public class RightController : MonoBehaviour {

	// Use this for initialization
	public GameObject SpawnedCube;
	public GameObject SpawnedCylinder;

	private SteamVR_TrackedController _controller;
	private GameObject leftController;
	private bool spawn;
	private string spawnObj;
	private bool is_collide = false;
	private SteamVR_TrackedObject trackedObj;
	private GameObject collidingObject;
	// 2
	private GameObject objectInHand;
    GameObject go;
        Global g;
    private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
        go = GameObject.Find("GlobalObject");
        g = go.GetComponent<Global>();
        //leftController = GameObject.FindGameObjectWithTag ("LeftController");
    }
	private void SetCollidingObject(Collider col)
	{
		// 1
		if (collidingObject || !col.GetComponent<Rigidbody>())
		{
			return;
		}
		// 2
		collidingObject = col.gameObject;
		Debug.Log("set the colliding object");

	}
	public void OnTriggerEnter(Collider other)
	{
		Debug.Log("trigger enter");
		Debug.Log ("" + other.tag);
		if(other.CompareTag ("Dot")){
			is_collide = true;
		}
	}

	// 2
	public void OnTriggerStay(Collider other)
	{
		Debug.Log("trigger stay");
		// SetCollidingObject(other);
		//if (other.CompareTag ("Cube")) {

		//	SetCollidingObject (other);
		//other.gameObject.GetComponent<CubeMesh>().Die ();
		//}

	}

	// 3
	public void OnTriggerExit(Collider other)
	{
		Debug.Log("trigger exit");
		if (!collidingObject)
		{
			return;
		}
		if(other.CompareTag ("Dot")){
			is_collide = false;
		}
		collidingObject = null;
	}
	private void GrabObject()
	{
		// 1
		Debug.Log("in grab");
		objectInHand = collidingObject;
		collidingObject = null;
		// 2
		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
	}

	// 3
	private FixedJoint AddFixedJoint()
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}
	private void ReleaseObject()
	{
		Debug.Log("in release");
		// 1
		if (GetComponent<FixedJoint>())
		{
			// 2
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());
			// 3
			objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
		}
		// 4
		objectInHand = null;
	}

	private void OnEnable()
	{
		_controller = GetComponent<SteamVR_TrackedController>();
		_controller.TriggerClicked += HandleTriggerClicked;
		//_controller.PadClicked += HandlePadClicked;
	}

	private void OnDisable()
	{
		_controller.TriggerClicked -= HandleTriggerClicked;
		//_controller.PadClicked -= HandlePadClicked;
	}


	private void HandleTriggerClicked(object sender, ClickedEventArgs e)
	{


	}


	void OnCollisionEnter(Collision collision)
	{

	}

	void OnCollisionExit(Collision collision)
	{

	}
	public void setSpawn(string obj){
        // GameObject go = GameObject.Find("GlobalObject");
        // Global g = go.GetComponent<Global>();
        Debug.Log("Entering set spawn " + obj);
        spawnObj = obj;
        Debug.Log(g == null);
        spawn = g.checkInventory(obj);
        Debug.Log("Spawn bool: " + spawn);
    }

	void SpawnObject(){
		GameObject spawned = null;
		if (spawnObj.Equals ("Square")) {
			Debug.Log ("Spawned a cube");
			spawned = Instantiate (SpawnedCube, _controller.transform.position, Quaternion.identity);
            g.decrementInventory(spawnObj);
		} else if (spawnObj.Equals ("Circle")) {
			Debug.Log ("Spawned a cylinder");
			spawned = Instantiate (SpawnedCylinder, _controller.transform.position, Quaternion.identity);
            g.decrementInventory(spawnObj);
        } else
        {
            spawned = null;
        }
//		objectInHand = spawned;
//		collidingObject = null;
//		var joint = AddFixedJoint();
//		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();

	}
		

	// Update is called once per frame
	void Update () {
		if (spawn) {
			SpawnObject ();
			spawn = false;
		}
		if (Controller.GetHairTriggerDown())
		{
			if (collidingObject) {
				GrabObject ();
			} else if (objectInHand) {
				//this handles the case where an object has be spawned and we want to drop it in the scene
				ReleaseObject ();
			}
		}

		// 2
		if (Controller.GetHairTriggerUp())
		{
			if (objectInHand)
			{
				ReleaseObject();
			}
		}
	}
}
