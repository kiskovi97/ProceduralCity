using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

class Polygon : Shape
{
    public List<Vector3> controlPoints;
    private int material;
    public Polygon(List<Vector3> inputControlPoints, int material)
    {
        this.controlPoints = inputControlPoints;
        this.material = material;
    }
    
    public List<Triangle> getTriangles()
    {
        List<Triangle> list = new List<Triangle>();
        if (controlPoints.Count >= 4)
        {
            list.AddRange(new Rectangle(controlPoints[0], controlPoints[1], controlPoints[3], controlPoints[2], material).getTriangles());
        }
        if (controlPoints.Count == 6)
        {
            list.AddRange(new Rectangle(controlPoints[0], controlPoints[3], controlPoints[5], controlPoints[4], material).getTriangles());
        }
        if (controlPoints.Count == 5)
        {
            list.Add(new Triangle(controlPoints[0], controlPoints[3], controlPoints[4], material, new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) }));
        }
        return list;
    }
}