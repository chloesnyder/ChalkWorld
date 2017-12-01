using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source code: https://unity3d.college/2016/11/16/steamvr-controller-input/
public class LeftController : MonoBehaviour {

    // Use this for initialization
	private SteamVR_TrackedController _controller;
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
		// this object was clicked - do something
		GameObject obj = GameObject.Find("GlobalObject");
		Global g = obj.GetComponent<Global>();
		bool status = g.selected;
		Debug.Log("mouse down once");
		if (is_collide == true) {
			if (status == false) {
				Debug.Log ("status false");
				g.selected = true;
				g.start = gameObject.transform.position;
				// Destroy(gameObject);
			} else {
				Debug.Log ("status true");
				g.end = gameObject.transform.position;
				g.selected = false;
				g.color = new Color (0.2f, 1, 0.4f);
				g.DrawLine (g.start, g.end, g.color);
				Debug.Log ("start is" + g.start + "and end is " + g.end);
				// Destroy(gameObject);
			}
		}
        
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
  
    /*  void OnMouseDown()
      {
          // this object was clicked - do something
          GameObject obj = GameObject.Find("GlobalObject");
          Global g = obj.GetComponent<Global>();
          bool status = g.selected;
          Debug.Log("mouse down once");
          if (status == false)
          {
              Debug.Log("status false");
              g.selected = true;
              g.start = gameObject.transform.position;
             // Destroy(gameObject);
          }
          else
          {
              Debug.Log("status true");
              g.end = gameObject.transform.position;
              g.selected = false;
              g.color = new Color(0.2f, 1, 0.4f);
              g.DrawLine(g.start, g.end, g.color);
              Debug.Log("start is" + g.start + "and end is " + g.end);
             // Destroy(gameObject);
          }
      }*/
    // Update is called once per frame
    void Update () {
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
            {
                GrabObject();
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
