using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CubeMesh : MonoBehaviour {
    public Vector3[] verts;
    public GameObject myDots;
    // Use this for initialization
    void Start () {
        CreateCube();
        CreateSphere();

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
        DrawLine(transform.TransformPoint(vertices[2]), transform.TransformPoint(vertices[3]), color);
        DrawLine(transform.TransformPoint(vertices[3]), transform.TransformPoint(vertices[4]), color);
        DrawLine(transform.TransformPoint(vertices[4]), transform.TransformPoint(vertices[5]), color);
        DrawLine(transform.TransformPoint(vertices[5]), transform.TransformPoint(vertices[2]), color);
    }
    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth =0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        
       // GameObject.Destroy(myLine, duration);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
