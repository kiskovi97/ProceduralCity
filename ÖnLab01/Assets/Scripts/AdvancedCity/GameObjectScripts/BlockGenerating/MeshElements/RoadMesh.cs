using UnityEngine;


class RoadMesh : MeshElementImpl
{
    public RoadMesh(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU, bool closingLine, bool backwoard = false)
    {
        int mat = closingLine ? (int)RoadMaterial.CLOSINGLINE : (int)RoadMaterial.DASHEDLINE;
        shapes.Add(new ReactRectangleSideShape(LD, RD, LU, RU, mat, backwoard));
    }
}