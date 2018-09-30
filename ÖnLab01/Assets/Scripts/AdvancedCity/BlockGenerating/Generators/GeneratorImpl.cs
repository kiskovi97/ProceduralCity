using System.Collections.Generic;

class GeneratorImpl : Generator
{
    protected List<MeshElement> meshElements = new List<MeshElement>();
    protected List<Generator> generators = new List<Generator>();
    public Triangle[] getTriangles()
    {
        List<Triangle> triangles = new List<Triangle>();
        foreach(MeshElement element in meshElements)
        {
            triangles.AddRange(element.getTriangles());
        }
        foreach(Generator generator in generators)
        {
            triangles.AddRange(generator.getTriangles());
        }
        return triangles.ToArray();
    }
}
