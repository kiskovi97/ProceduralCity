using UnityEngine;
using System.Collections.Generic;

class Roof : MeshElementImpl
{
    static int MATERIAL = (int)MaterialEnum.ROOF;
    static int MATERIALSIMPLEWALL = (int)MaterialEnum.BASE;
    public Roof(List<Vector3> controlpoints, bool last)
    {
        shapes.Add(new Polygon(controlpoints, 0));
        if (controlpoints[0].y > 2) return;
        if (last && controlpoints.Count == 4) {
            Vector3 tetopontA = (controlpoints[0] + controlpoints[1]) / 2 + new Vector3(0, 0.3f, 0);
            Vector3 tetopontB = (controlpoints[3] + controlpoints[2]) / 2 + new Vector3(0, 0.3f, 0);
            shapes.Add(new ReactRectangle(controlpoints[1], controlpoints[2],   tetopontA, tetopontB, MATERIAL));
            shapes.Add(new ReactRectangle(controlpoints[3], controlpoints[0],  tetopontB, tetopontA, MATERIAL));
            shapes.Add(new TriangleShape(controlpoints[3], tetopontB, controlpoints[2], MATERIALSIMPLEWALL));
            shapes.Add(new TriangleShape(controlpoints[1], tetopontA, controlpoints[0], MATERIALSIMPLEWALL));
        }
        else if (last && controlpoints.Count == 6)
            {
                Vector3 tetopontA = (controlpoints[1] + controlpoints[2]) / 2 + new Vector3(0, 0.3f, 0);
                Vector3 tetopontB = (controlpoints[0] + controlpoints[3]) / 2 + new Vector3(0, 0.3f, 0);
                shapes.Add(new ReactRectangle(controlpoints[2], controlpoints[3], tetopontA, tetopontB, MATERIAL));
                shapes.Add(new ReactRectangle(tetopontA, tetopontB, controlpoints[1], controlpoints[0], MATERIAL));
                shapes.Add(new TriangleShape(controlpoints[0], tetopontB, controlpoints[3], MATERIALSIMPLEWALL));
                shapes.Add(new TriangleShape(controlpoints[2], tetopontA, controlpoints[1], MATERIALSIMPLEWALL));
                tetopontA = (controlpoints[0] + controlpoints[3]) / 2 + new Vector3(0, 0.3f, 0);
                tetopontB = (controlpoints[5] + controlpoints[4]) / 2 + new Vector3(0, 0.3f, 0);
                shapes.Add(new ReactRectangle(tetopontB, tetopontA, controlpoints[4], controlpoints[3], MATERIAL));
                shapes.Add(new ReactRectangle(controlpoints[5], controlpoints[0], tetopontB, tetopontA, MATERIAL));
                shapes.Add(new TriangleShape(controlpoints[5], tetopontB, controlpoints[4], MATERIALSIMPLEWALL));
                shapes.Add(new TriangleShape(controlpoints[3], tetopontA, controlpoints[0], MATERIALSIMPLEWALL));
            } else if (last)
        {
            Vector3 tetopontA = (controlpoints[0] + controlpoints[1]) / 2 + new Vector3(0, 0.3f, 0);
            Vector3 tetopontB = (controlpoints[0] + controlpoints[4]) / 2 + new Vector3(0, 0.3f, 0);
            Vector3 center = (controlpoints[0] + controlpoints[1] + controlpoints[2] + controlpoints[3] + controlpoints[4]) / 5 + new Vector3(0, 0.3f, 0);
            shapes.Add(new TriangleShape(controlpoints[0], tetopontA, center, MATERIAL));
            shapes.Add(new TriangleShape(controlpoints[0], center, tetopontB,  MATERIAL));
            shapes.Add(new ReactRectangle(controlpoints[1], controlpoints[2], tetopontA, center, MATERIAL));
            shapes.Add(new ReactRectangle(tetopontB, center,  controlpoints[4], controlpoints[3], MATERIAL));
            shapes.Add(new TriangleShape(controlpoints[3], center, controlpoints[2], MATERIAL));
            shapes.Add(new TriangleShape(controlpoints[0], controlpoints[1], tetopontA, MATERIALSIMPLEWALL));
            shapes.Add(new TriangleShape(controlpoints[4], controlpoints[0], tetopontB, MATERIALSIMPLEWALL));
            List<Vector3> roofOne = new List<Vector3>();
            roofOne.Add(controlpoints[2]);
            roofOne.Add(controlpoints[3]);
            roofOne.Add(tetopontB);
            roofOne.Add(tetopontA);
            //shapes.Add(new Polygon(roofOne, MATERIAL));
        }
    }
}