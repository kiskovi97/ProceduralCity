using UnityEngine;

class FrontWallGenerator : GeneratorImpl
{
    public FrontWallGenerator(Vector3 before, Vector3 next, float floor, int floorNumber, bool ground)
    {
        int windowCount = (int)((before - next).magnitude / floor);
        if (windowCount < 1)
        {
            windowCount = 0;
        }
        if (ground)
        {
            GroundFrontWallGenerator groundFrontWall = new GroundFrontWallGenerator(
                before, before + new Vector3(0, floor * 1.5f, 0),
                next, next + new Vector3(0, floor * 1.5f, 0), windowCount, floor);
            generators.Add(groundFrontWall);
            before += new Vector3(0, floor * 1.5f, 0);
            next += new Vector3(0, floor * 1.5f, 0);
        } 
        for (int j = 1; j <= floorNumber; j++)
        {
            float nextHeight = floor * j;
            float prevHeight = floor * (j - 1);
            FloorWallGenerator frontWall = new FloorWallGenerator(
                before + new Vector3(0, prevHeight, 0),
                before + new Vector3(0, nextHeight, 0),
                next + new Vector3(0, prevHeight, 0),
                next + new Vector3(0, nextHeight, 0), windowCount, floor);
            generators.Add(frontWall);
        }
    }
}
