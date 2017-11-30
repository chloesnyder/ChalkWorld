using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CubeMesh : MonoBehaviour {
    public Vector3[] verts;
    public GameObject myDots;
    private int count = 0;
    private GameObject[] dots = new GameObject[8];
    private GameObject[] lines=new GameObject[4];
    // Use this for initialization
    void Start () {
        CreateCube();
        CreateSphere();
       // dots = new GameObject[8];
       // lines = new List<GameObject>();
        /*
        dots.Capacity = 8;
        lines.Capacity = 4;
      
        for(int i = 0; i < 8; i++)
        {
            dots[i] = new GameObject();
        }
        for(int i = 0; i < 4; i++)
        {
            lines[i] = new GameObject();
        }
        */
    }
    void OnMouseDown()
    {
        GameObject obj = GameObject.Find("GlobalObject");
        Global g = obj.GetComponent<Global>();
        count++;
        Debug.Log("click cube");
    }


    private void CreateSphere()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        verts = mesh.vertices;
        Debug.Log(mesh.vertices[0]);
        for (int i = 0; i < verts.Length; i++)
        {

            //  GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // sphere.transform.position = verts[i]+gameObject.transform.position;
            // sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            // instantiate the Bullet
            transform.TransformPoint(verts[i]);
             GameObject obj = Instantiate(myDots, transform.TransformPoint(verts[i]), transform.rotation) as GameObject;
            //  dots[i]=Instantiate(myDots, transform.TransformPoint(verts[i]), transform.rotation) as GameObject;
            // get the Bullet Script Component of the new Bullet instance
            // DotsController b = dots[i].GetComponent<DotsController>();
            dots[i] = obj;
        }
    }
    private void CreateCube()
    {
        Vector3[] vertices = {
            new Vector3 (-0.5f, -0.5f, -0.5f),
            new Vector3 (0.5f, -0.5f, -0.5f),
            new Vector3 (0.5f, 0.5f, -0.5f),
            new Vector3 (-0.5f, 0.5f, -0.5f),
            new Vector3 (-0.5f, 0.5f, 0.5f),
            new Vector3 (0.5f, 0.5f, 0.5f),
            new Vector3 (0.5f, -0.5f, 0.5f),
            new Vector3 (-0.5f, -0.5f, 0.5f),
        };

        int[] triangles = {
            0, 2, 1, //face front
			0, 3, 2,
            2, 3, 4, //face top
			2, 4, 5,
            1, 2, 5, //face right
			1, 5, 6,
            0, 7, 4, //face left
			0, 4, 3,
            5, 4, 7, //face back
			5, 7, 6,
            0, 6, 7, //face bottom
			0, 1, 6
        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
       MeshUtility.Optimize(mesh);
        mesh.RecalculateNormals();
        Color color = new Color(1.0f, 0.8f, 0.2f);
        DrawLine(0,transform.TransformPoint(vertices[2]), transform.TransformPoint(vertices[3]), color);
        DrawLine(1,transform.TransformPoint(vertices[3]), transform.TransformPoint(vertices[4]), color);
        DrawLine(2,transform.TransformPoint(vertices[4]), transform.TransformPoint(vertices[5]), color);
        DrawLine(3,transform.TransformPoint(vertices[5]), transform.TransformPoint(vertices[2]), color);
    }
    void DrawLine(int i, Vector3 start, Vector3 end, Color color)
    {
        GameObject myline = new GameObject();
        myline.transform.position = start;
        myline.AddComponent<LineRenderer>();
        LineRenderer lr = myline.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth =0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lines[i] = myline;
       // lines.Add(myLine);
       // GameObject.Destroy(myLine, duration);
    }
    // Update is called once per frame
    void Update () {
        GameObject obj = GameObject.Find("GlobalObject");
        Global g = obj.GetComponent<Global>();
        if (g.count == 4)
        {
            Destroy(gameObject);
           for(int i = 0; i < 8; i++)
            {
                Destroy(dots[i]);
            }
           for(int i = 0; i < 4; i++)
            {
                Destroy(lines[i]);
            }
           for(int i = 0; i < g.lines.Capacity; i++)
            {
                Destroy(g.lines[i]);
            }
        }
	}
}
