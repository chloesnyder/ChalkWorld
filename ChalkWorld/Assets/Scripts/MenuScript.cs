using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    // UI elts
    public Text cubeNumText;
    public Text cylinderNumText;
    public Button cubeButton;
    public Button cylinderButton;
    public Toggle eraseModecheckBox;

    public GameObject rightControllerObject;

    RightController rc;


    //Use this for initialization

   void Start () {

       menuActive = true;

       numCubes = 5;

       numCylinders = 5;


       cylinderNumText.text = numCylinders.ToString();

       cubeNumText.text = numCubes.ToString();


       cubeButton.onClick.AddListener(cubePressed);

       cylinderButton.onClick.AddListener(cylinderPressed);

        rc = rightControllerObject.GetComponent<RightController>(); //GameObject.FindGameObjectWithTag("DrawingHand").GetComponent<RightController>();
        
        //if (rc)
        //{

        //   Debug.Log("what the fuck it's there...");
        //}

        pointer.enabled = false;
    // eraseModecheckBox.onValueChanged
    }




void Update()
    {
      //  menu.SetActive(menuActive);
      //  pointer.enabled = menuActive;
      //  pointer.activateOnEnable = true;

      //  pointer.pointerRenderer.enabled = menuActive;

        cylinderNumText.text = numCylinders.ToString();
        cubeNumText.text = numCubes.ToString();

    }

 
    // Update is called once per frame
    void FixedUpdate () {
        // toggles menu on and off on touchpad press
 

     //   if(controlEvents.touchpadPressed)
    //    {
    //        menuActive = !menuActive;
     //   }
  

	}


    void cubePressed()
    {
        // enables drawing a cube, takes a cube out of inventory
        // rightControllerScript.setSpawn("Square");
        rc.setSpawn("Square");
        numCubes--;
        canDrawCube = true;
        canDrawCylinder = false;
    }

    void cylinderPressed()
    {
        rc.setSpawn("Circle");
        numCylinders--;
        canDrawCube = false;
        canDrawCylinder = true;
    }

    void eraseModeToggled()
    {
        eraseMode = !eraseMode;
    }






}


