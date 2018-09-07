using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CrossingMesh : MeshElementImpl
{
    public CrossingMesh(List<Vector3> controlpoints, int mat)
    {
        shapes.Add(new Polygon(controlpoints, mat));
    }
}
