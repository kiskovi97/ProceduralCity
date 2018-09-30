using System.Collections.Generic;
using UnityEngine;

class CrossingGenerator : GeneratorImpl
{
    public CrossingGenerator(Vector3[] kontrolpoints)
    {
        meshElements.Add(new CrossingMesh(kontrolpoints));
    }
}