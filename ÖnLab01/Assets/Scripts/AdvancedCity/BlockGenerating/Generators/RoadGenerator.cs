using System.Collections.Generic;
using UnityEngine;

class RoadGenerator : GeneratorImpl
{
    public RoadGenerator(Vector3 inputS1, Vector3 inputS2, Vector3 inputK1, Vector3 inputK2, int savok, bool tram, bool sideway, float inputZebra)
    {
        float zebra = inputZebra / 2;
        Vector3 dir = (inputS1 - inputS2).normalized;
        Vector3 s1 = inputS1 - dir * zebra;
        Vector3 s2 = inputS2 + dir * zebra;
        Vector3 k1 = inputK1 - dir * zebra;
        Vector3 k2 = inputK2 + dir * zebra;
        if (sideway)
        {
            elements.Add(new RoadMesh(s1, s2, k1, k2, 0));
            return;
        }
        else
        {
            elements.Add(new RoadMesh(inputK1, inputS1, k1, s1, 3));
            elements.Add(new RoadMesh(inputS2, inputK2, s2, k2, 3));
        }
        float db = savok * 2;
        for (int i = 0; i < db; i++)
        {
            bool forditva = false;
            int mat = (i != db / 2 && i != db / 2 - 1) ? 1 : 2;
            if (i == 0 || i == db - 1) { forditva = !forditva; mat = 2; }
            if ((forditva && i >= savok) || (i < savok && !forditva))
            {
                elements.Add(new RoadMesh(
                    (s2 * i + k2 * (db - i)) / (db),
                    (s1 * i + k1 * (db - i)) / (db),
                    (s2 * (i + 1) + k2 * (db - i - 1)) / (db),
                    (s1 * (i + 1) + k1 * (db - i - 1)) / (db), mat, forditva));
            }
            else
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