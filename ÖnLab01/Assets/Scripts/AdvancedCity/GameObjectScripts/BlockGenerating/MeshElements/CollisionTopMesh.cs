using UnityEngine;

class CollisionTopMesh : MeshElementImpl
{
    public CollisionTopMesh(Vector3[] controlpoints)
    {
        shapes.Add(new Polygon(controlpoints, 0));
    }
}
