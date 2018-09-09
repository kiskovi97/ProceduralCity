using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CollisionSide : MeshElementImpl
{
    public CollisionSide(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU, int mat)
    {
        shapes.Add(new Rectangle(LD,LU,RD,RU, mat));
    }
}
