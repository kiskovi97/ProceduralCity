using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

class Roof : MeshElementImpl
{
    static int MATERIAL = 1;
    static int MATERIALSIMPLEWALL = 2;
    public Roof(List<Vector3> controlpoints, bool last)
    {
        shapes.Add(new Polygon(controlpoints, MATERIALSIMPLEWALL));
        if (last)
        {
            List<Vector3> roofOne = new List<Vector3>();
            Vector3 tetopontA = (controlpoints[0] + controlpoints[1]) / 2 + new Vector3(0, 0.3f, 0);
            Vector3 tetopontB = (controlpoints[3] + controlpoints[2]) / 2 + new Vector3(0, 0.3f, 0);
            Vector3 kozeppont = (tetopontA + tetopontB) / 2;
            //tetopontA = (tetopontA * 3 + kozeppont) / 4;
            //tetopontB = (tetopontB * 3 + kozeppont) / 4;
            roofOne.Add(controlpoints[1]);
            roofOne.Add(controlpoints[2]);
            roofOne.Add(tetopontB);
            roofOne.Add(tetopontA);
            shapes.Add(new Polygon(roofOne, MATERIAL));
            List<Vector3> roofTwo = new List<Vector3>();
            roofTwo.Add(tetopontA);
            roofTwo.Add(tetopontB);
            roofTwo.Add(controlpoints[3]);
            roofTwo.Add(controlpoints[0]);
            shapes.Add(new Polygon(roofTwo, MATERIAL));
            shapes.Add(new TriangleShape(controlpoints[3], tetopontB, controlpoints[2], MATERIALSIMPLEWALL));
            shapes.Add(new TriangleShape(controlpoints[1], tetopontA, controlpoints[0], MATERIALSIMPLEWALL));
        }
        
    }
}