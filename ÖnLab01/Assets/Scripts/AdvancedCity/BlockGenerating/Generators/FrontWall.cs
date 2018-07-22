using System;
using System.Collections.Generic;
using UnityEngine;

class FrontWall : GeneratorImpl
{
    public FrontWall(Vector3 elozo, Vector3 kovetkezo, float floor, int floorNumber, bool ground)
    {
        int windowCount = (int)((elozo - kovetkezo).magnitude / floor);
        if (windowCount < 1)
        {
            windowCount = 1;
        }
        if (ground)
        {
            GroundFrontWall groundFrontWall = new GroundFrontWall(
                elozo,
                elozo + new Vector3(0, floor * 1.5f, 0),
                kovetkezo,
                kovetkezo + new Vector3(0, floor * 1.5f, 0), windowCount, floor);
            generators.Add(groundFrontWall);
            elozo += new Vector3(0, floor * 1.5f, 0);
            kovetkezo += new Vector3(0, floor * 1.5f, 0);
        }
        

        for (int j = 1; j <= floorNumber; j++)
        {
            float magas = floor * j;
            float magaselozo = floor * (j - 1);
            FloorWall frontWall = new FloorWall(
                elozo + new Vector3(0, magaselozo, 0),
                elozo + new Vector3(0, magas, 0),
                kovetkezo + new Vector3(0, magaselozo, 0),
                kovetkezo + new Vector3(0, magas, 0), windowCount, floor);
            generators.Add(frontWall);
        }
    }
}
