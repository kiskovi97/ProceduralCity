using UnityEngine;
using System.Collections.Generic;

class Roof : MeshElementImpl
{
    static int MATERIAL = (int)MaterialEnum.ROOF;
    static int MATERIALSIMPLEWALL = (int)MaterialEnum.SIMPLE;
    static int MATERIALSIDEWALK = (int)MaterialEnum.BASE;
    public Roof(List<Vector3> controlpoints, bool last)
    {
        if (last)
        {
            List<Vector3> innerRing = MyMath.innerPoints(controlpoints, 0.03f);
            List<Vector3> upControl = upList(controlpoints, 0.05f);
            List<Vector3> upRing = upList(innerRing, 0.05f);
            shapes.Add(new Ring(controlpoints, upControl, MATERIALSIMPLEWALL));
            shapes.Add(new Ring(upControl, upRing, MATERIALSIMPLEWALL));
            shapes.Add(new Ring(upRing, innerRing, MATERIALSIMPLEWALL));
            shapes.Add(new Polygon(innerRing, MATERIALSIMPLEWALL));
            int i = 3; 
            while (i> 0)
            {
                i--;
                if (Random.value > 0.7f) break;
                Vector3 kp = controlpoints[1];
                Vector3 up = new Vector3(0, Random.value*0.2f, 0);
                Vector3 left = controlpoints[2] - kp;
                Vector3 right = controlpoints[0] - kp;
                left = left / 2;
                right = right / 2;
                kp = kp + left / 2 + right / 2;
                kp = kp + Random.value * left;
                kp = kp + Random.value * right;
                left = left / (Random.value * 2.0f + 3);
                right = right / (Random.value * 2.0f + 3);
                shapes.Add(new Box(kp, left, right, up, MATERIALSIMPLEWALL));
            }
            
        } else
        {
            shapes.Add(new Polygon(controlpoints, MATERIALSIMPLEWALL));
        }
    }

    private List<Vector3> upList(List<Vector3> controlpoints, float up)
    {
        List<Vector3> output = new List<Vector3>();
        for (int i=0; i<controlpoints.Count; i++)
        {
            output.Add(controlpoints[i] + new Vector3(0,up,0));
        }
        return output;
    }

    private void TriangleRoof(List<Vector3> controlpoints, bool last)
    {
        shapes.Add(new Polygon(controlpoints, 0));
        if (controlpoints[0].y > 2) return;
        if (last && controlpoints.Count == 4)
        {
            Vector3 tetopontA = (controlpoints[0] + controlpoints[1]) / 2 + new Vector3(0, 0.3f, 0);
            Vector3 tetopontB = (controlpoints[3] + controlpoints[2]) / 2 + new Vector3(0, 0.3f, 0);
            shapes.Add(new ReactRectangle(controlpoints[1], controlpoints[2], tetopontA, tetopontB, MATERIAL));
            shapes.Add(new ReactRectangle(controlpoints[3], controlpoints[0], tetopontB, tetopontA, MATERIAL));
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
        }
        else if (last)
        {
            Vector3 tetopontA = (controlpoints[0] + controlpoints[1]) / 2 + new Vector3(0, 0.3f, 0);
            Vector3 tetopontB = (controlpoints[0] + controlpoints[4]) / 2 + new Vector3(0, 0.3f, 0);
            Vector3 center = (controlpoints[0] + controlpoints[1] + controlpoints[2] + controlpoints[3] + controlpoints[4]) / 5 + new Vector3(0, 0.3f, 0);
            shapes.Add(new TriangleShape(controlpoints[0], tetopontA, center, MATERIAL));
            shapes.Add(new TriangleShape(controlpoints[0], center, tetopontB, MATERIAL));
            shapes.Add(new ReactRectangle(controlpoints[1], controlpoints[2], tetopontA, center, MATERIAL));
            shapes.Add(new ReactRectangle(tetopontB, center, controlpoints[4], controlpoints[3], MATERIAL));
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