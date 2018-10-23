using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    [System.Serializable]
    public class Crossing : System.Object
    {
        public bool main = false;

        public bool tram = false;

        public Vector3 center;

        private readonly float zebra = 0.7f;

        public List<Neighbour> neighbours = new List<Neighbour>();

        private IObjectGenerator generator;

        private readonly float roadSize;

        private readonly int curveIteration = 5;

        [System.Serializable]
        public class Neighbour : System.Object
        {
            public Road road;

            public HelpLine helpline;

            public CarPath carpath;

            public Neighbour(Road road)
            {
                this.road = road;
            }
        }

        public void SetMovementPoints(MovementPointContainer container)
        {
            foreach (Neighbour neighbour in neighbours) {
                neighbour.carpath.SetMovementPoints(container);
                if (neighbour.road != null)
                    neighbour.road.SetMovementPoints(container);
            }
        }

        public IEnumerable<MovementPoint> GetPoints()
        {
            List<MovementPoint> output = new List<MovementPoint>();
            foreach(Neighbour neighbour in neighbours)
            {
                output.AddRange(neighbour.carpath.GetPoints());
            }
            return output;
        }

        public Crossing(Vector3 center, bool main, bool tram, IObjectGenerator generator, float roadSize)
        {
            this.center = center;
            this.main = main;
            this.tram = tram;
            this.generator = generator;
            this.roadSize = roadSize;
        }

        public bool IsCorssing()
        {
            return neighbours.Count > 2;
        }

        public void AddRoad(Road newRoad)
        {
            neighbours.Add(new Neighbour(newRoad));
        }

        public void AddLines(HelpLine helpLine, Road road)
        {
            if (helpLine == null) return;
            foreach (Neighbour szomszed in neighbours)
            {
                if (szomszed.road.Equals(road))
                {
                    szomszed.helpline = helpLine;
                }
            }
            road.AddHelpLine(this, helpLine);
        }

        public void CarsPathSetting()
        {
            OrderNeighbours();
            for (int i = neighbours.Count - 1; i >= 0; i--)
            {
                int jobbra = i + 1;
                if (jobbra > neighbours.Count - 1) jobbra = 0;
                CreateMovePoints(neighbours[i], neighbours[jobbra]);
            }

            for (int i = neighbours.Count - 1; i >= 0; i--)
            {
                int jobbra = i + 1;
                if (jobbra > neighbours.Count - 1) jobbra = 0;
                ConnectMovementPoints(neighbours[i], neighbours[jobbra].carpath);
            }

            for (int i = 0; i < neighbours.Count; i++)
            {
                Vector3 iDir = neighbours[i].road.GetDir(this);
                for (int j = 0; j < neighbours.Count; j++)
                {
                    if (j == i) continue;
                    Vector3 jDir = neighbours[j].road.GetDir(this);
                    neighbours[i].carpath.others = neighbours[i].carpath.others.Concat(
                        MovementPoint.Connect(neighbours[i].carpath.input, neighbours[j].carpath.output, iDir, jDir * -1)).ToArray();
                    if (neighbours[i].carpath.tramInput != null && neighbours[j].carpath.tramOutput != null)
                        neighbours[i].carpath.others = neighbours[i].carpath.others.Concat(
                            MovementPoint.CurveAndConnect(neighbours[i].carpath.tramInput, neighbours[j].carpath.tramOutput, curveIteration)).ToArray();
                }
            }
        }

        private void OrderNeighbours()
        {
            for (int i = 0; i < neighbours.Count - 1; i++)
            {
                Vector3 actualDir = neighbours[i].road.GetDir(this);
                float minAngle = 360;
                int z = i;
                for (int j = i + 1; j < neighbours.Count; j++)
                {
                    Vector3 otherDir = neighbours[j].road.GetDir(this);
                    float otherAngle = Vector3.SignedAngle(actualDir, otherDir, new Vector3(0, 1, 0));
                    if (otherAngle < 0) otherAngle += 360;
                    if (minAngle > otherAngle)
                    {
                        z = j;
                        minAngle = otherAngle;
                    }
                }
                if (z > i + 1)
                {
                    Neighbour tmp = neighbours[i + 1];
                    neighbours[i + 1] = neighbours[z];
                    neighbours[z] = tmp;
                }
            }
        }

        void CreateMovePoints(Neighbour actual, Neighbour right)
        {
            Vector3[] line = actual.helpline.mainLine;
            int savCount = actual.road.Savok();
            int carSavCount = actual.road.tram ? savCount - 1 : savCount;
            Vector3 direction = actual.road.GetDir(this).normalized * -1;
            CarPath carpath = new CarPath
            {
                leftCross = new MovementPoint((actual.helpline.roadEdgeCross + actual.helpline.sideCross) / 2),
                rightCross = new MovementPoint((right.helpline.roadEdgeCross + right.helpline.sideCross) / 2),
                input = new MovementPoint[carSavCount],
                output = new MovementPoint[carSavCount],
                others = new MovementPoint[0],
                tramInput = null,
                tramOutput = null
            };
            for (int j = 0; j < carSavCount; j++)
            {
                float realZebra = (neighbours.Count == 2) ? 0 : zebra;
                int a = (1 + j * 2);
                int b = savCount * 4 - a;
                carpath.input[j] = new MovementPoint((line[0] * a + line[1] * b) / (savCount * 4) + direction * realZebra);
                carpath.input[j].SetDirection(actual.road.GetDir(this));
                if (neighbours.Count > 2) carpath.input[j].OpenClose(false);
                carpath.output[j] = new MovementPoint((line[0] * b + line[1] * a) / (savCount * 4));
                carpath.output[j].SetDirection(actual.road.GetDir(this) * -1);
            }
            if (actual.road.tram)
            {
                int a = (1 + carSavCount * 2);
                int b = (savCount * 4 - a);
                carpath.tramInput = new MovementPoint((line[0] * a + line[1] * b) / (savCount * 4));
                carpath.tramInput.SetDirection(actual.road.GetDir(this));
                carpath.tramOutput = new MovementPoint((line[0] * b + line[1] * a) / (savCount * 4));
                carpath.tramOutput.SetDirection(actual.road.GetDir(this) * -1);
            }
            actual.carpath = carpath;
        }

        void ConnectMovementPoints(Neighbour neighbour, CarPath rightCarpath)
        {
            CarPath carpath = neighbour.carpath;
            carpath.leftCross.ConnectPoint(carpath.rightCross);
            carpath.rightCross.ConnectPoint(carpath.leftCross);
            carpath.rightCross.ConnectPoint(rightCarpath.leftCross);
            rightCarpath.leftCross.ConnectPoint(carpath.rightCross);
            neighbour.road.AddCarpath(this, carpath.SwitchCopy());
        }

        public int openRoadIndex = 0;

        public void Switch(bool Yellow)
        {
            if (neighbours.Count < 3) return;
            if (Yellow)
            {
                CarPath carpath = neighbours[openRoadIndex].carpath;
                foreach (MovementPoint point in carpath.input) point.OpenClose(false);
                if (carpath.lamps != null)
                    foreach (MeshRenderer renderer in carpath.lamps)
                    {
                        Material[] tomb = renderer.sharedMaterials;
                        tomb[1] = CrossLampColorsRYG[1];
                        tomb[2] = CrossLampColorsRYG[0];
                        tomb[3] = CrossLampColorsRYG[0];
                        renderer.materials = tomb;
                    }

                openRoadIndex++;
                if (openRoadIndex > neighbours.Count - 1) openRoadIndex = 0;
                carpath = neighbours[openRoadIndex].carpath;
                foreach (MovementPoint point in carpath.input) point.OpenClose(true);
                if (carpath.lamps != null)
                    foreach (MeshRenderer renderer in carpath.lamps)
                    {
                        Material[] tomb = renderer.sharedMaterials;
                        tomb[1] = CrossLampColorsRYG[0];
                        tomb[2] = CrossLampColorsRYG[0];
                        tomb[3] = CrossLampColorsRYG[3];
                        renderer.materials = tomb;
                    }
            }
            else
            {
                CarPath carpath = neighbours[openRoadIndex].carpath;
                if (carpath.lamps != null)
                    foreach (MeshRenderer renderer in carpath.lamps)
                    {
                        Material[] tomb = renderer.sharedMaterials;
                        tomb[1] = CrossLampColorsRYG[0];
                        tomb[2] = CrossLampColorsRYG[2];
                        tomb[3] = CrossLampColorsRYG[0];
                        renderer.materials = tomb;
                    }
                int next = openRoadIndex + 1;
                if (next > neighbours.Count - 1) next = 0;
                carpath = neighbours[next].carpath;
                if (carpath.lamps != null)
                    foreach (MeshRenderer renderer in carpath.lamps)
                    {
                        Material[] tomb = renderer.sharedMaterials;
                        tomb[1] = CrossLampColorsRYG[1];
                        tomb[2] = CrossLampColorsRYG[2];
                        tomb[3] = CrossLampColorsRYG[0];
                        renderer.materials = tomb;
                    }
            }
            Yellow = !Yellow;
        }

        public void Draw(bool helplines_draw, bool depthtest, bool tramIsPresent)
        {
            List<Vector3> polygon = new List<Vector3>();
            for (int j = 0; j < neighbours.Count; j++)
            {
                Neighbour neighbour = neighbours[j];
                if (helplines_draw) neighbour.carpath.DrawHelpLines(depthtest);
                if (neighbours.Count > 2) MakeCrossLamps(neighbour);

                int x = j + 1;
                if (x >= neighbours.Count) x = 0;
                // SideWalk Draw
                if (neighbour.helpline.sideline[0] != neighbour.helpline.sideline[1])
                {
                    bool distance = neighbour.helpline.mainLine[0] == neighbour.helpline.sideline[1];
                    generator.CreateRoad(neighbour.helpline.sideline[1], neighbour.helpline.sideline[0],
                        distance ? neighbours[x].helpline.sideCross : neighbour.helpline.sideCross,
                        distance ? neighbours[x].helpline.sideCross : neighbour.helpline.sideCross, 1, false, true);
                }
                // Polygon Upload
                if (neighbour.helpline.sideline[1] == neighbour.helpline.mainLine[0])
                {
                    if (polygon.Count < 1 || polygon[polygon.Count - 1] != neighbour.helpline.mainLine[1])
                        polygon.Add(neighbour.helpline.mainLine[1]);
                    if (polygon.Count < 1 || polygon[polygon.Count - 1] != neighbour.helpline.sideline[1])
                        polygon.Add(neighbour.helpline.sideline[1]);
                }
                else
                {
                    if (polygon.Count < 1 || polygon[polygon.Count - 1] != neighbour.helpline.sideline[1])
                        polygon.Add(neighbour.helpline.sideline[1]);
                    if (polygon.Count < 1 || polygon[polygon.Count - 1] != neighbour.helpline.mainLine[1])
                        polygon.Add(neighbour.helpline.mainLine[1]);
                }
                // Tram
                if (neighbour.carpath.tramInput != null && tram && tramIsPresent)
                {
                    for (int i = 0; i < neighbours.Count; i++)
                    {
                        if (j == i) break;
                        if (neighbours[i].carpath.tramOutput != null)
                        {
                            DrawTram(neighbour.carpath.tramInput, neighbours[i].carpath.tramOutput, neighbours[i].carpath.tramInput, neighbour.carpath.tramOutput);
                        }
                    }
                }
            }
            generator.CreateCrossing(polygon);
        }

        void DrawTram(MovementPoint oneFrom, MovementPoint oneToward, MovementPoint otherFrom, MovementPoint otherToward)
        {
            List<Vector3> egyikList = new List<Vector3>();
            int max = 10;
            while (oneFrom != oneToward && max > 0)
            {
                max--;
                MovementPoint tmp = oneFrom.GetPoint();
                if (tmp == null) break;
                generator.AddLine(oneFrom.center, tmp.center, 0.2f, 0.35f, 0.35f);
                egyikList.Add(oneFrom.center);
                oneFrom = tmp;
            }
            egyikList.Add(oneFrom.center);
            egyikList.Reverse();
            if (max == 0) return;
            max = 10;
            int i = 0;
            while (otherFrom != otherToward && max > 0)
            {
                max--;
                i++;
                MovementPoint tmp = otherFrom.GetPoint();
                if (tmp == null) break;
                generator.AddLine(otherFrom.center, tmp.center, 0.2f, 0.35f, 0.35f);
                Vector3 merolegesFrom = (otherFrom.center - egyikList[i - 1]).normalized * roadSize / 2;
                Vector3 merolegesTowards = (tmp.center - egyikList[i]).normalized * roadSize / 2;
                generator.CreateRails(egyikList[i - 1] + merolegesFrom, egyikList[i] + merolegesTowards, egyikList[i - 1] - merolegesFrom, egyikList[i] - merolegesTowards);
                generator.CreateRails(otherFrom.center + merolegesFrom, tmp.center + merolegesTowards, otherFrom.center - merolegesFrom, tmp.center - merolegesTowards);
                otherFrom = tmp;
            }
        }

        private void MakeCrossLamps(Neighbour szomszed)
        {
            Vector3 forward = szomszed.road.GetDir(this).normalized * -1;
            szomszed.carpath.lamps = new MeshRenderer[szomszed.carpath.input.Length];
            for (int i = 0; i < szomszed.carpath.input.Length; i++)
            {
                Vector3 pos = szomszed.carpath.input[i].center;
                Vector3 intersect = MyMath.Intersect(szomszed.helpline.mainLine[0], szomszed.helpline.mainLine[1] - szomszed.helpline.mainLine[0], pos, forward);
                GameObject lamp = generator.CreateCrossLamp(intersect, forward, 0.3f);
                MeshRenderer renderer = lamp.GetComponent<MeshRenderer>();
                Material[] tomb = renderer.sharedMaterials;
                CrossLampColorsRYG = tomb.ToArray();
                tomb[2] = tomb[0];
                tomb[3] = tomb[0];
                renderer.materials = tomb;
                generator.AddLine(intersect, intersect, 0.2f, .3f, .4f);
                szomszed.carpath.lamps[i] = renderer;
            }
            generator.AddLine(szomszed.helpline.mainLine[1], (szomszed.helpline.mainLine[1] + szomszed.helpline.mainLine[0]) / 2, 0.2f, 0.4f, 0.4f);
            generator.AddLine(szomszed.helpline.mainLine[1], szomszed.helpline.mainLine[1], 0.6f, 0, 0.4f);
        }

        public Material[] CrossLampColorsRYG;

        private int cars = 0;

        private int kimenetIndex = 0;

        public bool AddVehicle(Vehicle car)
        {
            if (neighbours == null) return false;
            if (neighbours.Count > cars)
            {
                car.SetPoint(neighbours[cars].carpath.output[kimenetIndex++]);
                if (kimenetIndex >= neighbours[cars].carpath.output.Length || (kimenetIndex >= neighbours[cars].carpath.output.Length - 1 && neighbours[cars].road.tram))
                {
                    cars++;
                    kimenetIndex = 0;
                }
                return true;
            }
            return false;
        }

        private int peoples = 0;

        public bool AddPeople(Vehicle car)
        {
            if (neighbours == null) return false;
            if (neighbours.Count > peoples)
            {
                car.SetPoint(neighbours[peoples].carpath.rightCross);
                peoples++;
                return true;
            }
            return false;
        }

        public void AddTram(Vehicle tram1, Vehicle tram2)
        {
            bool carbool = false;
            foreach (Neighbour szomszed in neighbours)
            {
                if (szomszed.road.tram && !carbool)
                {
                    carbool = true;
                    tram1.SetPoint(szomszed.carpath.tramOutput);
                    continue;
                }
                if (szomszed.road.tram && carbool)
                {
                    tram2.SetPoint(szomszed.carpath.tramOutput);
                    return;
                }
            }
        }
        public bool HavePlace()
        {
            return neighbours.Count > cars && neighbours.Count > 1;
        }

        public bool HavePlaceForPeople()
        {
            return neighbours.Count > peoples && neighbours.Count > 1;
        }

        public bool IsCrossing()
        {
            if (neighbours == null) return false;
            return neighbours.Count > 2;
        }

        public Road GetNeighbourRoad(Vector3 to)
        {
            int i = 0;
            while (i < neighbours.Count)
            {
                Vector3 other = neighbours[i].road.NextCros(this).center;
                if (other.Equals(to))
                {
                    break;
                }
                i++;
            }
            if (i < neighbours.Count)
                return neighbours[i].road;
            else
                return null;
        }
        public Vector3 SideCross(Crossing crossing)
        {
            if (crossing == null) return center;
            List<Crossing> list = NeighbourCrossings();
            if (list == null) return center;
            if (list.Contains(crossing))
            {
                int i = list.IndexOf(crossing);
                return neighbours[i].helpline.sideCross;
            }
            return center;
        }
        public Vector3 MainCross(Crossing crossing)
        {
            if (crossing == null) return center;
            List<Crossing> list = NeighbourCrossings();
            if (list == null) return center;
            if (list.Contains(crossing))
            {
                int i = list.IndexOf(crossing);
                return neighbours[i].helpline.mainLine[1];
            }
            else
                return center;
        }
        public Vector3 OtherSideCross(Crossing crossing)
        {
            if (crossing == null) return center;
            List<Crossing> list = NeighbourCrossings();
            if (list == null) return center;
            if (list.Contains(crossing))
            {
                int i = list.IndexOf(crossing);
                if (i < neighbours.Count - 1)
                    return neighbours[i + 1].helpline.sideCross;
                else
                    return neighbours[0].helpline.sideCross;
            }
            else
                return center;
        }
        public Vector3 OtherMainCross(Crossing crossing)
        {
            if (crossing == null) return center;
            List<Crossing> list = NeighbourCrossings();
            if (list == null) return center;
            if (list.Contains(crossing))
            {
                int i = list.IndexOf(crossing);
                if (i < neighbours.Count - 1)
                    return neighbours[i].helpline.mainLine[0];
                else
                    return neighbours[i].helpline.mainLine[0];
            }
            else
                return center;
        }

        public Road GetRoad(Crossing cros)
        {
            if (neighbours == null) return null;
            foreach (Neighbour szomszed in neighbours)
            {
                if (cros == szomszed.road.NextCros(this)) return szomszed.road;
            }
            return null;
        }


        public List<Crossing> NeighbourCrossings()
        {
            OrderNeighbours();
            if (neighbours == null) return null;
            List<Crossing> output = new List<Crossing>();
            for (int i = 0; i < neighbours.Count; i++)
                output.Add(neighbours[i].road.NextCros(this));
            return output;
        }

        public Crossing Next(Crossing crossing, bool jobbra)
        {
            OrderNeighbours();
            if (crossing == null) return null;
            List<Crossing> list = NeighbourCrossings();
            if (list == null) return null;
            if (list.Contains(crossing))
            {
                int i = list.IndexOf(crossing);
                int x = i + 1;
                if (x > list.Count - 1) x = 0;
                return list[x];
            }
            else return null;
        }

    }
}
