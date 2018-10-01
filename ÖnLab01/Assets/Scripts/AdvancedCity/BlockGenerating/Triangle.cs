using UnityEngine;

public class Triangle
{
    public int material;
    public Vector3 A;
    public Vector3 B;
    public Vector3 C;
    public Vector2[] uvs;
    public Triangle(Vector3 A, Vector3 B, Vector3 C, int material, Vector2[] uvs)
    {
        this.material = material;
        this.A = A;
        this.B = B;
        this.C = C;
        this.uvs = uvs;
    }
}
