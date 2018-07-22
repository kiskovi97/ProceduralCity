using System.Collections.Generic;
using UnityEngine;

class Building : GeneratorImpl
{
   public Building(List<Vector3> kontrolpoints, float floor, int floorNumber, int megHouse = 3, bool ground = true)
    {
        if (floorNumber < 1) return;
        for (int i = 0; i < kontrolpoints.Count; i++)
        {
            int elozo = i - 1;
            if (elozo < 0) elozo = kontrolpoints.Count - 1;
            FrontWall frontWall = new FrontWall(kontrolpoints[elozo], kontrolpoints[i], floor, floorNumber, ground);
            generators.Add(frontWall);
        }
        float magasMax = floorNumber * floor;
        if (ground)
        {
            magasMax += floor * 1.5f;
        }
        MakeRoof(kontrolpoints.ToArray(), new Vector3(0, magasMax, 0));
        if (megHouse > 0)
        {
            List<Vector3> newControlPoints = new List<Vector3>();
            newControlPoints.AddRange(kontrolpoints);
            Vector3 kozeppont = new Vector3(0,0,0);
            for (int i=0; i< newControlPoints.Count; i++)
            {
                kozeppont += newControlPoints[i];
            }
            kozeppont = kozeppont * (1.0f / newControlPoints.Count);
            for (int i = 0; i < newControlPoints.Count; i++)
            {
                newControlPoints[i] += (kozeppont - newControlPoints[i])*(1.0f/5.0f);
                newControlPoints[i] += new Vector3(0, magasMax, 0);
            }
            generators.Add(new Building(newControlPoints, floor, floorNumber/2, megHouse-1, false));
        }
    }

    private void MakeRoof(Vector3[] roofPoints, Vector3 up)
    {
        List<Vector3> controlPoints = new List<Vector3>();
        controlPoints.AddRange(roofPoints);
        for (int i = 0; i < controlPoints.Count; i++)
        {
            controlPoints[i] += up;
        }
        elements.Add(new Roof(controlPoints));
    }
}