using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source code: https://unity3d.college/2016/11/16/steamvr-controller-input/
public class LeftController : MonoBehaviour {

    // Use this for initialization
	private SteamVR_TrackedController _controller;
    private bool is_collide = false;
	private bool editing = false;
    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    // 2
    private GameObject objectInHand;
    private GameObject objToErease;
    private GameObject objToextrude;
    private Vector3 far= new Vector3(-100, -100, -100);
    private Vector3 start;
    private Vector3 end;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        start = far;
    }
    private void SetCollidingObject(Collider col)
    {
        // 1
        if (col.CompareTag("Dot"))
        {
            collidingObject = col.gameObject;
            Debug.Log("set the colliding object");
        }
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
        SetCollidingObject(other);

    }

    // 2
    public void OnTriggerStay(Collider other)
    {
			
			SetCollidingObject (other);


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
			Debug.Log ("in grab");
			objectInHand = collidingObject;
			collidingObject = null;
			// 2
			var joint = AddFixedJoint ();
			joint.connectedBody = objectInHand.GetComponent<Rigidbody> ();
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
			if (GetComponent<FixedJoint> ()) {
				// 2
				GetComponent<FixedJoint> ().connectedBody = null;
				Destroy (GetComponent<FixedJoint> ());
				// 3
				objectInHand.GetComponent<Rigidbody> ().velocity = Controller.velocity;
				objectInHand.GetComponent<Rigidbody> ().angularVelocity = Controller.angularVelocity;
			}
			// 4
			objectInHand = null;
    }

	private void OnEnable()
	{
		_controller = GetComponent<SteamVR_TrackedController>();
		_controller.TriggerClicked += HandleTriggerClicked;
        _controller.TriggerUnclicked += HandleTriggerUnClicked;
		_controller.PadClicked += HandlePadClicked;
	}

	private void OnDisable()
	{
		_controller.TriggerClicked -= HandleTriggerClicked;
        _controller.TriggerUnclicked -= HandleTriggerUnClicked;
		_controller.PadClicked -= HandlePadClicked;
	}

	public void editingToggle(){
		editing = !editing;
	}
	private void HandlePadClicked(object sender, ClickedEventArgs e){
		editing = !editing;
	}
    private void HandleTriggerUnClicked(object sender, ClickedEventArgs e)
    {
        Debug.Log("uncliceked trigger");
		if (editing) {
			if (start != far) {        //get one point to extrude;
            
				if (objToextrude != null) {

					Vector3 triggerPoint = transform.position;
					end = objToextrude.transform.InverseTransformPoint (triggerPoint);
                    Extrude_cube extrude = objToextrude.GetComponent<Extrude_cube> ();
					extrude.Extrude (start, end);
					start = far;
                
				} else {
					Debug.Log ("obj to extrude is null");
				}
			}
			start = far;
			objToextrude = null;
			objToErease = null;
		} else {
			ReleaseObject ();
		}
    }


    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
	{
        // this object was clicked - do something
        Debug.Log("cliceked trigger");
        GameObject obj = GameObject.Find("GlobalObject");
		Global g = obj.GetComponent<Global>();
		bool status = g.selected;
		
		if (editing) {
        
			if (collidingObject != null) {
				if (collidingObject.CompareTag ("Ecube")) {
					Debug.Log ("trigger clicked extude cube");
					//get the start point
					Vector3 triggerplace = transform.position;
					Vector3 localtrigger = collidingObject.transform.InverseTransformPoint (triggerplace);
					Debug.Log ("the trigger in cube space is" + localtrigger);
                    Extrude_cube extrude = collidingObject.GetComponent<Extrude_cube> ();
               
					start = extrude.faces [0].center;
					float distance = (localtrigger - start).magnitude;
					for (int i = 1; i < extrude.faces.Count; i++) {
						float length = (extrude.faces [i].center - localtrigger).magnitude;
						if (distance > length) {
							start = extrude.faces [i].center;
						}
					}
					Debug.Log ("the start point to extrude is" + start);
					objToextrude = collidingObject;
                    g.selected = false;
                }
            
				if (collidingObject.CompareTag ("Dot")) {
					//if (is_collide == true) {
						if (status == false) {
                            status = true;
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
							if (g.count >= 4) {

								collidingObject.GetComponent<DotController> ().cube.GetComponent<Extrude_cube> ().Die ();
								g.count = 0;
								collidingObject = null;
								// Destroy(gameObject);
							}
						}
					//}
				}
            
			} else {
				Debug.Log ("trigger clicked but no colliding object");
			}
		} else {
			if(collidingObject != null)
				GrabObject();
		}
     
	}

    /*
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
     else if (collider.CompareTag("Ecube"))
        {
            Debug.Log("collide with ecube");
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
  */

    // Update is called once per frame
    void Update () {
//        if (Controller.GetHairTriggerDown())
//        {
//            if (collidingObject)
//            {
//                GrabObject();
//            }
//        }

        // 2
//        if (Controller.GetHairTriggerUp())
//        {
//            if (objectInHand)
//            {
//                ReleaseObject();
//            }
//        }
	}
}
