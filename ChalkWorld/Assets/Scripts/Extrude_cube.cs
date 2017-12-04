using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Extrude_cube : MonoBehaviour {

    // Use this for initialization
    public struct Face
    {
        public Vector3 center;
        public int[] points;

        public Face(Vector3 center, int[] points)
        {
            this.points = new int[4];
            this.center = center;
            this.points = points;

        }
    }
    public List<Face> faces = new List<Face>();
    public List<Vector3> verts = new List<Vector3>();
    public List<int> triangle = new List<int>();
    public Vector3 start;
    public Vector3 end;
    private Vector3 outbound = new Vector3(1000, 1000, 1000);
    void Start()
    {
        start = outbound;
        end = outbound;
        CreatPlane();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
       // Debug.Log("triangle number is" + mesh.triangles.Length);
        for (int i = 0; i < mesh.vertices.Length; i++)
            // Debug.Log(mesh.vertices[i]);
            GetComponent<MeshCollider>().sharedMesh = mesh;
        for (int i = 0; i < faces.Count; i++)
        {
          //  Debug.Log("face center is:" + faces[i].center);
        }
        Vector3 start1 = new Vector3(0, 0.1f, 0);
        Vector3 end1 = new Vector3(0, 1.2f, 0);
        Extrude(start1, end1);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreatPlane()
    {
        Vector3[] vertices = {
            new Vector3 (-0.5f, -0.1f, -0.5f), //0
            new Vector3 (0.5f, -0.1f, -0.5f),  //1
            new Vector3 (0.5f, 0.1f, -0.5f),    //2
            new Vector3 (-0.5f, 0.1f, -0.5f),   //3
            new Vector3 (-0.5f, 0.1f, 0.5f),    //4
            new Vector3 (0.5f, 0.1f, 0.5f),     //5
            new Vector3 (0.5f, -0.1f, 0.5f),    //6
            new Vector3 (-0.5f, -0.1f, 0.5f),   //7
        };
        for (int i = 0; i < 8; i++)
        {
            verts.Add(vertices[i]);
        }
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
        for (int i = 0; i < 36; i++)
        {
            triangle.Add(triangles[i]);
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        MeshUtility.Optimize(mesh);
        mesh.RecalculateNormals();
        int[] index = { 0, 1, 2, 3 };
        faces.Add(new Face(new Vector3(0, 0, -0.5f), index));
        int[] index2 = { 2, 5, 4, 3 };
        faces.Add(new Face(new Vector3(0, 0.1f, 0), index2));
        int[] index3 = { 1, 6, 5, 2 };
        faces.Add(new Face(new Vector3(0.5f, 0, 0), index3));
        int[] index4 = { 0, 3, 4, 7 };
        faces.Add(new Face(new Vector3(-0.5f, 0, 0), index4));
        int[] index5 = { 4, 5, 6, 7 };
        faces.Add(new Face(new Vector3(0, 0, 0.5f), index5));
        int[] index6 = { 0, 7, 6, 1 };
        faces.Add(new Face(new Vector3(0, -0.1f, 0), index6));
    }
    void GeneFace(int[] v)
    {
        triangle.Add(v[0]);
        triangle.Add(v[2]);
        triangle.Add(v[1]);

        triangle.Add(v[0]);
        triangle.Add(v[3]);
        triangle.Add(v[2]);

        Vector3 center = (verts[v[0]] + verts[v[1]] + verts[v[2]] + verts[v[3]]) / 4;
        faces.Add(new Face(center, v));
    }
    public void Extrude(Vector3 start, Vector3 end)  //start is the central point of the face, 
    {
        Vector3 normal = end - start;
        Face face = new Face();
        for (int i = 0; i < faces.Count; i++)
        {
            if (faces[i].center == start)
            {
                Debug.Log("can find the extrude start point");
                face = faces[i];
                break;
            }
        }
        int index = verts.Count; //get the current number of vert we have;
       // Debug.Log("verts number is" + index);
        //add verts
        Vector3 v0 = verts[face.points[0]] + normal;
        Vector3 v1 = verts[face.points[1]] + normal;
        Vector3 v2 = verts[face.points[2]] + normal;
        Vector3 v3 = verts[face.points[3]] + normal;
        verts.Add(v0);
        verts.Add(v1);
        verts.Add(v2);
        verts.Add(v3);

        //add new triangles& faces

        int[] front = { face.points[0], face.points[1], index + 1, index };
        GeneFace(front);  //front face;
        int[] bank = { face.points[2], face.points[3], index + 3, index + 2 };
        GeneFace(bank);     //bank face;
        int[] right = { face.points[1], face.points[2], index + 2, index + 1 };
        GeneFace(right);   //right face;
        int[] left = { face.points[3], face.points[0], index, index + 3 };
        GeneFace(left);   //left face;
        int[] top = { index, index + 1, index + 2, index + 3 };
        GeneFace(top);    //top face;
        //refresh the mesh
        Vector3[] vertices = verts.ToArray();
        int[] triangles = triangle.ToArray();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        MeshUtility.Optimize(mesh);
        mesh.RecalculateNormals();
        for (int i = 0; i < mesh.normals.Length; i++)
        {
           // Debug.Log("normal is:" + mesh.normals[i]);
        }
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }

    private void OnMouseDown()
    {
        Debug.Log("mouse down");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            Debug.Log("hit something triangle" + hit.triangleIndex);
            int t_index = hit.triangleIndex / 2;
            GameObject obj = hit.collider.gameObject;
            start = obj.GetComponent<Extrude_cube>().faces[t_index].center;
            Debug.Log("start point is" + start);
            Vector3 point = hit.collider.gameObject.transform.InverseTransformPoint(hit.point); //world space to local space
                                                                                                // hit.triangleIndex

        }
    }

    private void OnMouseUp()
    {
        Debug.Log("mouse up");
        Vector3 point = Input.mousePosition;
        end = new Vector3(1, 1, 1);
        if (start != outbound)
        {
            Extrude(start, end);
        }
        start = outbound;
        end = outbound;
    }
}
