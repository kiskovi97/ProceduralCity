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

    public static Vector3 Intersect(Vector3 P,Vector3 V,Vector3 Q,Vector3 U)
    {
            float div = (U.x * V.z - U.z * V.x);
            if (Math.Abs(div) <0.2f) return (P + Q) * 0.5f;
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

        public static float Area(Vector3 a, Vector3 b, Vector3 c)
        {
            return Mathf.Abs((a.x * b.z + b.x * c.z + c.x * a.z
                - a.z * b.x - b.z * c.x - c.z * a.x) / 2.0f);
        }

    public static bool Between(Vector3 egyik, Vector3 masik, Vector3 kozotte)
    {
        Vector3 egyikFele = egyik - kozotte;
        Vector3 masikFele = masik - kozotte;
        float angle  = Vector3.Angle(egyikFele, masikFele);
        return (angle > 179) && (angle < 181);
    }
}

