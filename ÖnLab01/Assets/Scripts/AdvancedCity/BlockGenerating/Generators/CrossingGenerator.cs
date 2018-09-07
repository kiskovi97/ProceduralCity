using System.Collections.Generic;
using UnityEngine;

class CrossingGenerator : GeneratorImpl
{
    public CrossingGenerator(List<Vector3> kontrolpoints, int mat)
    {
        elements.Add(new CrossingMesh(kontrolpoints, mat));
    }
}