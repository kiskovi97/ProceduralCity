using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ReactRectangle : Shape
{
    public int material;
    public Vector3 LeftDown;
    public Vector3 LeftUp;
    public Vector3 RightUp;
    public Vector3 RightDown;
    public ReactRectangle(Vector3 LeftDown, Vector3 RightDown, Vector3 LeftUp, Vector3 RightUp, int material)
    {
        this.material = material;
        this.LeftDown = LeftDown;
        this.LeftUp = LeftUp;
        this.RightUp = RightUp;
        this.RightDown = RightDown;
    }
    public List<Triangle> getTriangles()
    {
        List<Triangle> list = new List<Triangle>();
        float sizeUD = (LeftDown - LeftUp).magnitude;
        float sizeLR = (LeftDown - RightDown).magnitude;
        float sizeULR = (LeftUp - RightUp).magnitude;
        list.Add(new Triangle(LeftDown, RightUp, LeftUp, material, new Vector2[] { new Vector2(0, 0), new Vector2(sizeULR / sizeUD, 1), new Vector2(0, 1) }));
        list.Add(new Triangle(RightUp, LeftDown, RightDown, material, new Vector2[] { new Vector2(sizeULR / sizeUD, 1), new Vector2(0, 0), new Vector2(sizeLR / sizeUD, 0) }));
        return list;
    }
}