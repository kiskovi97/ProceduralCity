using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SpecialBuildingObject : BuildingObject
{
    public override void ReGenerate()
    {
        Clear();
        SepcialBuildingGenerator building = new SepcialBuildingGenerator(kontrolpoints, floor, floorNumber);
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
}
