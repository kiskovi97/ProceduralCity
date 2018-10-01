using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    public class Crossing
    {
        public bool main;
        public bool tram = false;
        public Vector3 center;
        readonly float zebra = 0.7f;
        List<Neighbor> neighbours = new List<Neighbor>();
        GameObjectGenerator generator;
        public class Neighbor
        {
            public Neighbor(Road road)
            {
                this.road = road;
            }
            public Road road;
            public HelpLine helpline;
            public CarPath carpath;
        }
        public class CarPath
        {
            public MovementPoint[] others;
            public MovementPoint[] bemenet;
            public MovementPoint[] kimenet;
            public MovementPoint leftCross;
            public MovementPoint rightCross;
            public MeshRenderer[] lamps;
        }
        public class HelpLine
        {
            public Vector3[] mainline;
            public Vector3[] sideline;
            public Vector3 roadEdgeCross;
            public Vector3 sideCross;
        }

        public bool IsCorssing()
        {
            return neighbours.Count > 2;
        }

        public Crossing(Vector3 center, bool main, bool tram, GameObjectGenerator generator)
        {
            this.center = center;
            this.main = main;
            this.tram = tram;
            this.generator = generator;
        }

        public void AddSzomszed(Road ujszomszed)
        {
            neighbours.Add(new Neighbor(ujszomszed));
        }
        public void AddLines(Vector3[] mainline, Vector3[] sideline, Vector3 roadedgecross, Vector3 sidewalkcorss, Road road)
        {
            if (mainline == null || sideline == null) return;
            foreach (Neighbor szomszed in neighbours)
            {
                if (szomszed.road.Equals(road))
                {
                    szomszed.helpline = new HelpLine
                    {
                        mainline = mainline,
                        sideline = sideline,
                        roadEdgeCross = roadedgecross,
                        sideCross = sidewalkcorss
                    };
                }
            }
        }

        private void Order()
        {
            for (int i = 0; i < neighbours.Count - 1; i++)
            {
                Vector3 actualDir = neighbours[i].road.NextCros(this).center - center;
                float minAngle = 360;
                int z = i;
                for (int j = i + 1; j < neighbours.Count; j++)
                {
                    Vector3 otherDir = neighbours[j].road.NextCros(this).center - center;
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
                    Neighbor tmp = neighbours[i + 1];
                    neighbours[i + 1] = neighbours[z];
                    neighbours[z] = tmp;
                }
            }
        }

        public void CarsPathSetting()
        {
            Order();
            for (int i = neighbours.Count - 1; i >= 0; i--)
            {
                int jobbra = i + 1;
                if (jobbra > neighbours.Count - 1) jobbra = 0;
                MakeMovePoints(neighbours[i], neighbours[jobbra]);
            }

            for (int i = neighbours.Count - 1; i >= 0; i--)
            {
                int jobbra = i + 1;
                if (jobbra > neighbours.Count - 1) jobbra = 0;
                MakeRoads(i, jobbra);
            }

            if (neighbours.Count > 2)
                for (int i = 0; i < neighbours.Count; i++)
                {
                    for (int j = 0; j < neighbours.Count; j++)
                    {
                        if (j == i) continue;
                        Vector3 iDir = neighbours[i].road.getDir(this);
                        Vector3 jDir = neighbours[j].road.getDir(this);
                        neighbours[i].carpath.others = neighbours[i].carpath.others.Concat(
                            MovementPoint.Connect(neighbours[i].carpath.bemenet, neighbours[j].carpath.kimenet, neighbours[i].road.tram, neighbours[j].road.tram, iDir, jDir * -1))
                            .ToArray();
                    }
                }
            if (neighbours.Count == 2)
            {
                Vector3 iDir = neighbours[0].road.getDir(this);
                Vector3 jDir = neighbours[1].road.getDir(this);
                neighbours[0].carpath.others =
                    MovementPoint.Connect(neighbours[0].carpath.bemenet, neighbours[1].carpath.kimenet, neighbours[0].road.tram, neighbours[1].road.tram, iDir, jDir * -1);
                neighbours[1].carpath.others =
                    MovementPoint.Connect(neighbours[1].carpath.bemenet, neighbours[0].carpath.kimenet, neighbours[1].road.tram, neighbours[0].road.tram, jDir, iDir * -1);
            }
        }

        public int nyitott = 0;
        public bool stop = false;
        public void Valt()
        {
            if (neighbours.Count < 3) return;
            foreach (Neighbor szomszed in neighbours)
            {
                for (int i = 0; i < szomszed.carpath.bemenet.Length; i++)
                {
                    MovementPoint point = szomszed.carpath.bemenet[i];
                    point.Nyitott(false);
                    if (szomszed.road.tram && i == szomszed.carpath.bemenet.Length - 1)
                        point.Nyitott(true);
                }
                if (szomszed.carpath.lamps != null)
                    foreach (MeshRenderer renderer in szomszed.carpath.lamps)
                    {
                        Material[] tomb = renderer.materials;
                        tomb[1] = RYG[1];
                        tomb[2] = RYG[0];
                        tomb[3] = RYG[0];
                        renderer.materials = tomb;
                    }
            }
            if (!stop)
            {
                nyitott++;
                if (nyitott > neighbours.Count - 1) nyitott = 0;
                Neighbor szomszed = neighbours[nyitott];
                foreach (MovementPoint point in szomszed.carpath.bemenet)
                {
                    point.Nyitott(true);
                }
                if (szomszed.carpath.lamps != null)
                    foreach (MeshRenderer renderer in szomszed.carpath.lamps)
                    {
                        Material[] tomb = renderer.materials;
                        tomb[1] = RYG[0];
                        tomb[2] = RYG[0];
                        tomb[3] = RYG[3];
                        renderer.materials = tomb;
                    }
            }
            else
            {
                Neighbor szomszed = neighbours[nyitott];
                if (szomszed.carpath.lamps != null)
                    foreach (MeshRenderer renderer in szomszed.carpath.lamps)
                    {
                        Material[] tomb = renderer.materials;
                        tomb[1] = RYG[0];
                        tomb[2] = RYG[2];
                        tomb[3] = RYG[0];
                        renderer.materials = tomb;
                    }
                int next = nyitott + 1;
                if (next > neighbours.Count - 1) next = 0;
                szomszed = neighbours[next];
                if (szomszed.carpath.lamps != null)
                    foreach (MeshRenderer renderer in szomszed.carpath.lamps)
                    {
                        Material[] tomb = renderer.materials;
                        tomb[1] = RYG[1];
                        tomb[2] = RYG[2];
                        tomb[3] = RYG[0];
                        renderer.materials = tomb;
                    }
            }
            stop = !stop;
        }

        void MakeMovePoints(Neighbor actual, Neighbor right)
        {
            Vector3[] line = actual.helpline.mainline;
            Vector3 othercenter = actual.road.NextCros(this).center;
            Vector3 direction = (othercenter - center).normalized;
            if (line != null)
            {
                CarPath carpath = new CarPath();
                int thissavok = actual.road.Savok();
                carpath.leftCross = new MovementPoint((actual.helpline.roadEdgeCross + actual.helpline.sideCross) / 2);
                carpath.rightCross = new MovementPoint((right.helpline.roadEdgeCross + right.helpline.sideCross) / 2);
                carpath.bemenet = new MovementPoint[thissavok];
                carpath.kimenet = new MovementPoint[thissavok];
                carpath.others = new MovementPoint[0];
                for (int j = 0; j < thissavok; j++)
                {
                    int a = (1 + j * 2);
                    int b = thissavok * 4 - a;
                    float realZebra = (neighbours.Count == 2 || (actual.road.tram && j== thissavok - 1)) ? 0 : zebra;
                    carpath.bemenet[j] = new MovementPoint((line[0] * a + line[1] * b) / (thissavok * 4) + direction * realZebra);
                    if (neighbours.Count > 2) carpath.bemenet[j].Nyitott(false);
                    carpath.kimenet[j] = new MovementPoint((line[0] * b + line[1] * a) / (thissavok * 4));
                }
                actual.carpath = carpath;
            }
        }

        void MakeRoads(int i, int jobbra)
        {
            CarPath carpath = neighbours[i].carpath;
            List<MovementPoint> kimenet = new List<MovementPoint>(carpath.kimenet)
            {
                carpath.rightCross
            };
            List<MovementPoint> bemenet = new List<MovementPoint>(carpath.bemenet)
            {
                carpath.leftCross
            };
            carpath.rightCross.ConnectPoint(neighbours[jobbra].carpath.leftCross);
            neighbours[jobbra].carpath.leftCross.ConnectPoint(carpath.rightCross);
            carpath.leftCross.ConnectPoint(carpath.rightCross);
            carpath.rightCross.ConnectPoint(carpath.leftCross);
            neighbours[i].road.addMovePoint(this, kimenet.ToArray(), bemenet.ToArray());
        }

        public void Draw(bool helplines_draw, bool depthtest, bool tramIsPresent)
        {
            List<Vector3> polygon = new List<Vector3>();
            Vector3 uplittle = new Vector3(0, 0.1f, 0);
            for (int j = 0; j < neighbours.Count; j++)
            {
                Neighbor szomszed = neighbours[j];
                int x = j + 1;
                if (x >= neighbours.Count) x = 0;
                if (helplines_draw)
                {
                    szomszed.carpath.leftCross.Draw(depthtest);
                    szomszed.carpath.rightCross.Draw(depthtest);
                    for (int i = 0; i < szomszed.carpath.kimenet.Length; i++)
                        szomszed.carpath.kimenet[i].Draw(depthtest);
                    for (int i = 0; i < szomszed.carpath.bemenet.Length; i++)
                        szomszed.carpath.bemenet[i].Draw(depthtest);
                    for (int i = 0; i < szomszed.carpath.others.Length; i++)
                        szomszed.carpath.others[i].Draw(depthtest);
                }
                if (szomszed.helpline.sideline[0] != szomszed.helpline.sideline[1])
                {
                    bool distance = szomszed.helpline.mainline[0] == szomszed.helpline.sideline[1];
                    generator.CreateRoad(szomszed.helpline.sideline[1], szomszed.helpline.sideline[0],
                        distance ? neighbours[x].helpline.sideCross : szomszed.helpline.sideCross,
                        distance ? neighbours[x].helpline.sideCross : szomszed.helpline.sideCross, 1, false, true);
                }
                if (neighbours.Count > 2)
                {
                    makeLamps(szomszed);
                }
                if (szomszed.helpline.sideline[1] == szomszed.helpline.mainline[0])
                {
                    uplittle = uplittle * 3;
                    if (polygon.Count < 1 || polygon[polygon.Count - 1] != szomszed.helpline.mainline[1])
                        polygon.Add(szomszed.helpline.mainline[1]);
                    if (polygon.Count < 1 || polygon[polygon.Count - 1] != szomszed.helpline.sideline[1])
                        polygon.Add(szomszed.helpline.sideline[1]);
                }
                else
                {
                    uplittle = uplittle * 3;
                    if (polygon.Count < 1 || polygon[polygon.Count - 1] != szomszed.helpline.sideline[1])
                        polygon.Add(szomszed.helpline.sideline[1]);
                    if (polygon.Count < 1 || polygon[polygon.Count - 1] != szomszed.helpline.mainline[1])
                        polygon.Add(szomszed.helpline.mainline[1]);
                }
            }
            generator.CreateCrossing(polygon);

            if (tram)
            {
                List<Neighbor> trams = neighbours.FindAll(TramNeighbour);
                MovementPoint egyik = trams[0].carpath.bemenet.Last();
                MovementPoint masik = trams[1].carpath.kimenet.Last();
                List<Vector3> egyikList = new List<Vector3>();
                List<Vector3> masikList = new List<Vector3>();
                int max = 10;
                while (egyik != masik && max > 0)
                {
                    max--;
                    MovementPoint tmp = egyik.getPoint();
                    if (tmp == null) break;
                    if (tramIsPresent) generator.AddLine(egyik.center, tmp.center, 0.2f, 0.35f);
                    egyikList.Add(egyik.center);
                    egyik = tmp;
                }
                egyikList.Add(egyik.center);
                egyik = trams[1].carpath.bemenet.Last();
                masik = trams[0].carpath.kimenet.Last();
                max = 10;
                while (egyik != masik && max > 0)
                {
                    max--;
                    MovementPoint tmp = egyik.getPoint();
                    if (tmp == null) break;
                    if (tramIsPresent) generator.AddLine(egyik.center, tmp.center, 0.2f, 0.35f);
                    masikList.Add(egyik.center);
                    egyik = tmp;
                }
                masikList.Add(egyik.center);
                for (int i = 0; i < egyikList.Count; i++)
                {
                    Vector3 tmpEgyik = egyikList[i];
                    Vector3 tmpMasik = masikList[masikList.Count - 1 - i];
                    Vector3 tmpCenter = (tmpEgyik + tmpMasik) / 2;
                    tmpEgyik = tmpEgyik + (tmpEgyik - tmpCenter);
                    tmpMasik = tmpMasik + (tmpMasik - tmpCenter);
                    egyikList[i] = tmpEgyik;
                    masikList[masikList.Count - 1 - i] = tmpMasik;
                }
                for (int i = 0; i < egyikList.Count - 1; i++)
                {
                    Vector3 tmpEgyik = egyikList[i];
                    Vector3 tmpMasik = masikList[masikList.Count - i - 1];
                    Vector3 tmpCenter = (tmpEgyik + tmpMasik) / 2;
                    Vector3 tmpEgyik2 = egyikList[i + 1];
                    Vector3 tmpMasik2 = masikList[masikList.Count - i - 2];
                    Vector3 tmpCenter2 = (tmpEgyik2 + tmpMasik2) / 2;
                    if (tramIsPresent) generator.CreateRails(tmpCenter2, tmpCenter, tmpEgyik2, tmpEgyik, 3);
                    if (tramIsPresent) generator.CreateRails(tmpCenter, tmpCenter2, tmpMasik, tmpMasik2, 3);
                }
            }
        }

        private bool TramNeighbour(Neighbor szomszed)
        {
            return szomszed.road.tram;
        }

        private void makeLamps(Neighbor szomszed)
        {
            Vector3 forward = Vector3.Cross(szomszed.helpline.mainline[0] - szomszed.helpline.mainline[1], new Vector3(0, 1, 0));
            szomszed.carpath.lamps = new MeshRenderer[szomszed.carpath.bemenet.Length];
            if (szomszed.road.tram) szomszed.carpath.lamps = new MeshRenderer[szomszed.carpath.bemenet.Length - 1];
            for (int i = 0; i < szomszed.carpath.bemenet.Length; i++)
            {
                if (szomszed.road.tram && i == szomszed.carpath.bemenet.Length - 1) break;
                Vector3 pos = szomszed.carpath.bemenet[i].center;
                Vector3 meroleges = Vector3.Cross(szomszed.helpline.mainline[1] - szomszed.helpline.mainline[0], new Vector3(0, 1, 0));
                Vector3 intersect = MyMath.Intersect(szomszed.helpline.mainline[0], szomszed.helpline.mainline[1] - szomszed.helpline.mainline[0], pos, meroleges);
                GameObject lamp = generator.createCrossLamp(intersect, forward, 0.3f);
                MeshRenderer renderer = lamp.GetComponent<MeshRenderer>();
                Material[] tomb = renderer.materials;
                RYG = tomb.ToArray();
                tomb[2] = tomb[0];
                tomb[3] = tomb[0];
                renderer.materials = tomb;
                generator.AddLine(intersect, intersect, 0.2f, .3f, .4f);
                if (szomszed.road.tram && i == szomszed.carpath.bemenet.Length - 1) break;
                szomszed.carpath.lamps[i] = renderer;
            }
            generator.AddLine(szomszed.helpline.mainline[1], (szomszed.helpline.mainline[1] + szomszed.helpline.mainline[0]) / 2, 0.2f, 0.4f);
            generator.AddLine(szomszed.helpline.mainline[1], szomszed.helpline.mainline[1], 0.6f, 0, 0.4f);
        }

        private Material[] RYG;

        private int cars = 0;
        private int kimenetIndex = 0;
        public bool AddVehicle(Vehicle car)
        {
            if (neighbours == null) return false;
            if (neighbours.Count > cars)
            {
                car.setPoint(neighbours[cars].carpath.kimenet[kimenetIndex++]);
                if (kimenetIndex >= neighbours[cars].carpath.kimenet.Length || (kimenetIndex >= neighbours[cars].carpath.kimenet.Length - 1 && neighbours[cars].road.tram))
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
                car.setPoint(neighbours[peoples].carpath.rightCross);
                peoples++;
                return true;
            }
            return false;
        }

        public void AddTram(Vehicle car, Vehicle car2)
        {
            bool carbool = false;
            foreach (Neighbor szomszed in neighbours)
            {
                if (szomszed.road.tram && !carbool)
                {
                    carbool = true;
                    int max = szomszed.carpath.kimenet.Length - 1;
                    car.setPoint(szomszed.carpath.kimenet[max]);
                    continue;
                }
                if (szomszed.road.tram && carbool)
                {
                    int max = szomszed.carpath.kimenet.Length - 1;
                    car2.setPoint(szomszed.carpath.kimenet[max]);
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

        public bool isCrossing()
        {
            if (neighbours == null) return false;
            return neighbours.Count > 2;
        }

        public Road getSzomszedRoad(Vector3 to)
        {
            int i = 0;
            while (i < neighbours.Count)
            {
                Vector3 masik = neighbours[i].road.NextCros(this).center;
                if (masik.Equals(to))
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
        public Vector3 Kereszt(Crossing crossing)
        {
            if (crossing == null) return center;
            List<Crossing> list = getSzomszedok();
            if (list == null) return center;
            if (list.Contains(crossing))
            {
                int i = list.IndexOf(crossing);
                return neighbours[i].helpline.sideCross;
            }
            else
                return center;
        }
        public Vector3 KeresztRoad(Crossing crossing)
        {
            if (crossing == null) return center;
            List<Crossing> list = getSzomszedok();
            if (list == null) return center;
            if (list.Contains(crossing))
            {
                int i = list.IndexOf(crossing);
                return neighbours[i].helpline.mainline[1];
            }
            else
                return center;
        }
        public Vector3 KeresztMasik(Crossing crossing)
        {
            if (crossing == null) return center;
            List<Crossing> list = getSzomszedok();
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
        public Vector3 KeresztRoadMasik(Crossing crossing)
        {
            if (crossing == null) return center;
            List<Crossing> list = getSzomszedok();
            if (list == null) return center;
            if (list.Contains(crossing))
            {
                int i = list.IndexOf(crossing);
                if (i < neighbours.Count - 1)
                    return neighbours[i].helpline.mainline[0];
                else
                    return neighbours[i].helpline.mainline[0];
            }
            else
                return center;
        }

        public Road GetRoad(Crossing cros)
        {
            if (neighbours == null) return null;
            foreach (Neighbor szomszed in neighbours)
            {
                if (cros == szomszed.road.NextCros(this)) return szomszed.road;
            }
            return null;
        }

        private void sorbaRendez()
        {
            for (int i = 0; i < neighbours.Count - 1; i++)
            {
                Vector3 eddigiirany = neighbours[i].road.NextCros(this).center - center;
                float eddigiszog = 360;
                int z = i;
                for (int j = i + 1; j < neighbours.Count; j++)
                {
                    Vector3 masirany = neighbours[j].road.NextCros(this).center - center;
                    float szog = Vector3.SignedAngle(eddigiirany, masirany, new Vector3(0, 1, 0));
                    if (szog < 0) szog += 360;
                    if (eddigiszog > szog)
                    {
                        z = j;
                        eddigiszog = szog;
                    }
                }
                if (z > i + 1)
                {
                    Neighbor tmp = neighbours[i + 1];
                    neighbours[i + 1] = neighbours[z];
                    neighbours[z] = tmp;
                }
            }
        }

        public List<Crossing> getSzomszedok()
        {
            sorbaRendez();
            if (neighbours == null) return null;
            List<Crossing> kimenet = new List<Crossing>();
            for (int i = 0; i < neighbours.Count; i++)
                kimenet.Add(neighbours[i].road.NextCros(this));
            return kimenet;
        }
        public Crossing Kovetkezo(Crossing crossing, bool jobbra)
        {
            if (crossing == null) return null;
            List<Crossing> list = getSzomszedok();
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
