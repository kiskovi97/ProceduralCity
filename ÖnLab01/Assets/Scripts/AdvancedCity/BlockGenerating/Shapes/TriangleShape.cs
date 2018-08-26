using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TriangleShape : Shape
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
    public List<Triangle> getTriangles()
    {
        List<Triangle> list = new List<Triangle>();
        list.Add(new Triangle(A, B, C, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) }));
        return list;
    }
}
