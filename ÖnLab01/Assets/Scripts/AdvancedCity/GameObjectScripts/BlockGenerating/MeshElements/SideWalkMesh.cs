using UnityEngine;

class SideWalkMesh : MeshElementImpl
{
    public SideWalkMesh(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        shapes.Add(new ReactRectangleSideShape(LD, RD, LU, RU, (int)RoadMaterial.SIDEWALK));
    }
}