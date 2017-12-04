using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Extrude_cylinder : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            Debug.Log("vertices is" + mesh.vertices[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
