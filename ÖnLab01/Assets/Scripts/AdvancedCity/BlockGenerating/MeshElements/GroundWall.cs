using UnityEngine;

class GroundWall : MeshElementImpl
{
    public GroundWall(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        shapes.Add(new ReactRectangle(LD, RD, LU, RU, (int)BlockMaterial.GROUNDWALL));
    }
}
