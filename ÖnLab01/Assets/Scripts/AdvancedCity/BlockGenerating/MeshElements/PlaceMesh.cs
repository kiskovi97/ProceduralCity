using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PlaceMesh : MeshElementImpl
{
    public PlaceMesh(List<Vector3> controlpoints)
    {
        shapes.Add(new Polygon(controlpoints, 0));
    }
}
