using System.Collections.Generic;
using UnityEngine;

class SepcialBuildingGenerator : GeneratorImpl
{
    CollisionBoxGenerator box = null;
    BuildingGenerator next = null;
    public SepcialBuildingGenerator(Vector3[] controlpoints, float floorSize, int floorCount)
    {
        if (floorCount < 1) return;
        float height = floorCount * floorSize;
        meshElements.Add(new RoofMesh(controlpoints, true));
        box = new CollisionBoxGenerator(controlpoints, height); 
    }

    public Triangle[] GetCollision()
    {
        List<Triangle> triangles = new List<Triangle>();
        if (box != null) triangles.AddRange(box.getTriangles());
        if (next != null) triangles.AddRange(next.GetCollision());
        return triangles.ToArray();
    }
}