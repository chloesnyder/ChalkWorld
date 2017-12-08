using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRotation : MonoBehaviour {
   public GameObject follow;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 baseposition = new Vector3(0, -3.0f, 0);
        this.transform.position = baseposition+follow.transform.position;
        
        //this.transform.rotation = Quaternion.identity;
	}
}
