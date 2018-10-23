using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SpecialBuildingObject : BuildingObject
{
    public GameObject prefab;
    public Vector3 center;

    public override void ReGenerate()
    {
        Clear();
        GreenPlaceGenerator building = new GreenPlaceGenerator(kontrolpoints);
        center = new Vector3(0, 0, 0);
        foreach (Vector3 point in kontrolpoints)
        {
            center += point;
        }
        center /= kontrolpoints.Length;
        float max = 0;
        foreach (Triangle triangle in building.getTriangles())
        {
            //AddTriangle(triangle);
            if (triangle.A.y > max) max = triangle.A.y;
        }
        if (prefab != null && kontrolpoints.Length > 3)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.position = center;
            obj.transform.rotation = Quaternion.LookRotation(kontrolpoints[2] - kontrolpoints[1]);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.tag = "Generated";
            obj.transform.SetParent(transform);
        }
        CreateMesh();
    }

    public override void DestorySelf()
    {
        if (prefab == null)
            DestroyImmediate(gameObject);
    }
}
