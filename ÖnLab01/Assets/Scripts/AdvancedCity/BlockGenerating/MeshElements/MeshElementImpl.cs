using System.Collections.Generic;

class MeshElementImpl : MeshElement
{
    protected List<Shape> shapes = new List<Shape>();
    public Triangle[] getTriangles()
    {
        List<Triangle> triangles = new List<Triangle>();
        foreach( Shape shape in shapes)
        {
            triangles.AddRange(shape.getTriangles());
        }
        return triangles.ToArray();
    }
}

