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
	void Start () {
        selected = false;
       stars= GameObject.FindGameObjectsWithTag("star");
        
	}
	
	// Update is called once per frame
	void Update () {
        stars = GameObject.FindGameObjectsWithTag("star");
        if (stars.Length <= 0)
        {
            SceneManager.LoadScene("Win_screen", LoadSceneMode.Single);
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
}
