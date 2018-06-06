using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    class GameObjectGenerator : MonoBehaviour
    {
        public GameObject blockObject;
        public GameObject roadObject;
        List<Crossing> roads;
        List<List<Crossing>> circles;
        
        public void GenerateBlocks(List<Crossing> crossings)
        {
            circles = new List<List<Crossing>>();
            roads = crossings;
            if (roads == null)
            {
                Debug.Log("ERROR Not initializaled Roads");
                return ;
            }

            if (roads.Count <= 0) return;
            foreach (Crossing cros in roads)
            {
                List<Crossing> szomszedok = cros.getSzomszedok();
                foreach (Crossing second in szomszedok)
                {
                    GenerateCircle(cros, second, false);
                }
            }

            foreach (List<Crossing> circle in circles)
            {
                List<Vector3> controlPoints = new List<Vector3>();
                for (int i=0; i< circle.Count; i++)
                {
                    int x = i + 1;
                    if (x > circle.Count - 1) x = 0;
                    controlPoints.Add(circle[i].Kereszt(circle[x]));
                }
                GameObject real = Instantiate(blockObject);
                BlockObjectScript blockscript = real.GetComponent<BlockObjectScript>();
                controlPoints.Reverse();
                blockscript.GenerateBlockMesh(controlPoints);
            }
        }

        void GenerateCircle(Crossing root, Crossing second, bool jobbra)
        {
            if (second == null) return;

            List<Crossing> circle = new List<Crossing>();
            circle.Add(root);
            circle.Add(second);
            bool ok = true;
            int last = circle.Count - 1;
            while (ok)
            {
                Crossing nextroad = circle[last].Kovetkezo(circle[last - 1], jobbra);
                if (nextroad == null) return;
                if (nextroad == root) ok = false;
                else
                {
                    foreach (Crossing road in circle)
                    {
                        if (road == nextroad) return;
                    }
                    circle.Add(nextroad);
                    last++;
                }

            }
            if (circle.Count <= 2) return;
            ok = true;
            foreach (List<Crossing> eddigi in circles)
            {
                if (CircleEqual(eddigi, circle)) ok = false;
            }
            if (ok)
            {
                circles.Add(circle);
            }

        }
        bool CircleEqual(List<Crossing> egyik, List<Crossing> masik)
        {
            List<Crossing> hosszu = new List<Crossing>();
            hosszu.AddRange(masik);
            hosszu.AddRange(masik.GetRange(0, masik.Count - 1));
            int j = 0;
            bool van = false;
            for (int i = 0; i < hosszu.Count; i++)
            {
                if (hosszu[i] == egyik[j])
                {
                    j++;
                    if (j == egyik.Count) return true;
                    van = true;
                }
                if (hosszu[i] != egyik[j] && !van) continue;
            }
            if (j == egyik.Count) return true;
            return false;
        }
        public void CreateRoad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int mat)
        {
           GameObject road = Instantiate(roadObject);
            RoadPhysicalObject roadobj =  road.GetComponent<RoadPhysicalObject>();
            roadobj.GenerateBlockMesh(a, b, c, d, mat);
            roadobj.CreateMesh();
        }
    }
}
