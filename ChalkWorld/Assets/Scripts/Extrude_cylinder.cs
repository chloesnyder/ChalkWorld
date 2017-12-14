using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Extrude_cylinder : MonoBehaviour {

    // Use this for initialization
    public List<GameObject> dots = new List<GameObject>();
    public GameObject myDots;
    public Color origColor;
    public Color origOutline;
    bool feetColliding;
    float timer = 2.0f;
    void Start () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
       // Debug.Log("size of vertices is" + mesh.vertexCount);
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            if ((Mathf.Abs(mesh.vertices[i].x)>0.41f&&Mathf.Abs(mesh.vertices[i].z)< 0.1f) || (Mathf.Abs(mesh.vertices[i].z) > 0.41f && Mathf.Abs(mesh.vertices[i].x) < 0.1f))
            {
              //  Debug.Log("vertices is" + mesh.vertices[i]);
                addDot(mesh.vertices[i]);
            }
        }

        origColor = GetComponent<Renderer>().material.color;
        origOutline = GetComponent<Renderer>().material.GetColor("_OutlineColor");
    }

    public void Die()
    {
		GameObject obj = GameObject.Find("GlobalObject");
		Global g = obj.GetComponent<Global>();
		g.addCylinder ();
        Destroy(gameObject);
        for (int i = 0; i < dots.Count; i++)
        {
            Destroy(dots[i]);
        }

        for (int i = 0; i < g.lines.Capacity; i++)
        {
            Destroy(g.lines[i]);
        }

    }
    private void addDot(Vector3 point)
    {
        GameObject obj = Instantiate(myDots, transform.TransformPoint(point), transform.rotation) as GameObject;
        obj.transform.parent = this.transform;
        obj.GetComponent<DotController>().cube = gameObject;
       // obj.transform.localScale *= 0.5f;
        dots.Add(obj);
    }
    // Update is called once per frame
    void Update () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        Material m_Material;
        m_Material = GetComponent<Renderer>().material;

        

       if (feetColliding)
        {
            timer -= Time.deltaTime;
            // Debug.Log("the material is" + m_Material.color);
            m_Material.color = Color.green;
            m_Material.SetColor("_OutlineColor", Color.green);
            if (timer <= 0)
            {
                feetColliding = false;
                timer = 2.0f;
            }

        }
        else
        {
            m_Material.color = origColor;
            m_Material.SetColor("_OutlineColor", origOutline);
        }

    
        
    }

    public void changeColor()
    {
        feetColliding = true;
    }
}
