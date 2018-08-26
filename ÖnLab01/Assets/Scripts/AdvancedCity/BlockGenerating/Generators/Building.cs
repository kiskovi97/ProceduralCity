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
        MakeRoof(kontrolpoints.ToArray(), new Vector3(0, magasMax, 0), (megHouse <= 0 || (floorNumber/2) < 1));
        if (megHouse > 0)
        {
            List<Vector3> newControlPoints = new List<Vector3>();
            newControlPoints.AddRange(kontrolpoints);
            int indexStart = (int) (Random.value * newControlPoints.Count);
            int index2 = indexStart + 1;
            int index3 = indexStart + 2;
            int index4 = indexStart + 3;
            if (index4 >= newControlPoints.Count)
            {
                index4 = index4 - newControlPoints.Count;
                if (index3 >= newControlPoints.Count)
                {
                    index3 = index3 - newControlPoints.Count;
                    if (index2 >= newControlPoints.Count)
                    {
                        index2 = index2 - newControlPoints.Count;
                    }
                 }
            }
            newControlPoints[index2] = (newControlPoints[index2] * 3 + newControlPoints[indexStart]) / 4;
            newControlPoints[index3] = (newControlPoints[index3] * 3 + newControlPoints[index4]) / 4;
            for (int i = 0; i < newControlPoints.Count; i++)
            {
                newControlPoints[i] += new Vector3(0, magasMax, 0);
            }
            generators.Add(new Building(newControlPoints, floor, floorNumber/2, megHouse-1, false));
        }
    }

    private void MakeRoof(Vector3[] roofPoints, Vector3 up, bool last)
    {
        List<Vector3> controlPoints = new List<Vector3>();
        controlPoints.AddRange(roofPoints);
        for (int i = 0; i < controlPoints.Count; i++)
        {
            controlPoints[i] += up;
        }
        elements.Add(new Roof(controlPoints, last));
    }
}