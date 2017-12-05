using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MenuScript : MonoBehaviour {

	public VRTK_ControllerReference leftController;
	public VRTK_ControllerReference rightController;
	public GameObject menu;
	public VRTK_Pointer pointer;
	bool turnOn;
	bool pressedOnce;

	// Use this for initialization
	void Start () {
		turnOn = false;
		pressedOnce = false;
	}
	
	// Update is called once per frame
	void Update () {
		// toggles menu on and off on touchpad press
		bool active = pointer.IsActivationButtonPressed ();
		menu.SetActive (active);


	}


}


