using System.Collections.Generic;

class MeshElementImpl : IMeshElement
{
    protected List<IShape> shapes = new List<IShape>();
    public Triangle[] getTriangles()
    {
        List<Triangle> triangles = new List<Triangle>();
        foreach( IShape shape in shapes)
        {
            triangles.AddRange(shape.GetTriangles());
        }
        return triangles.ToArray();
    }
}

