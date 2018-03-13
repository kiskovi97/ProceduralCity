﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class BlockObjectScript : MonoBehaviour {
    Mesh mesh;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

	}
    
    List<Vector3> vertexes = new List<Vector3>();
    List<int> triangles = new List<int>();
    public void MakeMeshData(List<Vector3> loading, Vector3 kozeppont)
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertexes.AddRange(loading);
        vertexes.Add(kozeppont);
        for (int i = 0; i < vertexes.Count - 2; i++)
        {
            triangles.Add(i + 1);
            triangles.Add(i);
            triangles.Add(vertexes.Count - 1);
        }
        triangles.Add(0);
        triangles.Add(vertexes.Count - 2);
        triangles.Add(vertexes.Count - 1);
        int elso = vertexes.Count;
        float magassag = Random.value;
        for (int i = 0; i < elso; i++)
        {
            vertexes.Add(vertexes[i] + new Vector3(0,1,0)*magassag);
        }
        for (int i = 0; i < elso -2; i++)
        {
            triangles.Add(i + elso);
            triangles.Add(i);
            triangles.Add(i + 1);


            triangles.Add(i + elso);
            triangles.Add(i + 1);
            triangles.Add(i + elso + 1);

            triangles.Add(i + elso);
            triangles.Add(i + elso + 1);
            triangles.Add(elso + elso - 1);
        }

        triangles.Add(elso + elso - 2); 
        triangles.Add(elso - 2); 
        triangles.Add(0); 

        triangles.Add(elso + elso - 2);
        triangles.Add(0);
        triangles.Add(elso);

        triangles.Add(vertexes.Count - 2);
        triangles.Add(elso);
        triangles.Add(vertexes.Count - 1);
    }
    public void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertexes.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
