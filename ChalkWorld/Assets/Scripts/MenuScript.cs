using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MenuScript : MonoBehaviour {

	public VRTK_ControllerReference leftController;
	public VRTK_ControllerReference rightController;
	public GameObject menu;
	public VRTK_Pointer pointer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool active = pointer.IsActivationButtonPressed ();
		if (active){
			menu.SetActive (!menu.activeInHierarchy);
		}
	}


}


