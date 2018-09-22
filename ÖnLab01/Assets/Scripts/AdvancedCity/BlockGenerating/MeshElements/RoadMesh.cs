using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class RoadMesh : MeshElementImpl
{
    public RoadMesh(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU, int mat, bool backwoard=false)
    {
        shapes.Add(new ReactRectangleSide(LD, RD, LU, RU, mat, backwoard));
    }
}