using UnityEngine;

public class RectangleShape : IShape
{
    public int material;
    public Vector3 LeftDown;
    public Vector3 LeftUp;
    public Vector3 RightUp;
    public Vector3 RightDown;
    public RectangleShape(Vector3 LeftDown, Vector3 RightDown, Vector3 LeftUp, Vector3 RightUp, int material)
    {
        this.material = material;
        this.LeftDown = LeftDown;
        this.LeftUp = LeftUp;
        this.RightUp = RightUp;
        this.RightDown = RightDown;
    }
    public Triangle[] GetTriangles()
    {
        Triangle[] list = new Triangle[]
        {
            new Triangle(LeftDown, RightUp, LeftUp, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) }),
            new Triangle(RightUp, LeftDown, RightDown, material, new Vector2[] { new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0) })
        };
        return list;
    }
}
