using System;
using System.Collections.Generic;
using UnityEngine;


public class MyMath
{
    public static Vector3 Intersect(Vector3 P, Vector3 V, Vector3 Q, Vector3 U)
    {
        V = To2D(V);
        U = To2D(U);
        float div = (U.x * V.z - U.z * V.x);
        if (Math.Abs(div) < 0.2f) return (P + Q) * 0.5f;
        float t2 = (Q.z * V.x + P.x * V.z - P.z * V.x - Q.x * V.z) / div;
        return (Q + t2 * U);
    }

    public static Vector3 To2D(Vector3 vector)
    {
        Vector3 ki = vector;
        ki.y = 0;
        return ki.normalized;
    }

    public static Vector3 Meroleges(Vector3 actualPoint, Vector3 nextPoint)
    {
        Vector3 next_irany = To2D(nextPoint - actualPoint).normalized;
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        Vector3 meroleges = To2D(rotation * next_irany).normalized;
        return meroleges;
    }

    public static Vector3[] InnerPoints(Vector3[] controlPoints, float scale)
    {
        List<Vector3> outpout = new List<Vector3>();
        for(int i=0; i< controlPoints.Length; i++)
        {
            int j = i+1;
            if (j > controlPoints.Length - 1) j = 0;
            int z = i - 1;
            if (z < 0) z = controlPoints.Length - 1;
            Vector3 nextMer = Meroleges(controlPoints[i], controlPoints[j]).normalized * scale;
            Vector3 elozoMer = Meroleges(controlPoints[z], controlPoints[i]).normalized * scale;
            Vector3 kereszt = Intersect(controlPoints[i] + elozoMer, controlPoints[z] - controlPoints[i], controlPoints[i] + nextMer, controlPoints[i] - controlPoints[j]);
            outpout.Add(kereszt);
        }
        return outpout.ToArray();
    }

    public static float Area(Vector3 a, Vector3 b, Vector3 c)
    {
        return Mathf.Abs((a.x * b.z + b.x * c.z + c.x * a.z
            - a.z * b.x - b.z * c.x - c.z * a.x) / 2.0f);
    }

    public static bool Between(Vector3 one, Vector3 other, Vector3 middle)
    {
        Vector3 egyikFele = one - middle;
        Vector3 masikFele = other - middle;
        float angle = Vector3.Angle(egyikFele, masikFele);
        if (one == middle || other == middle) return false;
        return (angle > 179.9f) && (angle < 180.1f);
    }

    public static bool PrincipleVertex(Vector3 lineA, Vector3 lineB, Vector3[] polygon)
    {
        int kereszt = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            if (lineA == polygon[i] || lineB == polygon[i]) continue;
            int j = i + 1;
            if (j > polygon.Length - 1) j = 0;
            if (lineA == polygon[j] || lineB == polygon[j]) continue;
            Vector3 intersect = Intersect(polygon[i], polygon[i] - polygon[j], lineA, lineA - lineB);
            if (Between(lineA, lineB, intersect) && Between(polygon[i], polygon[j], intersect)) kereszt++;
        }
        return kereszt == 0;
    }

    public static bool InnerPoint(Vector3 a, Vector3[] polygon)
    {
        int kereszt = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            if (a == polygon[i]) continue;
            int j = i + 1;
            if (j > polygon.Length - 1) j = 0;
            if (a == polygon[j]) continue;
            Vector3 intersect = Intersect(polygon[i], polygon[i] - polygon[j], a, new Vector3(0, 0, 1));
            if (Between(polygon[i], polygon[j], intersect) && Between(a,a+new Vector3(0,0,100),intersect)) kereszt++;
        }
        return kereszt % 2 == 1;
    }

    private static bool ToTheRight(Vector3 a, Vector3 one, Vector3 other)
    {
        Vector3 dir1 = one - a;
        Vector3 dir2 = other - a;
        float angle = Vector3.SignedAngle(dir1, dir2, new Vector3(0,1,0));
        return angle > 0;
    }

    public static bool IsEar(int i, Vector3[] polygon)
    {
        int prev = i - 1;
        int next = i + 1;
        if (next > polygon.Length - 1) next = 0;
        if (prev < 0) prev = polygon.Length - 1;
        if (InnerPoint((polygon[prev] + polygon[next]) / 2, polygon))
        {
            return PrincipleVertex(polygon[prev], polygon[next], polygon);
        } else
        {
            return false;
        }
    }
}

