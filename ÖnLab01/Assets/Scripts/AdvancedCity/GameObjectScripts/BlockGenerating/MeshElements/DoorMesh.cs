using UnityEngine;

class DoorMesh : MeshElementImpl
{
    public DoorMesh(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        Vector3 inner = Vector3.Cross((LD - LU), (LD - RD)) / 2;
        Vector3 LCenter = (2 * LD + LU) / 3;
        Vector3 RCenter = (2 * RD + RU) / 3;
        shapes.Add(new ReactRectangleShape(LCenter, LCenter + inner, LU, LU + inner, (int)BlockMaterial.SIMPLEWALL));
        shapes.Add(new ReactRectangleShape(LD, LD + inner, LCenter, LCenter + inner, (int)BlockMaterial.GROUNDWALL));
        shapes.Add(new ReactRectangleShape(RU, RU + inner, RCenter, RCenter + inner, (int)BlockMaterial.SIMPLEWALL));
        shapes.Add(new ReactRectangleShape(RCenter, RCenter + inner, RD, RD + inner, (int)BlockMaterial.GROUNDWALL));
        shapes.Add(new ReactRectangleShape(LU, LU + inner, RU, RU + inner, (int)BlockMaterial.SIMPLEWALL));
        shapes.Add(new RectangleShape(LD + inner, RD + inner, LU + inner, RU + inner, (int)BlockMaterial.DOOR));
    }
}
