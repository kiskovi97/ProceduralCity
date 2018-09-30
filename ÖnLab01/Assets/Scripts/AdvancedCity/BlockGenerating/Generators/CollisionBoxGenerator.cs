using System.Collections.Generic;
using UnityEngine;

class CollisionBoxGenerator : GeneratorImpl
{
    public CollisionBoxGenerator(Vector3[] kontrolpoints, float upFloat)
    {
        Vector3 up = new Vector3(0, upFloat, 0);
        Vector3[] magas = (Vector3[]) kontrolpoints.Clone();
        for (int i = 0; i < kontrolpoints.Length; i++)
        {
            int j = i - 1;
            if (j < 0) j = kontrolpoints.Length - 1;
            meshElements.Add(new CollisionSideMesh(kontrolpoints[i], kontrolpoints[i] + up, kontrolpoints[j], kontrolpoints[j] + up));
            magas[i] += up;
        }
        meshElements.Add(new CollisionTopMesh(kontrolpoints));
        meshElements.Add(new CollisionTopMesh(magas));
    }
}