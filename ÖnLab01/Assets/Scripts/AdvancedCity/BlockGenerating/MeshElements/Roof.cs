using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

class Roof : MeshElementImpl
{
    static int MATERIAL = 1;
    public Roof(List<Vector3> controlpoints)
    {
        shapes.Add(new Polygon(controlpoints, MATERIAL));
    }
}