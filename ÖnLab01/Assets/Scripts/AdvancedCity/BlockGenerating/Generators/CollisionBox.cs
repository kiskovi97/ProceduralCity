using System.Collections.Generic;
using UnityEngine;

class CollisionBox : GeneratorImpl
{
    public CollisionBox(List<Vector3> kontrolpoints)
    {
        Vector3 up = new Vector3(0, 5, 0);
        List<Vector3> magas = new List<Vector3>();
        for (int i=0; i< kontrolpoints.Count; i++)
        {
            int j = i + 1;
            if (j > kontrolpoints.Count - 1) j = 0;
            elements.Add(new CollisionSide(kontrolpoints[i], kontrolpoints[i] + up, kontrolpoints[j], kontrolpoints[j] + up, 0));
            magas.Add(kontrolpoints[i] + up);
        }
        elements.Add(new CollisionTop(kontrolpoints,0));
        elements.Add(new CollisionTop(magas, 0));
    }
}