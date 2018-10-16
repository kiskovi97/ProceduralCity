using System.Collections.Generic;
using UnityEngine;

class RoadGenerator : GeneratorImpl
{
    public RoadGenerator(Vector3 inputS1, Vector3 inputS2, Vector3 inputK1, Vector3 inputK2, int bandCount, bool sideway, float zebra1, float zebra2)
    {
        Vector3 dir = (inputS1 - inputS2).normalized;
        Vector3 s1 = inputS1 - dir * zebra1 / 2;
        Vector3 s2 = inputS2 + dir * zebra2 / 2;
        Vector3 k1 = inputK1 - dir * zebra1 / 2;
        Vector3 k2 = inputK2 + dir * zebra2 / 2;
        if (sideway)
        {
            meshElements.Add(new SideWalkMesh(s1, s2, k1, k2));
            return;
        }
        else
        {
            if (zebra1 > 0) meshElements.Add(new ZebraMesh(inputK1, inputS1, k1, s1));
            if (zebra2 > 0) meshElements.Add(new ZebraMesh(inputS2, inputK2, s2, k2));
        }
        float db = bandCount * 2;
        for (int i = 0; i < db; i++)
        {
            bool mirror = false;
            bool cloasingLine = (i != db / 2 && i != db / 2 - 1);
            if (i == 0 || i == db - 1) cloasingLine = false;
            if (i >= bandCount) mirror = !mirror;
            if (i == db / 2 || i == db / 2 - 1) { mirror = !mirror;}
            if (db == 2) { mirror = !mirror;}

            if (mirror)
            {
                meshElements.Add(new RoadMesh(
                      (s2 * i + k2 * (db - i)) / (db),
                      (s1 * i + k1 * (db - i)) / (db),
                      (s2 * (i + 1) + k2 * (db - i - 1)) / (db),
                      (s1 * (i + 1) + k1 * (db - i - 1)) / (db), cloasingLine, mirror));
            }
            else
            {
                meshElements.Add(new RoadMesh(
                    (s1 * (i + 1) + k1 * (db - i - 1)) / (db),
                    (s2 * (i + 1) + k2 * (db - i - 1)) / (db),
                    (s1 * i + k1 * (db - i)) / (db),
                    (s2 * i + k2 * (db - i)) / (db), cloasingLine, mirror));
            }
        }
    }
}