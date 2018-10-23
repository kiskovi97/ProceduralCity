using UnityEngine;
using System.Collections.Generic;

public class RingShape : IShape
{
    public int material;
    public Vector3[] innerRing;
    public Vector3[] outerRing;
    public RingShape(Vector3[] outerRing, Vector3[] innerRing, int material)
    {
        this.material = material;
        this.innerRing = innerRing;
        this.outerRing = outerRing;
    }
    public Triangle[] GetTriangles()
    {
        List<Triangle> list = new List<Triangle>();
        for (int i = 0; i < outerRing.Length; i++)
        {
            int j = i + 1;
            if (j > outerRing.Length - 1) j = 0;
            Vector3 LeftDown = outerRing[i];
            Vector3 LeftUp = innerRing[i];
            Vector3 RightUp = innerRing[j];
            Vector3 RightDown = outerRing[j];
            list.Add(new Triangle(LeftDown, RightUp, LeftUp, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) }));
            list.Add(new Triangle(RightUp, LeftDown, RightDown, material, new Vector2[] { new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0) }));
        }
        return list.ToArray();
    }
}
