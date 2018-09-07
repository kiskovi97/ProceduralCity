using System.Collections.Generic;
using UnityEngine;

class GreenPlaceGenerator : GeneratorImpl
{
    public GreenPlaceGenerator(List<Vector3> kontrolpoints)
    {
        elements.Add(new PlaceMesh(kontrolpoints));
    }
}