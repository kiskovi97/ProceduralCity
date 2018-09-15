using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Box: Shape
{
    public int material;
    public Vector3 LeftDownFront;
    public Vector3 dir01;
    public Vector3 dir02;
    public Vector3 dir03;
    public Box(Vector3 LeftDownFront, Vector3 dir01, Vector3 dir02, Vector3 dir03, int material)
    {
        this.material = material;
        this.LeftDownFront = LeftDownFront;
        this.dir01 = dir01;
        this.dir02 = dir02;
        this.dir03 = dir03;
    }
    public List<Triangle> getTriangles()
    {
        List<Triangle> list = new List<Triangle>();
        list.Add(new Triangle(LeftDownFront + dir01 + dir03, LeftDownFront, LeftDownFront + dir01, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) }));
        list.Add(new Triangle(LeftDownFront, LeftDownFront + dir03 + dir01, LeftDownFront + dir03, material, new Vector2[] { new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0) }));
        list.Add(new Triangle(LeftDownFront, LeftDownFront + dir02 + dir03, LeftDownFront + dir02, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) }));
        list.Add(new Triangle(LeftDownFront + dir03 + dir02, LeftDownFront, LeftDownFront + dir03, material, new Vector2[] { new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0) }));

        list.Add(new Triangle(LeftDownFront + dir02, LeftDownFront + dir01 + dir02 + dir03, LeftDownFront + dir02 + dir01, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) }));
        list.Add(new Triangle(LeftDownFront + dir01 + dir02 + dir03, LeftDownFront + dir02, LeftDownFront + dir02 + dir03, material, new Vector2[] { new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0) }));
        list.Add(new Triangle(LeftDownFront + dir01 + dir02 + dir03, LeftDownFront + dir01, LeftDownFront + dir02 + dir01, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) }));
        list.Add(new Triangle(LeftDownFront + dir01, LeftDownFront + dir01 + dir02 + dir03, LeftDownFront + dir01 + dir03, material, new Vector2[] { new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0) }));


        list.Add(new Triangle(LeftDownFront + dir01 + dir02 + dir03, LeftDownFront+ dir03, LeftDownFront + dir01 + dir03, material, new Vector2[] { new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) }));
        list.Add(new Triangle(LeftDownFront + dir03, LeftDownFront + dir01 + dir02 + dir03, LeftDownFront + dir02 + dir03, material, new Vector2[] { new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0) }));

        return list;
    }
}
