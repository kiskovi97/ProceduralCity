using UnityEngine;
using System.Collections.Generic;

class Polygon : IShape
{
    public List<Vector3> controlPoints;
    public List<Triangle> ears = new List<Triangle>();
    public Polygon(Vector3[] inputControlPoints, int material)
    {
        if (inputControlPoints.Length < 3) return;
        controlPoints = new List<Vector3>();
        controlPoints.AddRange(inputControlPoints);
        int max = 100;
        int i = 0;
        while (controlPoints.Count > 3 && max > 0)
        {
            max--;
            if (i >= controlPoints.Count) i = 0;
            int prev = i - 1;
            int next = i + 1;
            if (prev < 0) prev = controlPoints.Count - 1;
            if (next >= controlPoints.Count) next = 0;
            if (MyMath.IsEar(i, controlPoints.ToArray()))
            {
                Vector3 Atmp = controlPoints[prev];
                Vector3 Btmp = controlPoints[i];
                Vector3 Ctmp = controlPoints[next];
                ears.Add(new Triangle(Atmp, Btmp, Ctmp, material,
                    new Vector2[] { new Vector2(Atmp.x, Atmp.z), new Vector2(Btmp.x, Btmp.z), new Vector2(Ctmp.x, Ctmp.z) }));
                controlPoints.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
        Vector3 A = controlPoints[0];
        Vector3 B = controlPoints[1];
        Vector3 C = controlPoints[2];
        ears.Add(new Triangle(A, B, C, material, new Vector2[] {
                    new Vector2(A.x,A.z),new Vector2(B.x,B.z),new Vector2(C.x,C.z)}));
        if (max <= 0)
        {
            if (controlPoints.Count > 3)
            {
                Vector3 Atmp = controlPoints[0];
                Vector3 Btmp = controlPoints[2];
                Vector3 Ctmp = controlPoints[3];
                ears.Add(new Triangle(Atmp, Btmp, Ctmp, material,
                    new Vector2[] { new Vector2(Atmp.x, Atmp.z), new Vector2(Btmp.x, Btmp.z), new Vector2(Ctmp.x, Ctmp.z) }));
            }
        }
    }

    public Triangle[] GetTriangles()
    {
        return ears.ToArray();
    }
}