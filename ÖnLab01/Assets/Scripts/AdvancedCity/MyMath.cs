using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class MyMath
    {
        public MyMath()
        {

        }
        public Vector3 Intersect(Vector3 P,Vector3 V,Vector3 Q,Vector3 U)
        {
            float div = (U.x * V.z - U.z * V.x);
            if (div == 0) return (P + Q) * 0.5f;
            float t2 = (Q.z * V.x + P.x * V.z - P.z * V.x - Q.x * V.z) / div;
            return (Q + t2 * U);

        }
    }
}
