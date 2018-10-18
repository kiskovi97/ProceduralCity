using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BuildingObject : GeneratedObjectWithCollision
{
    protected Vector3[] kontrolpoints;
    protected int floorNumber;
    protected float floor;
    public Material SimpleMaterial;
    public Material RoofMaterial;
    public Material SimpleWallMaterial;
    public Material GroundWallMaterial;
    public Material DoorMaterial;
    public Material Window01Material;
    public Material Window02Material;
    public Material Window03Material;
    public Material BaseMaterial;
    // Update is called once per frame
    public virtual void MakeBuilding(Vector3[] kontrolpoints, int floorNumber, float floor)
    {
        this.kontrolpoints = kontrolpoints;
        this.floorNumber = floorNumber;
        this.floor = floor;
        Material[] myMaterials = new Material[9]
        {
            SimpleMaterial,
            RoofMaterial,
            SimpleWallMaterial,
            GroundWallMaterial,
            DoorMaterial,
            Window01Material,
            Window02Material,
            Window03Material,
            BaseMaterial
        };
        GetComponent<MeshRenderer>().sharedMaterials = myMaterials;
        ReGenerate();
    }

    public virtual void ReGenerate()
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
