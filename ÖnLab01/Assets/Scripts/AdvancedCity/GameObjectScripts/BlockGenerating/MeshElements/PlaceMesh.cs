using UnityEngine;

class PlaceMesh : MeshElementImpl
{
    public PlaceMesh(Vector3[] controlpoints)
    {
        shapes.Add(new Polygon(controlpoints, 0));
    }
}
