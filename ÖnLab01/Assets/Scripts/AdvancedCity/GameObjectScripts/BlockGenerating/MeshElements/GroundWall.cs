using UnityEngine;

class GroundWallMesh : MeshElementImpl
{
    public GroundWallMesh(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        shapes.Add(new ReactRectangleShape(LD, RD, LU, RU, (int)BlockMaterial.GROUNDWALL));
    }
}
