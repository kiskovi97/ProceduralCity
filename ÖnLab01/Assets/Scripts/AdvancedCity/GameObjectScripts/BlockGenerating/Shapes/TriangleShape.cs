using UnityEngine;

public class TriangleShape : IShape
{
    public int material;
    public Vector3 A;
    public Vector3 B;
    public Vector3 C;
    public TriangleShape(Vector3 A, Vector3 B, Vector3 C, int material)
    {
        this.material = material;
        this.A = A;
        this.B = B;
        this.C = C;
    }
    public Triangle[] GetTriangles()
    {
        Triangle[] list = new Triangle[]
        {
            new Triangle(A, B, C, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) })
        };
        return list;
    }
}
