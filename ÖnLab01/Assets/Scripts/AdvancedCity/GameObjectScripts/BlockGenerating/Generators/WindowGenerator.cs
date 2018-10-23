using UnityEngine;
using UnityEditor;

class WindowGenerator : GeneratorImpl
{
    private readonly float R1 = 0.2f;
    private readonly float R2 = 0.8f;
    private readonly float D1 = 0.2f;
    private readonly float D2 = 0.6f;
    private Vector3 LU;
    private Vector3 down;
    private Vector3 right;
    private Vector3 inward;
    public WindowGenerator(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        this.LU = LU;
        down = LD - LU;
        right = RU - LU;
        meshElements.Add(new SimpleWallMesh(LU + down*D1, LU, RU + down*D1, RU));
        meshElements.Add(new SimpleWallMesh(LD , LU + down * D2, RD, RU + down * D2));
        meshElements.Add(new SimpleWallMesh(Real(D2, 0), Real(D1, 0), Real(D2, R1), Real(D1, R1)));
        meshElements.Add(new SimpleWallMesh(Real(D2, R2), Real(D1, R2), Real(D2, 1), Real(D1, 1)));
        inward = Vector3.Cross(right, down).normalized * 0.01f;
        meshElements.Add(new SimpleWallMesh(Real(D2, R1), Real(D1, R1), Real(D2, R1) + inward, Real(D1, R1) + inward));
        meshElements.Add(new SimpleWallMesh(Real(D1, R1), Real(D1, R2), Real(D1, R1) + inward, Real(D1, R2) + inward));
        meshElements.Add(new SimpleWallMesh(Real(D1, R2), Real(D2, R2), Real(D1, R2) + inward, Real(D2, R2) + inward));
        meshElements.Add(new SimpleWallMesh(Real(D2, R2), Real(D2, R1), Real(D2, R2) + inward, Real(D2, R1) + inward));
        meshElements.Add(new WindowMesh(Real(D2, R1) + inward, Real(D1, R1) + inward, Real(D2, R2) + inward, Real(D1, R2) + inward));
    }

    private Vector3 Real(float i , float j)
    {
        return LU + down * i + right * j;
    }
}