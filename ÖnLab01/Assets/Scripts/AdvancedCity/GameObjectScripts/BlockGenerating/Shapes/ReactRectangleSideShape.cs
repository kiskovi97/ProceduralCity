using UnityEngine;
using System.Collections.Generic;

public class ReactRectangleSideShape : IShape
{
    public int material;
    public Vector3 LeftDown;
    public Vector3 LeftUp;
    public Vector3 RightUp;
    public Vector3 RightDown;
    public bool backward;
    public ReactRectangleSideShape(Vector3 LeftDown, Vector3 RightDown, Vector3 LeftUp, Vector3 RightUp, int material, bool backward = false)
    {
        this.material = material;
        this.LeftDown = LeftDown;
        this.LeftUp = LeftUp;
        this.RightUp = RightUp;
        this.RightDown = RightDown;
        this.backward = backward;
    }
    public Triangle[] GetTriangles()
    {
        List<Triangle> list = new List<Triangle>();
        float sizeUD = (LeftDown - LeftUp).magnitude;
        float sizeLR = (LeftDown - RightDown).magnitude;
        float sizeRUD = (RightDown - RightUp).magnitude * 0.3f;
        if (backward)
        {
            list.Add(new Triangle(LeftDown, RightUp, LeftUp, material, new Vector2[] { new Vector2(0, sizeRUD / sizeLR), new Vector2(1, 0), new Vector2(0, 0) }));
            list.Add(new Triangle(RightUp, LeftDown, RightDown, material, new Vector2[] { new Vector2(1, 0), new Vector2(0, sizeRUD / sizeLR), new Vector2(1, sizeRUD / sizeLR) }));
        } else
        {
            list.Add(new Triangle(LeftDown, RightUp, LeftUp, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, sizeRUD / sizeLR), new Vector2(0, sizeRUD / sizeLR) }));
            list.Add(new Triangle(RightUp, LeftDown, RightDown, material, new Vector2[] { new Vector2(1, sizeRUD / sizeLR), new Vector2(0, 0), new Vector2(1, 0) }));
        }
        return list.ToArray();
    }
}