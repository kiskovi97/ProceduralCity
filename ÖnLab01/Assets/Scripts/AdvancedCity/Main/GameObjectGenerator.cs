using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Assets.Scripts.AdvancedCity
{
    [RequireComponent(typeof(RoadGeneratingValues))]
    class GameObjectGenerator : MonoBehaviour
    {
        public GameObject roadObject;
        public GameObject crossLamp;
        public GameObject sideLamp;
        public GameObject wireObject;
        public GameObject railObject;
        List<Crossing> roads;
        List<List<Crossing>> circles;
        BuildingContainer buildingContainer;

        public void AddLine(Vector3 a, Vector3 b, float scale)
        {
            GameObject wire = Instantiate(wireObject);
            wire.transform.position = (a + b) / 2;
            wire.transform.rotation = Quaternion.LookRotation(a - b);
            wire.transform.localScale = new Vector3(scale, scale, (a - b).magnitude * 50);
        }


        public void CreateRails(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int mat)
        {
            AddRail((a * 3 + c * 2) / 5, (b * 3 + d * 2) / 5, 0.4f);
            AddRail((a * 2 + c * 3) / 5, (b * 2 + d * 3) / 5, 0.4f);
        }

        private void AddRail(Vector3 a, Vector3 b, float scale)
        {
            GameObject rail = Instantiate(railObject);
            rail.transform.position = (a + b) / 2;
            rail.transform.rotation = Quaternion.LookRotation(a - b);
            rail.transform.localScale = new Vector3(scale, scale, (a - b).magnitude * 50);
        }


        public void CreateCrossing(List<Vector3> polygon, int mat)
        {
            GameObject road = Instantiate(roadObject);
            RoadPhysicalObject roadobj = road.GetComponent<RoadPhysicalObject>();
            roadobj.CreateCrossingMesh(polygon, mat);
            roadobj.CreateMesh();
        }

        public GameObject createCrossLamp(Vector3 position, Vector3 forward)
        {
            GameObject output = Instantiate(crossLamp);
            output.transform.position = position;
            output.transform.rotation = Quaternion.LookRotation(forward, new Vector3(0, 1, 0));
            return output;
        }

        public GameObject createSideLamp(Vector3 position, Vector3 forward)
        {
            GameObject output = Instantiate(sideLamp);
            output.transform.position = position;
            output.transform.rotation = Quaternion.LookRotation(forward, new Vector3(0, 1, 0));
            return output;
        }

        public void GenerateBlocks(List<Crossing> crossings)
        {
            circles = new List<List<Crossing>>();
            roads = crossings;
            if (roads == null)
            {
                Debug.Log("ERROR Not initializaled Roads");
                return;
            }
            buildingContainer = GetComponent<BuildingContainer>();
            if (buildingContainer == null)
            {
                Debug.Log("Need Building Container");
                return;
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
            StartCoroutine(GenerateFromCircles());

        }

        IEnumerator GenerateFromCircles()
        {
            foreach (List<Crossing> circle in circles)
            {
                List<Vector3> controlPoints = new List<Vector3>();
                for (int i = 0; i < circle.Count; i++)
                {
                    int x = i + 1;
                    if (x > circle.Count - 1) x = 0;
                    controlPoints.Add(circle[i].Kereszt(circle[x]));
                }
                BlockGenerator generator = new BlockGeneratorBasic();
                controlPoints.Reverse();
                RoadGeneratingValues values = GetComponent<RoadGeneratingValues>();
                if (values == null) throw new System.Exception("No Values Connected");
                generator.SetValues(values);
                generator.GenerateBuildings(controlPoints, buildingContainer);
                yield return new WaitForSeconds(0.2f);
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
            RoadPhysicalObject roadobj = road.GetComponent<RoadPhysicalObject>();
            roadobj.CreateRoadMesh(a, b, c, d, mat);
            roadobj.CreateMesh();
        }

    }
}
