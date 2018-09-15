using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Ring: Shape
{
    public int material;
    public List<Vector3> innerRing;
    public List<Vector3> outerRing;
    public Ring(List<Vector3> outerRing, List<Vector3> innerRing, int material)
    {
        this.material = material;
        this.innerRing = innerRing;
        this.outerRing = outerRing;
    }
    public List<Triangle> getTriangles()
    {
        List<Triangle> list = new List<Triangle>();
        for (int i=0; i< outerRing.Count; i++)
        {
            int j = i + 1;
            if (j > outerRing.Count - 1) j = 0;
            Vector3 LeftDown = outerRing[i];
            Vector3 LeftUp = innerRing[i];
            Vector3 RightUp = innerRing[j];
            Vector3 RightDown = outerRing[j];
            list.Add(new Triangle(LeftDown, RightUp, LeftUp, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) }));
            list.Add(new Triangle(RightUp, LeftDown, RightDown, material, new Vector2[] { new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0) }));
        }
        return list;
    }
}
