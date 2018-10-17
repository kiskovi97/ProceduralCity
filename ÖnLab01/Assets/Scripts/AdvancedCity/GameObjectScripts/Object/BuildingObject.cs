using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BuildingObject : GeneratedObjectWithCollision
{
    Vector3[] kontrolpoints;
    int floorNumber;
    float floor;
    // Update is called once per frame
    public void MakeBuilding(Vector3[] kontrolpoints, int floorNumber, float floor)
    {
        this.kontrolpoints = kontrolpoints;
        this.floorNumber = floorNumber;
        this.floor = floor;
        ReGenerate();
    }

    public void ReGenerate()
    {
        Clear();
        BuildingGenerator building = new BuildingGenerator(kontrolpoints, floor, floorNumber);
        float max = 0;
        foreach (Triangle triangle in building.getTriangles())
        {
            AddTriangle(triangle);
            if (triangle.A.y > max) max = triangle.A.y;
        }

        Triangle[] collision = building.GetCollision();
        foreach (Triangle triangle in collision)
        {
            AddTriangleCollision(triangle);
        }
        CreateMesh();
        CreateColliderMesh();
    }

    public void MakeBase(Vector3 a, Vector3 b, Vector3 c)
    {
        Clear();
        TriangleShape triangleShape = new TriangleShape(a, b, c, (int)BlockMaterial.BASE);
        foreach (Triangle triangle in triangleShape.GetTriangles())
        {
            AddTriangle(triangle);
        }
        CreateMesh();
    }
}
