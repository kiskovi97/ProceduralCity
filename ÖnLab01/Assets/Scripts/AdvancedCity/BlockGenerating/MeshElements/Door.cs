using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Door : MeshElementImpl
{
    static int MATERIAL = 4;
    public Door(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        shapes.Add(new Rectangle(LD, RD, LU, RU, MATERIAL));
    }
}
