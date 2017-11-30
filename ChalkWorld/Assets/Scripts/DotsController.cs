using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source code: https://unity3d.college/2016/11/16/steamvr-controller-input/
public class DotsController : MonoBehaviour {

    // Use this for initialization
	private SteamVR_TrackedController _controller;

	void Start () {
        GameObject obj = GameObject.Find("GlobalObject");
        Global  g = obj.GetComponent<Global>();

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
		
	}
}
