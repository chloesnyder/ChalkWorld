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

       numCubes = 0;

       numCylinders = 0;


       cylinderNumText.text = numCylinders.ToString();

       cubeNumText.text = numCubes.ToString();


      // cubeButton.onClick.AddListener(cubePressed);

      // cylinderButton.onClick.AddListener(cylinderPressed);

        rc = rightControllerObject.GetComponent<RightController>(); //GameObject.FindGameObjectWithTag("DrawingHand").GetComponent<RightController>();
        
        pointer.enabled = false;

    }




void Update()
    {

        cylinderNumText.text = numCylinders.ToString();
        cubeNumText.text = numCubes.ToString();

    }

 
 //   // Update is called once per frame
 //   void FixedUpdate () {
   

	//}


/*    void cubePressed()
    {
        // enables drawing a cube, takes a cube out of inventory
        
        numCubes--;
        if (numCubes < 0)
        {
            numCubes = 0;
            rc.setSpawn("None");
        }
        else
        {
            rc.setSpawn("Square");
            canDrawCube = true;
            canDrawCylinder = false;
        }
    }

    void cylinderPressed()
    {
        numCylinders--;
        if (numCylinders < 0)
        {
            numCylinders = 0;
            rc.setSpawn("None");
        }
        else
        {
            rc.setSpawn("Circle");
            canDrawCube = false;
            canDrawCylinder = true;
        }
    }
    */
	public void addCylinder()
	{
        Debug.Log("added cylinder");
		numCylinders++;
        Debug.Log("Num of cylinder = " + numCylinders);
	}

	public void addCube()
	{
		numCubes++;
	}

    public void subCube()
    {
        numCubes--;
    }

    public void subCylinder()
    {
        numCylinders--;
    }





}


