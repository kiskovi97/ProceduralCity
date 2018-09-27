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
        public GameObject stoppingObj;
        List<Crossing> roads;
        List<List<Crossing>> circles;
        BuildingContainer buildingContainer;
        RoadGeneratingValues values;

        public void SetValues(RoadGeneratingValues values)
        {
            this.values = values;
        }
        private Vector3 convert(Vector3 point)
        {
            float magassag = values.getTextureValue(point) *10;
            return new Vector3(point.x, magassag + point.y, point.z);
        }

        private List<Vector3> convert(List<Vector3> list)
        {
            List<Vector3> output = new List<Vector3>();
            for (int i = 0; i < list.Count; i++)
            {
                output.Add(convert(list[i]));
            }
            return output;
        }

        GameObject wireBase = null;

        public void AddStoppingMesh(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            a = convert(a);
            b = convert(b);
            c = convert(c);
            d = convert(d);
            GameObject obj = Instantiate(stoppingObj);
            obj.transform.position = (a + b + c + d) / 4;
            obj.transform.localScale = new Vector3((a - b).magnitude, 0.5f, (a - c).magnitude/2);
            obj.transform.rotation = Quaternion.LookRotation(a - c);
        }

        public void AddLine(Vector3 a, Vector3 b, float scale)
        {
            a = convert(a);
            b = convert(b);
            if (wireBase == null)
            {
                wireBase = Instantiate(new GameObject(), new Vector3(0, 0, 0), new Quaternion());
                wireBase.name = "WireBase";
            }
            GameObject wire = Instantiate(wireObject);
            wire.transform.position = (a + b) / 2;
            wire.transform.rotation = Quaternion.LookRotation(a - b);
            wire.transform.localScale = new Vector3(scale, scale, (a - b).magnitude * 50);
            wire.transform.parent = wireBase.transform;
        }
        public void CreateRails(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int mat)
        {
            AddRail((a * 5 + c * 3) / 8, (b * 5 + d * 3) / 8, 0.4f);
            AddRail((a * 3 + c * 5) / 8, (b * 3 + d * 5) / 8, 0.4f);
        }

        public void CreateRails(MovementPoint a, MovementPoint b, float size, Vector3 meroleges, int mat)
        {
            if (a == null || b == null) return;
            if (a == b) return;
            MovementPoint next = a.getNextPoint();
            if (next!=b)
            {
                CreateRails(next, b, size, meroleges, mat);
            }
            AddRail(a.center + meroleges * (size / 2.0f), next.center + meroleges * (size / 2.0f), 0.4f);
            AddRail(a.center - meroleges * (size / 2.0f), next.center - meroleges * (size / 2.0f), 0.4f);
        }
        private void AddRail(Vector3 a, Vector3 b, float scale)
        {
            a = convert(a);
            b = convert(b);
            if (wireBase == null)
            {
                wireBase = Instantiate(new GameObject(), new Vector3(0, 0, 0), new Quaternion());
                wireBase.name = "WireBase";
            }
            GameObject rail = Instantiate(railObject);
            rail.transform.position = (a + b) / 2;
            rail.transform.rotation = Quaternion.LookRotation(a - b);
            rail.transform.localScale = new Vector3(scale, scale, (a - b).magnitude * 50);
            rail.transform.parent = wireBase.transform;
        }

        GameObject lampBase = null;
        public GameObject createCrossLamp(Vector3 position, Vector3 forward)
        {
            if (lampBase == null)
            {
                lampBase = Instantiate(new GameObject(), new Vector3(0, 0, 0), new Quaternion());
                lampBase.name = "lampBase";
            }
            GameObject output = Instantiate(crossLamp);
            output.transform.position = convert(position);
            output.transform.rotation = Quaternion.LookRotation(forward, new Vector3(0, 1, 0));
            output.transform.parent = lampBase.transform;
            return output;
        }
        public GameObject createSideLamp(Vector3 position, Vector3 forward)
        {
            if (lampBase == null)
            {
                lampBase = Instantiate(new GameObject(), new Vector3(0, 0, 0), new Quaternion());
                lampBase.name = "lampBase";
            }
            GameObject output = Instantiate(sideLamp);
            output.transform.position = convert(position);
            output.transform.rotation = Quaternion.LookRotation(forward, new Vector3(0, 1, 0));
            output.transform.parent = lampBase.transform;
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
            GenerateFromCircles();

        }

        void GenerateFromCircles()
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

        RoadPhysicalObject oneRod = null;
        public void CreateCrossing(List<Vector3> polygon, int mat)
        {
            if (oneRod == null)
            {
                GameObject road = Instantiate(roadObject);
                RoadPhysicalObject roadobj = road.GetComponent<RoadPhysicalObject>();
                oneRod = roadobj;
                oneRod.name = "Roads";
                oneRod.CreateCrossingMesh(convert(polygon), mat);
            }
            else
            {
                oneRod.AddCrossingMesh(convert(polygon), mat);
            }
        }
        public void CreateRoad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int savok, bool tram, bool sideway, float zebra = 0.0f)
        {
            a = convert(a);
            b = convert(b);
            c = convert(c);
            d = convert(d);
            if (oneRod == null)
            {
                GameObject road = Instantiate(roadObject);
                RoadPhysicalObject roadobj = road.GetComponent<RoadPhysicalObject>();
                oneRod = roadobj;
                oneRod.name = "Roads";
                oneRod.CreateRoadMesh(a, b, c, d, savok, tram, sideway, zebra);
            }
            else
            {
                oneRod.AddRoadMesh(a, b, c, d, savok, tram, sideway, zebra);
            }
        }
        public void CreatRoadMesh()
        {
            oneRod.CreateMesh();
        }

    }
}
