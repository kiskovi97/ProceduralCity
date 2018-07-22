using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class MeshElementImpl : MeshElement
{
    protected List<Shape> shapes = new List<Shape>();
    public List<Triangle> getTriangles()
    {
        List<Triangle> triangles = new List<Triangle>();
        foreach( Shape shape in shapes)
        {
            triangles.AddRange(shape.getTriangles());
        }
        return triangles;
    }
}

