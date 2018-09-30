using UnityEngine;

class GroundFrontWallGenerator : GeneratorImpl
{
    public GroundFrontWallGenerator(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU, int windowCount, float windowSize)
    {
        Vector3 centerD = (LD + RD) / 2;
        Vector3 centerU = (LU + RU) / 2;
        Vector3 leftToRight = (RD - LD).normalized;
        Vector3 leftDoorDown = centerD - leftToRight * (windowSize / 2);
        Vector3 rightDoorDown = centerD + leftToRight * (windowSize / 2);
        Vector3 leftDoorUp = centerU - leftToRight * (windowSize / 2);
        Vector3 rightDoorUp = centerU + leftToRight * (windowSize / 2);
        Vector3 upToDown = (LD - LU).normalized;
        Vector3 centerLeft = LU + upToDown * windowSize;
        Vector3 centerRight = RU + upToDown * windowSize;

        if ((LD-RD).magnitude > windowSize)
        {
            meshElements.Add(new Door(leftDoorDown, leftDoorUp, rightDoorDown, rightDoorUp));
            Vector3 centerCenter = centerU + upToDown * windowSize;
            Vector3 centerLeftDoor = centerCenter - leftToRight * (windowSize / 2);
            Vector3 centerRightDoor = centerCenter + leftToRight * (windowSize / 2);
            meshElements.Add(new GroundWall(LD, centerLeft, leftDoorDown, centerLeftDoor));
            meshElements.Add(new GroundWall(rightDoorDown, centerRightDoor, RD, centerRight));
            if (windowCount % 2 == 0)
            {
                windowCount = windowCount - 1;
            }
            generators.Add(new FloorWallGenerator(centerLeft, LU, centerLeftDoor, leftDoorUp, windowCount / 2, windowSize));
            generators.Add(new FloorWallGenerator(centerRightDoor, rightDoorUp, centerRight, RU, windowCount / 2, windowSize));
        } else
        {
            meshElements.Add(new GroundWall(LD, centerLeft, RD, centerRight));
            generators.Add(new FloorWallGenerator(centerLeft,LU,centerRight,RU,1,windowSize));
        }
        
    }
}