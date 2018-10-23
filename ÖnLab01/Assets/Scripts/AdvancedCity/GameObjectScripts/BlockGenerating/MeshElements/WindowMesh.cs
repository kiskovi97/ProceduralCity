using UnityEngine;

class WindowMesh : MeshElementImpl
{
    static int MATERIAL = 5;
    public WindowMesh(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        int plusz = (int)(Random.value * 3);
        shapes.Add(new RectangleShape(LD, RD, LU, RU, MATERIAL + plusz));
    }

}
