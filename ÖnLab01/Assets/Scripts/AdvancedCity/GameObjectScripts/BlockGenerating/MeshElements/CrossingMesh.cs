using UnityEngine;

class CrossingMesh : MeshElementImpl
{
    public CrossingMesh(Vector3[] controlpoints)
    {
        shapes.Add(new Polygon(controlpoints, (int)RoadMaterial.CROSSING));
    }
}
