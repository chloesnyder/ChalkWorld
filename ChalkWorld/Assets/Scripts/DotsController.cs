using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotsController : MonoBehaviour {

    // Use this for initialization
   
	void Start () {
        GameObject obj = GameObject.Find("GlobalObject");
        Global  g = obj.GetComponent<Global>();

    }
    void OnMouseDown()
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
    // Update is called once per frame
    void Update () {
		
	}
}
