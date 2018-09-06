using UnityEngine;
using UnityEditor;

class WindowGenerator : GeneratorImpl
{
    private float R1 = 0.2f;
    private float R2 = 0.8f;
    private float D1 = 0.2f;
    private float D2 = 0.6f;
    private Vector3 LU;
    private Vector3 down;
    private Vector3 right;
    private Vector3 inward;
    public WindowGenerator(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        //elements.Add(new Window(LD , LU, RD, RU));
        this.LU = LU;
        down = LD - LU;
        right = RU - LU;
        elements.Add(new SimpleWall(LU + down*D1, LU, RU + down*D1, RU));
        elements.Add(new SimpleWall(LD , LU + down * D2, RD, RU + down * D2));
        elements.Add(new SimpleWall(real(D2, 0), real(D1, 0), real(D2, R1), real(D1, R1)));
        elements.Add(new SimpleWall(real(D2, R2), real(D1, R2), real(D2, 1), real(D1, 1)));
        inward = Vector3.Cross(right, down).normalized * 0.01f;

        elements.Add(new SimpleWall(real(D2, R1), real(D1, R1), real(D2, R1) + inward, real(D1, R1) + inward));
        elements.Add(new SimpleWall(real(D1, R1), real(D1, R2), real(D1, R1) + inward, real(D1, R2) + inward));
        elements.Add(new SimpleWall(real(D1, R2), real(D2, R2), real(D1, R2) + inward, real(D2, R2) + inward));
        elements.Add(new SimpleWall(real(D2, R2), real(D2, R1), real(D2, R2) + inward, real(D2, R1) + inward));

        elements.Add(new Window(real(D2, R1) + inward, real(D1, R1) + inward, real(D2, R2) + inward, real(D1, R2) + inward));
    }

    private Vector3 real(float i , float j)
    {
        return LU + down * i + right * j;
    }
}