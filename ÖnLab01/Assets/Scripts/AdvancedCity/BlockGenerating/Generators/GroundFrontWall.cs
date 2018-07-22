using UnityEngine;
using UnityEditor;

class GroundFrontWall : GeneratorImpl
{
    public GroundFrontWall(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU, int windowCount, float windowSize)
    {
        Vector3 centerD = (LD + RD) / 2;
        Vector3 centerU = (LU + RU) / 2;
        Vector3 leftToRight = (RD - LD).normalized;
        Vector3 LeftDoorDown = centerD - leftToRight * (windowSize / 2);
        Vector3 RightDoorDown = centerD + leftToRight * (windowSize / 2);
        Vector3 LeftDoorUp = centerU - leftToRight * (windowSize / 2);
        Vector3 RightDoorUp = centerU + leftToRight * (windowSize / 2);

        Vector3 upToDown = (LD - LU).normalized;
        Vector3 centerLeft = LU + upToDown * windowSize;
        Vector3 centerRight = RU + upToDown * windowSize;

        if ((LD-RD).magnitude > windowSize)
        {
            elements.Add(new Door(LeftDoorDown, LeftDoorUp, RightDoorDown, RightDoorUp));
            Vector3 centerCenter = centerU + upToDown * windowSize;
            Vector3 centerLeftDoor = centerCenter - leftToRight * (windowSize / 2);
            Vector3 centerRightDoor = centerCenter + leftToRight * (windowSize / 2);
            elements.Add(new GroundWall(LD, centerLeft, LeftDoorDown, centerLeftDoor));
            elements.Add(new GroundWall(RightDoorDown, centerRightDoor, RD, centerRight));
            if (windowCount % 2 == 0)
            {
                windowCount = windowCount - 1;
            }
            generators.Add(new FloorWall(centerLeft, LU, centerLeftDoor, LeftDoorUp, windowCount / 2, windowSize));
            generators.Add(new FloorWall(centerRightDoor, RightDoorUp, centerRight, RU, windowCount / 2, windowSize));
        } else
        {
            elements.Add(new GroundWall(LD, centerLeft, RD, centerRight));
            generators.Add(new FloorWall(centerLeft,LU,centerRight,RU,1,windowSize));
        }
        
    }
}