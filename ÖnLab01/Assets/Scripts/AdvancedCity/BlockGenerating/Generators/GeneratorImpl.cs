using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class GeneratorImpl : Generator
{
    protected List<MeshElement> elements = new List<MeshElement>();
    protected List<Generator> generators = new List<Generator>();
    public List<Triangle> getTriangles()
    {
        List<Triangle> triangles = new List<Triangle>();
        foreach(MeshElement element in elements)
        {
            triangles.AddRange(element.getTriangles());
        }
        foreach(Generator generator in generators)
        {
            triangles.AddRange(generator.getTriangles());
        }
        return triangles;
    }
}
