using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class EX_Triangular : MonoBehaviour {

    public List<GameObject> dots = new List<GameObject>();
    public GameObject myDots;
    public struct Face
    {
        public Vector3 center;
        public int[] points;

        public Face(Vector3 center, int[] points)
        {
            this.points = new int[3];
            this.center = center;
            this.points = points;

        }
        public void SetCenter(Vector3 center)
        {
            this.center = center;
        }
    }
    public List<Face> faces = new List<Face>();
    public List<Vector3> verts = new List<Vector3>();
    public List<int> triangle = new List<int>();
    public Vector3 start;
    public Vector3 end;
    private Vector3 outbound = new Vector3(1000, 1000, 1000);



    // Use this for initialization
    void Start()
    {
        CreateOrigin();
        CreateSphere();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        Vector3 start1 = new Vector3(0, -0.35f, 0);
        Vector3 end1 = new Vector3(0.2f, -1.2f, 0);
         Extrude(start1, end1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void addDot(Vector3 point)
    {
        GameObject obj = Instantiate(myDots, transform.TransformPoint(point), transform.rotation) as GameObject;
        obj.transform.parent = this.transform;
        obj.GetComponent<DotController>().cube = gameObject;
        dots.Add(obj);
    }

    private void CreateSphere()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        Debug.Log(mesh.vertices[0]);
        for (int i = 0; i < mesh.vertices.Length; i++)
        {

            addDot(mesh.vertices[i]);

        }
    }

    public void Die()
    {
        Destroy(gameObject);
        GameObject obj = GameObject.Find("GlobalObject");
        Global g = obj.GetComponent<Global>();
        for (int i = 0; i < dots.Count; i++)
        {
            Destroy(dots[i]);
        }

        for (int i = 0; i < g.lines.Capacity; i++)
        {
            Destroy(g.lines[i]);
        }

    }
 

    void CreateOrigin()
    {
        Vector3[] vertices = {
            new Vector3 (1, -0.35f, 0), //0
            new Vector3 (-0.5f, -0.35f, 0.86f),  //1
            new Vector3 (-0.5f, -0.35f, -0.86f),    //2
            new Vector3 (0, 1.05f, 0),   //3

        };
        for (int i = 0; i < 4; i++)
        {
            verts.Add(vertices[i]);
        }
        int[] triangles = {
            0,1,2,
            0,3,1,
            0,2,3,
            1,3,2
        };
        for (int i = 0; i < 12; i++)
        {
            triangle.Add(triangles[i]);
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        MeshUtility.Optimize(mesh);
        mesh.RecalculateNormals();
        int[] index = { 0, 1, 2 };
        Vector3 center1 = (vertices[0] + vertices[1] + vertices[2]) / 3;
        faces.Add(new Face(center1, index));
        int[] index2 = { 0, 3, 1};
        Vector3 center2 = (vertices[0] + vertices[1] + vertices[3]) / 3;
        faces.Add(new Face(center2, index2));
        int[] index3 = { 0, 2, 3};
        Vector3 center3 = (vertices[0] + vertices[2] + vertices[3]) / 3;
        faces.Add(new Face(center3, index3));
        int[] index4 = { 1,3, 2 };
        Vector3 center4 = (vertices[1] + vertices[2] + vertices[3]) / 3;
        faces.Add(new Face(center4, index4));
   
    }
    void GeneFace(int[] v)
    {
        triangle.Add(v[0]);
        triangle.Add(v[1]);
        triangle.Add(v[2]);

        Vector3 center = (verts[v[0]] + verts[v[1]] + verts[v[2]] ) / 3;
        faces.Add(new Face(center, v));
    }

    void DeleteFace(Face face)
    {
        List<int> checker = new List<int>() { face.points[0], face.points[1], face.points[2] };
        int f_count = triangle.Count;
        for(int i = 0; i < f_count; i+=3)
        {
            Debug.Log("current i is:" + i);
            Debug.Log("current triangle size is:" + f_count);
            Debug.Log("current triangle array is:" + triangle.Count);
            int count = 0;
            if (checker.Contains(triangle[i]))
            {
                count++;
            }
            if (checker.Contains(triangle[ i+1]))
            {
                count++;
            }
            if (checker.Contains(triangle[ i+2]))
            {
                count++;
            }
            if (count == 3)
            {
                triangle.RemoveRange(3 * i, 3);
                Debug.Log("in delete face");
                break;
            }
        }
        faces.Remove(face);
    }
    public void Extrude(Vector3 start,Vector3 end)
    {
        Vector3 normal = end - start;
        Face face = new Face();
        int count = 0;
        Vector3 newVertPosition = new Vector3();
        for (int i = 0; i < faces.Count; i++)
        {
            count++;
            if (faces[i].center == start)
            {
                Debug.Log("------------------------can find the extrude start point");
                face = faces[i];
                break;
            }

        }
        Debug.Log("the start point is" + face.center);
        int index = verts.Count; //get the current number of vert we have;
                                 // Debug.Log("verts number is" + index);
                                 //add verts
        verts.Add(end); //add vert;
        addDot(end);
        int[] f1 = { face.points[0], face.points[1], index };
        int[] f2 = { face.points[1], face.points[2], index };
        int[] f3 = { face.points[2], face.points[0], index };
        GeneFace(f1);
        GeneFace(f2);
        GeneFace(f3);
        DeleteFace(face);
        //refresh the mesh
        Vector3[] vertices = verts.ToArray();
        int[] triangles = triangle.ToArray();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        MeshUtility.Optimize(mesh);
        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;


    }
}
