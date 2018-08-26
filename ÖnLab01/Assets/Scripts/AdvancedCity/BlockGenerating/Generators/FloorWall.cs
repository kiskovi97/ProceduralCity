using UnityEngine;
using UnityEditor;

class FloorWall : GeneratorImpl
{
    public FloorWall(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU, int windowCount, float windowSize)
    {
        if (windowSize > (LD - RD).magnitude)
        {
            elements.Add(new SimpleWall(LD, LU, RD, RU));
            return;
        }
        int i = 0;
        Vector3 leftToRight = (RD - LD).normalized;
        Vector3 width = leftToRight * windowSize;
        float fullLenght = (LD - RD).magnitude;
        float leftOver = fullLenght - (windowCount * windowSize);
        elements.Add(new SimpleWall(LD, LU, LD + leftToRight * (leftOver / 2), LU + leftToRight * (leftOver / 2)));
        LD = LD + leftToRight * (leftOver / 2);
        LU = LU + leftToRight * (leftOver / 2);
        while (i < windowCount)
        {
            elements.Add(new Window(LD + width * i, LU + width * i, LD + width * (i + 1), LU + width * (i + 1)));
            i++;
        }
        elements.Add(new SimpleWall(RU,RD,RU- leftToRight * (leftOver / 2), RD - leftToRight * (leftOver / 2)));
    }
}