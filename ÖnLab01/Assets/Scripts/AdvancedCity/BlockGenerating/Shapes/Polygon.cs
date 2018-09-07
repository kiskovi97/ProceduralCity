using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

class Polygon : Shape
{
    public List<Vector3> controlPoints;
    public List<Triangle> ears = new List<Triangle>();
    private int material;
    public Polygon(List<Vector3> inputControlPoints, int material)
    {
        controlPoints = new List<Vector3>();
        controlPoints.AddRange(inputControlPoints);
        this.material = material;
        int max = 100;
        while (controlPoints.Count >= 4 && max > 0)
        {
            max--;
            if (MyMath.isEar(0, controlPoints))
            {
                Vector3 Atmp = controlPoints[controlPoints.Count - 1];
                Vector3 Btmp = controlPoints[0];
                Vector3 Ctmp = controlPoints[1];
                ears.Add(new Triangle(Atmp, Btmp, Ctmp, material, 
                    new Vector2[] {new Vector2(Atmp.x,Atmp.z),new Vector2(Btmp.x,Btmp.z),new Vector2(Ctmp.x,Ctmp.z)}));
                controlPoints.RemoveAt(0);
            } else {
                Vector3 tmp = controlPoints[0];
                controlPoints.RemoveAt(0);
                controlPoints.Add(tmp);
            }
        }
        Vector3 A = controlPoints[0];
        Vector3 B = controlPoints[1];
        Vector3 C = controlPoints[2];
        ears.Add(new Triangle(A, B, C, material, new Vector2[] {
                    new Vector2(A.x,A.z),new Vector2(B.x,B.z),new Vector2(C.x,C.z)}));
        if (max <= 0)
        {
            if (controlPoints.Count > 3)
            {
                Vector3 Atmp = controlPoints[0];
                Vector3 Btmp = controlPoints[2];
                Vector3 Ctmp = controlPoints[3];
                ears.Add(new Triangle(Atmp, Btmp, Ctmp, material,
                    new Vector2[] { new Vector2(Atmp.x, Atmp.z), new Vector2(Btmp.x, Btmp.z), new Vector2(Ctmp.x, Ctmp.z) }));
            }
            /*for (int i=0; i<controlPoints.Count; i++)
            {
                int j = i + 1;
                if (j > controlPoints.Count - 1) j = 0;
                Debug.DrawLine(controlPoints[i], controlPoints[j], Color.blue, 1000, false);
            }*/
        }
    }
    
    public List<Triangle> getTriangles()
    {
        return ears;
    }
}