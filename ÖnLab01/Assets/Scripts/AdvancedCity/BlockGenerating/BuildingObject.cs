﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BuildingObject : MonoBehaviour {
    MeshFilter meshfilter;
    Mesh mesh;
    List<Vector3> meshVertexes;
    List<List<int>> subTriangles;
    List<Vector2> UV;
	// Use this for initialization
	void Start () {
        meshfilter = GetComponent<MeshFilter>();
        mesh = meshfilter.mesh;
        meshVertexes = new List<Vector3>();
        UV = new List<Vector2>();
        subTriangles = new List<List<int>>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        int MateraialCount = meshRenderer.materials.Length;
        for (int i=0; i< MateraialCount; i++)
        {
            subTriangles.Add(new List<int>());
        }
    }

    // Update is called once per frame
    public void MakeBuilding(List<Vector3> kontrolpoints, int min, int max, float floor)
    {
        Start();
        int floorNumber = (int)(Random.value * (max - min)) + min;
        float magasMax = floorNumber * floor;
        Building building = new Building(kontrolpoints, floor, floorNumber);
        foreach (Triangle triangle in building.getTriangles())
        {
            AddTriangle(triangle);
        }
        CreateMesh();
    }

    private void AddTriangle(Triangle triangle)
    {
        if (subTriangles.Count <= triangle.material)
        {
            Debug.Log("Need material : " + triangle.material);
            return;
        }
        Matrix4x4 matrix = gameObject.transform.worldToLocalMatrix;
        Vector3 to = transform.position;
        if (triangle.uvs.Length < 3) return;
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.A - to));
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.B - to));
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.C - to));
        UV.Add(triangle.uvs[0]);
        UV.Add(triangle.uvs[1]);
        UV.Add(triangle.uvs[2]);
    }

    void CreateMesh()
    {
        mesh.Clear();
        if (meshVertexes.Count < 3)
        {
            Destroy(this.gameObject, 0.1f);
            return;
        }
        mesh.vertices = meshVertexes.ToArray();
        mesh.subMeshCount = subTriangles.Count;
        for (int i = 0; i < subTriangles.Count; i++)
        {
            mesh.SetTriangles(subTriangles[i].ToArray(), i);
        }
        mesh.SetUVs(0, UV);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        //meshfilter.mesh = mesh;
    }
    public void DestorySelf()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
