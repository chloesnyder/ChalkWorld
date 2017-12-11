using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Extrude_cube : MonoBehaviour
{

    // Use this for initialization
	public List<GameObject> dots=new List<GameObject>();
	public GameObject myDots;

    bool feetColliding;
    float timer = 2.0f;
    public Color origColor;
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
		CreateSphere ();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
       Debug.Log("the size of normal at begain is"+ mesh.normals.Length) ;
        origColor = GetComponent<Renderer>().material.color;
        // Debug.Log("triangle number is" + mesh.triangles.Length);


        //  Vector3 start1 = new Vector3(0, 0.5f, 0);
        //  Vector3 end1 = new Vector3(0, 1.2f, 0);
        // Extrude(start1, end1);


        //  Debug.Log("the size of normal at extrude is" + mesh.normals.Length);
        /*
         * 
        start1 = new Vector3(0, 1.2f, 0);
        end1 = new Vector3(0, 2.2f, 0);
        Extrude(start1, end1);
        start1 = new Vector3(0.5f, 0, 0);
        end1 = new Vector3(1.5f, 0, 0);
        Extrude(start1, end1);
        */

    }

    // Update is called once per frame
    void Update()
    {
        Material m_Material;
        m_Material = GetComponent<Renderer>().material;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
     //   GetComponent<MeshCollider>().sharedMesh = mesh;

        if (feetColliding)
        {
            timer -= Time.deltaTime;
            // Debug.Log("the material is" + m_Material.color);
            m_Material.color = Color.green;
            if (timer <= 0)
            {
                feetColliding = false;
                timer = 2.0f;
            }

        }
        else
        {
            m_Material.color = origColor;
        }
    }
	private void CreateSphere()
	{
		Mesh mesh = GetComponent<MeshFilter>().mesh;

		//Debug.Log(mesh.vertices[0]);
		for (int i = 0; i < mesh.vertices.Length; i++)
		{


			//GameObject obj = Instantiate(myDots, transform.TransformPoint(mesh.vertices[i]), transform.rotation) as GameObject;
			//obj.GetComponent<DotController> ().cube = gameObject;
			//dots.Add(obj);
			addDot (mesh.vertices [i]);

		}
	}
	private void addDot(Vector3 point){
		GameObject obj = Instantiate(myDots, transform.TransformPoint(point), transform.rotation) as GameObject;
		obj.transform.parent = this.transform;
		obj.GetComponent<DotController> ().cube = gameObject;
		dots.Add(obj);
	}


	public void Die(){
		Destroy(gameObject);
		GameObject obj = GameObject.Find("GlobalObject");
		Global g = obj.GetComponent<Global>();
		for(int i=0;i<dots.Count;i++){
			Destroy(dots[i]);
		}

		for(int i = 0; i < g.lines.Capacity; i++)
		{
			Destroy(g.lines[i]);
		}

	}
    private void CreatPlane()
    {
        Vector3[] vertices = {
            new Vector3 (-0.5f, -0.5f, -0.5f), //0
            new Vector3 (0.5f, -0.5f, -0.5f),  //1
            new Vector3 (0.5f, 0.5f, -0.5f),    //2
            new Vector3 (-0.5f, 0.5f, -0.5f),   //3
            new Vector3 (-0.5f, 0.5f, 0.5f),    //4
            new Vector3 (0.5f, 0.5f, 0.5f),     //5
            new Vector3 (0.5f, -0.5f, 0.5f),    //6
            new Vector3 (-0.5f, -0.5f, 0.5f),   //7
        };
        for (int i = 0; i < 8; i++)
        {
            verts.Add(vertices[i]);
        }
        int[] triangles = {
            0, 2, 1, //face front
			0, 3, 2,

            2, 4, 5,
            2, 3, 4, //face top


			1, 5, 6,
            1, 2, 5, //face right
			

            0, 4, 3,
            0, 7, 4, //face left
			
            5, 7, 6,
            5, 4, 7, //face back
			
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
        faces.Add(new Face(new Vector3(0, 0.5f, 0), index2));
        int[] index3 = { 1, 6, 5, 2 };
        faces.Add(new Face(new Vector3(0.5f, 0, 0), index3));
        int[] index4 = { 0, 3, 4, 7 };
        faces.Add(new Face(new Vector3(-0.5f, 0, 0), index4));
        int[] index5 = {  5, 6, 7,4 };
        faces.Add(new Face(new Vector3(0, 0, 0.5f), index5));
        int[] index6 = { 0, 7, 6, 1 };
        faces.Add(new Face(new Vector3(0, -0.5f, 0), index6));
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
        int count = 0;
        for (int i = 0; i < faces.Count; i++)
        {
            count++;
            if (faces[i].center == start)
            {
               // Debug.Log("------------------------can find the extrude start point");
                face = faces[i];
               // break;
            }
         
        }



        Debug.Log("the start point real in extrude is" + face.center);
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
		addDot (v0);
		addDot (v1);
		addDot (v2);
		addDot (v3);
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
        
        /*
        int[] front = { face.points[0], face.points[3], index + 3, index };
        GeneFace(front);  //front face;

        int[] bank = { index + 2, face.points[2], face.points[1], index + 1 };
        GeneFace(bank);     //bank face;

        int[] right = { face.points[3], face.points[2], index + 2, index + 3 };
        GeneFace(right);   //right face;
        int[] left = { face.points[0], index, index + 1, face.points[1] };
        GeneFace(left);   //left face;
        int[] top = { index + 3, index + 2, index + 1, index };
        GeneFace(top);    //top face;
        */
    

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

    private void OnCollisionEnter(Collision collision)
    {
//Debug.Log("enter the collision!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!with tag" + collision.collider.gameObject.name);
       // if (collision.collider.gameObject.name.Equals("[VRTK][AUTOGEN][BodyColliderContainer]")|| collision.collider.gameObject.name.Equals("[VRTK][AUTOGEN][FootColliderContainer]"))
       // {
       //     Debug.Log("enter the collision!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
       //     feetColliding = true;
//}

  /*      Debug.Log("enter the collision!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!with tag" + collision.collider.gameObject.name);
        if (collision.collider.gameObject.CompareTag("feet"))
        {
         Debug.Log("enter the collision!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            feetColliding = true;
        }*/
    }

    private void OnCollisionExit(Collision collision)
    {
      /*  //collision.collider.CompareTag("feet")
        if (collision.gameObject.CompareTag("feet"))
        {
            feetColliding = false;
        }*/
    }

    public void changeColor()
    {
        feetColliding = true;
    }
}
