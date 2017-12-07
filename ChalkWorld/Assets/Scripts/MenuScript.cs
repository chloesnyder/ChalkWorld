using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MenuScript : MonoBehaviour { 

	public VRTK_ControllerReference rightController;
    public VRTK_ControllerEvents controlEvents;
	public GameObject menu;
	public VRTK_Pointer pointer;


    public float numCubes;
    public float numCylinders;
    public bool eraseMode;

    public bool canDrawCube;
    public bool canDrawCylinder;

    bool menuActive;



	// Use this for initialization
	void Start () {
        menuActive = false;
        numCubes = 5;
        numCylinders = 5;
	}

    void Update()
    {
        menu.SetActive(menuActive);
        pointer.enabled = menuActive;
        pointer.activateOnEnable = true;

        pointer.pointerRenderer.enabled = menuActive;


    }

 
    // Update is called once per frame
    void FixedUpdate () {
        // toggles menu on and off on touchpad press
 

        if(controlEvents.touchpadPressed)
        {
            menuActive = !menuActive;
        }
  

	}


    public void cubePressed()
    {
        // enables drawing a cube, takes a cube out of inventory
        numCubes--;
        canDrawCube = true;
        canDrawCylinder = false;
    }

    public void cylinderPressed()
    {
        // enables drawing a cube, takes a cube out of inventory
        numCylinders--;
        canDrawCube = false;
        canDrawCylinder = true;
    }

    public void eraseModeToggled(bool val)
    {
        eraseMode = val;
    }






}


