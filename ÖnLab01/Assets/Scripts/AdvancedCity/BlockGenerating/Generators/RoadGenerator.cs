using System.Collections.Generic;
using UnityEngine;

class RoadGenerator : GeneratorImpl
{
    public RoadGenerator(Vector3 s1, Vector3 s2, Vector3 k1, Vector3 k2, int savok, bool tram, bool sideway)
    {
        if (sideway)
        {
            elements.Add(new RoadMesh(s1, s2, k1, k2, 0));
            return;
        }
        float db = savok * 2;
        for (int i=0; i< db; i++)
        {
            bool forditva = false;
            int mat = (i != db / 2 && i != db / 2 - 1) ? 1 : 2;
            if (i == 0 || i == db - 1) { forditva = !forditva; mat = 2;}
            if ((forditva && i>=savok) || (i < savok && !forditva))
            {
                elements.Add(new RoadMesh(
                    (s2 * i + k2 * (db - i)) / (db),
                    (s1 * i + k1 * (db - i)) / (db),
                    (s2 * (i + 1) + k2 * (db - i - 1)) / (db),
                    (s1 * (i + 1) + k1 * (db - i - 1)) / (db), mat, forditva));
            } else
            {
                elements.Add(new RoadMesh(
                    (s1 * (i + 1) + k1 * (db - i - 1)) / (db),
                    (s2 * (i + 1) + k2 * (db - i - 1)) / (db),
                    (s1 * i + k1 * (db - i)) / (db),
                    (s2 * i + k2 * (db - i)) / (db), mat, forditva));
            }
        }
    }
}