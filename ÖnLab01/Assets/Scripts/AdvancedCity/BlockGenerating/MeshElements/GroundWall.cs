using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class GroundWall : MeshElementImpl
{
    static int MATERIAL = 3;
    public GroundWall(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        shapes.Add(new ReactRectangle(LD, RD, LU, RU, MATERIAL));
    }
}
