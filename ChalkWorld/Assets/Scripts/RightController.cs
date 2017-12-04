using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source code: https://unity3d.college/2016/11/16/steamvr-controller-input/
public class RightController : MonoBehaviour {

	// Use this for initialization
	public GameObject SpawnedCube;
	public GameObject SpawnedCylinder;

	private SteamVR_TrackedController _controller;
	private bool spawn;
	private string spawnObj;
	private bool is_collide = false;
	private SteamVR_TrackedObject trackedObj;
	private GameObject collidingObject;
	// 2
	private GameObject objectInHand; 
	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
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
		// the Collision contains a lot of info, but it’s the colliding
		// object we’re most interested in. 
		Debug.Log("enter collision");
		Collider collider = collision.collider;
		// AudioSource.PlayClipAtPoint(appear, Camera.main.transform.position);
		if (collider.CompareTag("Dot"))
		{

			is_collide = true;

			// a.transform.rotation = Camera.main.transform.rotation;
		}

		else
		{ // if we collided with something else, print to console
			// what the other thing was
			Debug.Log("Collided with " + collider.tag);
		}
	}

	void OnCollisionExit(Collision collision)
	{
		// the Collision contains a lot of info, but it’s the colliding
		// object we’re most interested in. 
		Collider collider = collision.collider;
		// AudioSource.PlayClipAtPoint(appear, Camera.main.transform.position);
		if (collider.CompareTag("Dot"))
		{

			is_collide = false;

			// a.transform.rotation = Camera.main.transform.rotation;
		}

		else
		{ // if we collided with something else, print to console
			// what the other thing was
			Debug.Log("Collided with " + collider.tag);
		}
	}
	public void setSpawn(string obj){
		spawnObj = obj;
		spawn = true;
	}

	void SpawnObject(){
		GameObject spawned;
		if (spawnObj.Equals ("Square")) {
			Debug.Log ("Spawned a cube");
			spawned = Instantiate (SpawnedCube, Vector3.zero, Quaternion.identity);
		} else if (spawnObj.Equals ("Circle")) {
			Debug.Log ("Spawned a cylinder");
			spawned = Instantiate (SpawnedCylinder, Vector3.zero, Quaternion.identity);
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
