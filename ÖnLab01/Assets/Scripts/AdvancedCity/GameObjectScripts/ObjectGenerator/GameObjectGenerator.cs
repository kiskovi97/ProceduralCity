using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Assets.Scripts.AdvancedCity
{
    public class GameObjectGenerator : MonoBehaviour, IObjectGenerator
    {
        public IGameObjects gameObjects;
        List<Crossing> roads;
        List<List<Crossing>> circles;
        IValues values;

        public void SetValues(IValues values, IGameObjects gameObjects)
        {
            this.values = values;
            this.gameObjects = gameObjects;
        }

        GameObject wireBase = null;

        public void AddStoppingMesh(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            new ObjectConverter(Instantiate(gameObjects.stoppingObj)).Rectangle(a, b, c, d);  
        }
        
        public void AddLine(Vector3 a, Vector3 b, float scale, float height1, float height2)
        {
            if (a == b && height1 == height2) return;
            if (wireBase == null)
            {
                wireBase = Instantiate(new GameObject());
                wireBase.name = "WireBase";
            }
            GameObject wire = Instantiate(gameObjects.wireObject);
            new ObjectConverter(wire).Line(a, b, scale, height1, height2);
            wire.transform.parent = wireBase.transform;
        }

        public void CreateRails(Vector3 oneFrom, Vector3 oneToward, Vector3 otherFrom, Vector3 otherToward)
        {
            AddRail((oneFrom * 5 + otherFrom * 3) / 8, (oneToward * 5 + otherToward * 3) / 8, 0.4f);
            AddRail((oneFrom * 3 + otherFrom * 5) / 8, (oneToward * 3 + otherToward * 5) / 8, 0.4f);
        }

        public void CreateRails(MovementPoint a, MovementPoint b, float size, Vector3 distanceVector, int mat)
        {
            if (a == null || b == null) return;
            if (a == b) return;
            MovementPoint next = a.GetNextPoint();
            if (next == null) return;
            if (next!=b)
            {
                CreateRails(next, b, size, distanceVector, mat);
            }
            AddRail(a.center + distanceVector * (size / 2.0f), next.center + distanceVector * (size / 2.0f), 0.4f);
            AddRail(a.center - distanceVector * (size / 2.0f), next.center - distanceVector * (size / 2.0f), 0.4f);
        }
        public void AddRail(Vector3 a, Vector3 b, float scale)
        {
            if (a == b) return;
            if (wireBase == null)
            {
                wireBase = Instantiate(new GameObject(), new Vector3(0, 0, 0), new Quaternion());
                wireBase.name = "WireBase";
            }
            GameObject rail = Instantiate(gameObjects.railObject);
            new ObjectConverter(rail).Line(a, b, scale, 0, 0);
            rail.transform.parent = wireBase.transform;
        }

        GameObject lampBase = null;
        public GameObject CreateCrossLamp(Vector3 position, Vector3 forward, float height)
        {
            if (lampBase == null)
            {
                lampBase = Instantiate(new GameObject(), new Vector3(0, 0, 0), new Quaternion());
                lampBase.name = "lampBase";
            }
            GameObject output = Instantiate(gameObjects.crossLamp);
            new ObjectConverter(output).Forward(position, forward, height);
            output.transform.parent = lampBase.transform;
            return output;
        }
        public GameObject CreateSideLamp(Vector3 position, Vector3 forward)
        {
            if (lampBase == null)
            {
                lampBase = Instantiate(new GameObject(), new Vector3(0, 0, 0), new Quaternion());
                lampBase.name = "lampBase";
            }
            if (gameObjects.sideLamp == null)
            {
                throw new System.Exception("FUCK");
            }
            GameObject output = Instantiate(gameObjects.sideLamp);
            new ObjectConverter(output).Forward(position, forward, 0.0f);
            output.transform.parent = lampBase.transform;
            return output;
        }

        public void GenerateBlocks(List<Crossing> crossings, step delegateStep, IValues values)
        {
            progress = 0;
            circles = new List<List<Crossing>>();
            roads = crossings;
            foreach (Crossing cros in roads)
            {
                List<Crossing> szomszedok = cros.NeighbourCrossings();
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
                    controlPoints.Add(circle[i].SideCross(circle[x]));
                }
                IBlockGenerator generator = new BlockGeneratorBasic();
                controlPoints.Reverse();
                generator.SetValues(values);
                generator.GenerateBuildings(controlPoints.ToArray(), this);
                progress += step;
                delegateStep(step);
                yield return null;
            }
            progress = 1;
            yield return null;
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
                Crossing nextroad = circle[last].Next(circle[last - 1], jobbra);
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
                GameObject road = Instantiate(gameObjects.roadObject);
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
                GameObject road = Instantiate(gameObjects.roadObject);
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

        public BuildingObject CreateBuilding(Vector3[] polygon)
        {
            Vector3 actual = polygon[0];
            int min = (int)values.HouseUpmin;
            int max = (int)values.HouseUpmax;
            float positionValue = (values.GetTextureValue(actual)) * (values.GetTextureValue(actual));
            int floorNumber = (int)((Random.value * 0.5 + 0.5) * (max - min) * positionValue) + min;
            GameObject gameObject = Instantiate(gameObjects.BuildingBySize(values.GetTextureValue(actual)));
            gameObject.transform.position = actual;
            BuildingObject building = gameObject.GetComponent<BuildingObject>();
            building.MakeBuilding(polygon, floorNumber, values.Floor);
            return building;
        }

        public void CreateGreenPlace(Vector3[] polygon)
        {
            GameObject obj = Instantiate(gameObjects.Place);
            obj.transform.position = polygon[0];
            GreenPlace place = obj.GetComponent<GreenPlace>();
            place.MakePlace(polygon);
        }
    }
}
