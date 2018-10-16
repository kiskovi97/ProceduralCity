using System.Collections.Generic;

class GeneratorImpl : IGenerator
{
    protected List<IMeshElement> meshElements = new List<IMeshElement>();
    protected List<IGenerator> generators = new List<IGenerator>();
    public Triangle[] getTriangles()
    {
        List<Triangle> triangles = new List<Triangle>();
        foreach(IMeshElement element in meshElements)
        {
            triangles.AddRange(element.getTriangles());
        }
        foreach(IGenerator generator in generators)
        {
            triangles.AddRange(generator.getTriangles());
        }
        return triangles.ToArray();
    }
}
