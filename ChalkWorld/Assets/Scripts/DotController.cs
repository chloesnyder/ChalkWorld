using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter(Collision collision)
	{
		// the Collision contains a lot of info, but it’s the colliding
		// object we’re most interested in. 
		Debug.Log("enter collision");
		//Destroy (gameObject);
	}
}
