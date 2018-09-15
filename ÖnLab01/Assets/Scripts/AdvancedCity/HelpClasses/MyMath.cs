using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MyMath
{
    public MyMath()
    {
    }

    public static Vector3 Intersect(Vector3 P, Vector3 V, Vector3 Q, Vector3 U)
    {
        float div = (U.x * V.z - U.z * V.x);
        if (Math.Abs(div) < 0.2f) return (P + Q) * 0.5f;
        float t2 = (Q.z * V.x + P.x * V.z - P.z * V.x - Q.x * V.z) / div;
        return (Q + t2 * U);
    }


    public static Vector3 Meroleges(Vector3 actual_point, Vector3 next_point)
    {
        Vector3 next_irany = (next_point - actual_point).normalized;
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        Vector3 meroleges = (rotation * next_irany).normalized;
        return meroleges;
    }

    public static List<Vector3> innerPoints(List<Vector3> controlpoints, float scale)
    {
        List<Vector3> outpout = new List<Vector3>();
        for(int i=0; i< controlpoints.Count; i++)
        {
            int j = i+1;
            if (j > controlpoints.Count - 1) j = 0;
            int z = i - 1;
            if (z < 0) z = controlpoints.Count - 1;
            Vector3 nextMer = Meroleges(controlpoints[i], controlpoints[j]).normalized * scale;
            Vector3 elozoMer = Meroleges(controlpoints[z], controlpoints[i]).normalized * scale;
            Vector3 kereszt = Intersect(controlpoints[i] + elozoMer, controlpoints[z] - controlpoints[i], controlpoints[i] + nextMer, controlpoints[i] - controlpoints[j]);
            outpout.Add(kereszt);
        }
        return outpout;
    }

    public static float Area(Vector3 a, Vector3 b, Vector3 c)
    {
        return Mathf.Abs((a.x * b.z + b.x * c.z + c.x * a.z
            - a.z * b.x - b.z * c.x - c.z * a.x) / 2.0f);
    }

    public static bool Between(Vector3 egyik, Vector3 masik, Vector3 kozotte)
    {
        Vector3 egyikFele = egyik - kozotte;
        Vector3 masikFele = masik - kozotte;
        float angle = Vector3.Angle(egyikFele, masikFele);
        if (egyik == kozotte || masik == kozotte) return false;
        return (angle > 179.9f) && (angle < 180.1f);
    }

    public static bool PrincipleVertex(Vector3 lineA, Vector3 lineB, List<Vector3> polygon)
    {
        int kereszt = 0;
        for (int i = 0; i < polygon.Count; i++)
        {
            if (lineA == polygon[i] || lineB == polygon[i]) continue;
            int j = i + 1;
            if (j > polygon.Count - 1) j = 0;
            if (lineA == polygon[j] || lineB == polygon[j]) continue;
            Vector3 intersect = Intersect(polygon[i], polygon[i] - polygon[j], lineA, lineA - lineB);
            if (Between(lineA, lineB, intersect) && Between(polygon[i], polygon[j], intersect)) kereszt++;
        }
        return kereszt == 0;
    }

    public static bool innerPoint(Vector3 a, List<Vector3> polygon)
    {
        int kereszt = 0;
        for (int i = 0; i < polygon.Count; i++)
        {
            if (a == polygon[i]) continue;
            int j = i + 1;
            if (j > polygon.Count - 1) j = 0;
            if (a == polygon[j]) continue;
            Vector3 intersect = Intersect(polygon[i], polygon[i] - polygon[j], a, new Vector3(0, 0, 1));
            if (Between(polygon[i], polygon[j], intersect) && Between(a,a+new Vector3(0,0,100),intersect)) kereszt++;
        }
        return kereszt % 2 == 1;
    }

    private static bool toTHeRight(Vector3 a, Vector3 egyik, Vector3 masik)
    {
        Vector3 egyikFele = egyik - a;
        Vector3 masikFele = masik - a;
        float angle = Vector3.SignedAngle(egyikFele, masikFele, new Vector3(0,1,0));
        return angle > 0;
    }

    public static bool isEar(int i, List<Vector3> polygon)
    {
        Vector3 point = polygon[i];
        int x = i - 1;
        int y = i + 1;
        if (y > polygon.Count - 1) y = 0;
        if (x < 0) x = polygon.Count - 1;
        if (innerPoint((polygon[x] + polygon[y]) / 2, polygon))
        {
            return PrincipleVertex(polygon[x], polygon[y], polygon);
        } else
        {
            return false;
        }
    }
}

