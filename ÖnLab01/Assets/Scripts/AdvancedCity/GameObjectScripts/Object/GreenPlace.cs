using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GreenPlace : GeneratedObject
{
    public void MakePlace(Vector3[] kontrolpoints)
    {
        Clear();
        GreenPlaceGenerator place = new GreenPlaceGenerator(kontrolpoints);
        foreach (Triangle triangle in place.getTriangles())
        {
            AddTriangle(triangle);
        }
        CreateMesh();
    }
}
