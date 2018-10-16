using UnityEngine;
using System.Collections.Generic;

class RoofMesh : MeshElementImpl
{
    static readonly int MATERIAL = (int)BlockMaterial.ROOF;
    static readonly int MATERIALSIMPLEWALL = (int)BlockMaterial.SIMPLE;
    static readonly int MATERIALSIDEWALK = (int)BlockMaterial.BASE;
    public RoofMesh(Vector3[] controlpoints, bool last)
    {
        if (last)
        {
            Vector3[] innerRing = MyMath.InnerPoints(controlpoints, 0.03f);
            Vector3[] upControl = UpList(controlpoints, 0.05f);
            Vector3[] upRing = UpList(innerRing, 0.05f);
            shapes.Add(new RingShape(controlpoints, upControl, MATERIALSIMPLEWALL));
            shapes.Add(new RingShape(upControl, upRing, MATERIALSIMPLEWALL));
            shapes.Add(new RingShape(upRing, innerRing, MATERIALSIMPLEWALL));
            shapes.Add(new Polygon(innerRing, MATERIALSIMPLEWALL));
            int i = 3;
            while (i > 0)
            {
                i--;
                if (Random.value > 0.7f) break;
                Vector3 kp = controlpoints[1];
                Vector3 up = new Vector3(0, Random.value * 0.2f, 0);
                Vector3 left = controlpoints[2] - kp;
                Vector3 right = controlpoints[0] - kp;
                left = left / 2;
                right = right / 2;
                kp = kp + left / 2 + right / 2;
                kp = kp + Random.value * left;
                kp = kp + Random.value * right;
                left = left / (Random.value * 2.0f + 3);
                right = right / (Random.value * 2.0f + 3);
                shapes.Add(new BoxShape(kp, left, right, up, MATERIALSIMPLEWALL));
            }
        }
        else
        {
            shapes.Add(new Polygon(controlpoints, MATERIALSIMPLEWALL));
        }
    }

    private Vector3[] UpList(Vector3[] controlpoints, float up)
    {
        Vector3[] output = new Vector3[controlpoints.Length];
        for (int i = 0; i < controlpoints.Length; i++)
        {
            output[i] = controlpoints[i] + new Vector3(0, up, 0);
        }
        return output;
    }
}