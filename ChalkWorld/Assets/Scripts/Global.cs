using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Global : MonoBehaviour {
    public Vector3 start;
    public Vector3 end;
    public Color color;
    public int count = 0;
    // Use this for initialization
    public bool selected;
    public List<GameObject> lines = new List<GameObject>();
    public int starNum;
    public GameObject[] stars;

    public bool onFloor = false;
    public float onFloorTimer = 1.5f;

    public GameObject rightControllerObject;
    //public MenuScript menuScript;

    void Start () {
        selected = false;
       stars= GameObject.FindGameObjectsWithTag("star");


		//menuScript = GetComponent<MenuScript> ();
        
	}
	
	// Update is called once per frame
	void Update () {
        stars = GameObject.FindGameObjectsWithTag("star");
        if (stars.Length <= 0)
        {
            SceneManager.LoadScene("Win_screen", LoadSceneMode.Single);
        }

        if (onFloor == true)
        {
            onFloorTimer -= Time.deltaTime;
            if (onFloorTimer <= 0)
            {
                SceneManager.LoadScene("Lose_screen", LoadSceneMode.Single);
            }
        }
        else
        {
            onFloorTimer = 1.5f;
        }

	}
   
    public void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        count++;
        lines.Add(myLine);
        // GameObject.Destroy(myLine, duration);
    }

	public void addCube()
	{
		//menuScript.addCube();
	}

	public void addCylinder()
	{
		//menuScript.addCylinder();
	}

    public void incrementInv(string shape)
    {
        //RightController rc = rightControllerObject.GetComponent<RightController>();
        Debug.Log("inside increment inv");
        if (shape.Equals("Cylinder"))
        {
            rightControllerObject.GetComponent<MenuScript>().addCylinder();
        }
        else if (shape.Equals("Cube"))
        {
            rightControllerObject.GetComponent<MenuScript>().addCube();
        }
    }

}
