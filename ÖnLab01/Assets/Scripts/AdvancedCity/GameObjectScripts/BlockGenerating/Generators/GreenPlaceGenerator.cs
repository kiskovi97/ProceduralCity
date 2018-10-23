using System.Collections.Generic;
using UnityEngine;

class GreenPlaceGenerator : GeneratorImpl
{
    public GreenPlaceGenerator(Vector3[] kontrolpoints)
    {
        Vector3[] points = new Vector3[kontrolpoints.Length];
        for (int i=0; i< kontrolpoints.Length; i++)
        {
            points[i] = (kontrolpoints[i] + new Vector3(0,-0.1f,0));
        }
        meshElements.Add(new PlaceMesh(points));
    }
}