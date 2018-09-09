using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CollisionTop : MeshElementImpl
{
    public CollisionTop(List<Vector3> controlpoints, int mat)
    {
        shapes.Add(new Polygon(controlpoints, mat));
    }
}
