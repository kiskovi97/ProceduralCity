using System.Collections.Generic;
using UnityEngine;

class GreenPlaceGenerator : GeneratorImpl
{
    public GreenPlaceGenerator(List<Vector3> kontrolpoints)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i=0; i< kontrolpoints.Count; i++)
        {
            points.Add(kontrolpoints[i] + new Vector3(0,-0.1f,0));
        }
        elements.Add(new PlaceMesh(points));
    }
}