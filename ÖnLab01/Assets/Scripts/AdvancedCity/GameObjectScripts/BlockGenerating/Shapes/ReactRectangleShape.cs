using UnityEngine;
using System.Collections.Generic;

public class ReactRectangleShape : IShape
{
    public int material;
    public Vector3 LeftDown;
    public Vector3 LeftUp;
    public Vector3 RightUp;
    public Vector3 RightDown;
    public ReactRectangleShape(Vector3 LeftDown, Vector3 RightDown, Vector3 LeftUp, Vector3 RightUp, int material)
    {
        this.material = material;
        this.LeftDown = LeftDown;
        this.LeftUp = LeftUp;
        this.RightUp = RightUp;
        this.RightDown = RightDown;
    }
    public Triangle[] GetTriangles()
    {
        float sizeUD = (LeftDown - LeftUp).magnitude;
        float sizeLR = (LeftDown - RightDown).magnitude;
        float sizeULR = (LeftUp - RightUp).magnitude;
        Triangle[] list = new Triangle[]
        {
            new Triangle(LeftDown, RightUp, LeftUp, material, new Vector2[] { new Vector2(0, 0), new Vector2(sizeULR / sizeUD, 1), new Vector2(0, 1) }),
            new Triangle(RightUp, LeftDown, RightDown, material, new Vector2[] { new Vector2(sizeULR / sizeUD, 1), new Vector2(0, 0), new Vector2(sizeLR / sizeUD, 0) })
        };
        return list;
    }
}