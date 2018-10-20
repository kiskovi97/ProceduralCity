using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SpecialBuildingObject : BuildingObject
{
    public override void ReGenerate()
    {
        Clear();
        GreenPlaceGenerator building = new GreenPlaceGenerator(kontrolpoints);
        float max = 0;
        foreach (Triangle triangle in building.getTriangles())
        {
            //AddTriangle(triangle);
            if (triangle.A.y > max) max = triangle.A.y;
        }
        CreateMesh();
    }
}
