using UnityEngine;

class CollisionSideMesh : MeshElementImpl
{
    public CollisionSideMesh(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        shapes.Add(new RectangleShape(LD, LU, RD, RU, 0));
    }
}
