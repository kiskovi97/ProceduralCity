using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Assets.Scripts.AdvancedCity
{
    [RequireComponent(typeof(RoadGeneratingValues))]
    public class GameObjectGenerator : MonoBehaviour
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

        GameObject wireBase = null;

        public void AddStoppingMesh(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            GameObject obj = Instantiate(stoppingObj);
            obj.transform.position = (a + b + c + d) / 4;
            obj.transform.localScale = new Vector3((a - b).magnitude, 0.5f, (a - c).magnitude/2);
            obj.transform.rotation = Quaternion.LookRotation(a - c);
        }

        public void AddLine(Vector3 a, Vector3 b, float scale)
        {
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
        public void AddLine(Vector3 a, Vector3 b, float scale, float magassag)
        {
            a += new Vector3(0, magassag, 0);
            b += new Vector3(0, magassag, 0);
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
        public void AddLine(Vector3 a, Vector3 b, float scale, float magassag, float magassag2)
        {
            a += new Vector3(0, magassag, 0);
            b += new Vector3(0, magassag2, 0);
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
        public GameObject createCrossLamp(Vector3 position, Vector3 forward, float magassag)
        {
            if (lampBase == null)
            {
                lampBase = Instantiate(new GameObject(), new Vector3(0, 0, 0), new Quaternion());
                lampBase.name = "lampBase";
            }
            GameObject output = Instantiate(crossLamp);
            output.transform.position = position + new Vector3(0, magassag, 0);
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
            output.transform.position = position;
            output.transform.rotation = Quaternion.LookRotation(forward, new Vector3(0, 1, 0));
            output.transform.parent = lampBase.transform;
            return output;
        }

        public void GenerateBlocks(List<Crossing> crossings, step delegateStep)
        {
            progress = 0;
            circles = new List<List<Crossing>>();
            roads = crossings;
            buildingContainer = GetComponent<BuildingContainer>();
            foreach (Crossing cros in roads)
            {
                List<Crossing> szomszedok = cros.getSzomszedok();
                foreach (Crossing second in szomszedok)
                {
                    GenerateCircle(cros, second, false);
                }
            }
            StartCoroutine(GenerateFromCircles(delegateStep));
        }
        public float progress = 0;
        public delegate void step(float step);
        IEnumerator GenerateFromCircles(step delegateStep)
        {
            float step = 1f / circles.Count;
            foreach (List<Crossing> circle in circles)
            {
                List<Vector3> controlPoints = new List<Vector3>();
                for (int i = 0; i < circle.Count; i++)
                {
                    int x = i + 1;
                    if (x > circle.Count - 1) x = 0;
                    controlPoints.Add(circle[i].Kereszt(circle[x]));
                }
                IBlockGenerator generator = new BlockGeneratorBasic();
                controlPoints.Reverse();
                RoadGeneratingValues values = GetComponent<RoadGeneratingValues>();
                if (values == null) throw new System.Exception("No Values Connected");
                generator.SetValues(values);
                generator.GenerateBuildings(controlPoints.ToArray(), buildingContainer);
                progress += step;
                delegateStep(step);
                yield return null;
            }
            progress = 1;
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
        public void CreateCrossing(List<Vector3> polygon)
        {
            if (oneRod == null)
            {
                GameObject road = Instantiate(roadObject);
                RoadPhysicalObject roadobj = road.GetComponent<RoadPhysicalObject>();
                oneRod = roadobj;
                oneRod.name = "Roads";
                oneRod.CreateCrossingMesh(polygon);
            }
            else
            {
                oneRod.AddCrossingMesh(polygon);
            }
        }
        public void CreateRoad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int savok, bool tram, bool sideway, float zebra = 0.0f, float otherzebra = 0.0f)
        {
            if (oneRod == null)
            {
                GameObject road = Instantiate(roadObject);
                RoadPhysicalObject roadobj = road.GetComponent<RoadPhysicalObject>();
                oneRod = roadobj;
                oneRod.name = "Roads";
                oneRod.CreateRoadMesh(a, b, c, d, savok, tram, sideway, zebra, otherzebra);
            }
            else
            {
                oneRod.AddRoadMesh(a, b, c, d, savok, tram, sideway, zebra, otherzebra);
            }
        }
        public RoadPhysicalObject CreatRoadMesh()
        {
            oneRod.CreateMesh();
            return oneRod;
        }
    }
}
