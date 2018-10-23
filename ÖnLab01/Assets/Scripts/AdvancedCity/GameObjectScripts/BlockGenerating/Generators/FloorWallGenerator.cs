using UnityEngine;

class FloorWallGenerator : GeneratorImpl
{
    public FloorWallGenerator(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU, int windowCount, float windowSize)
    {
        if (windowSize > (LD - RD).magnitude)
        {
            meshElements.Add(new SimpleWallMesh(LD, LU, RD, RU));
            return;
        }
        Vector3 leftToRight = (RD - LD).normalized;
        Vector3 width = leftToRight * windowSize;
        float leftOver = (LD - RD).magnitude - (windowCount * windowSize);
        meshElements.Add(new SimpleWallMesh(LD, LU, LD + leftToRight * (leftOver / 2), LU + leftToRight * (leftOver / 2)));
        LD = LD + leftToRight * (leftOver / 2);
        LU = LU + leftToRight * (leftOver / 2);
        for (int i=0; i< windowCount; i++)
        {
            meshElements.Add(new WindowMesh(LD + width * i, LU + width * i, LD + width * (i + 1), LU + width * (i + 1)));
        }
        meshElements.Add(new SimpleWallMesh(RU,RD,RU- leftToRight * (leftOver / 2), RD - leftToRight * (leftOver / 2)));
    }
}