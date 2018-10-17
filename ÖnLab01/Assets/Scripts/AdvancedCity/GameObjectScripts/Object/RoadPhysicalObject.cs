using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RoadPhysicalObject : GeneratedObject
{
    public void CreateRoadMesh(Vector3 s1, Vector3 s2, Vector3 k1, Vector3 k2, int savok, bool tram, bool sideway, float zebra, float otherzebra)
    {
        Clear();
        AddRoadMesh(s1, s2, k1, k2, savok, tram, sideway, zebra, otherzebra);
    }
    public void AddRoadMesh(Vector3 s1, Vector3 s2, Vector3 k1, Vector3 k2, int savok, bool tram, bool sideway, float zebra, float otherzebra)
    {
        RoadGenerator roadGenerator = new RoadGenerator(s1, s2, k1, k2, savok, sideway, zebra, otherzebra);
        foreach (Triangle triangle in roadGenerator.getTriangles())
        {
            AddTriangle(triangle);
        }
    }
    public void CreateCrossingMesh(List<Vector3> pointList)
    {
        Clear();
        AddCrossingMesh(pointList);
    }

    public void AddCrossingMesh(List<Vector3> pointList)
    {
        CrossingGenerator crossing = new CrossingGenerator(pointList.ToArray());
        foreach (Triangle triangle in crossing.getTriangles())
        {
            AddTriangle(triangle);
        }
    }
}
