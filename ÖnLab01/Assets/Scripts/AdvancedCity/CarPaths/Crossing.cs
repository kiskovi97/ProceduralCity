﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class Crossing
    {
        public bool main;
        readonly float zebra = 0.7f;
        public bool tram = false;
        public Vector3 center;
        List<Neighbor> szomszedok;
        GameObjectGenerator generator;
        public class Neighbor
        {
            public Neighbor(Road road)
            {
                szomszedRoad = road;
            }
            public Road szomszedRoad;
            public HelpLine helpline;
            public CarPath carpath;
        }
        public class CarPath
        {
            public MovementPoint[] others;
            public MovementPoint[] bemenet;
            public MovementPoint[] kimenet;
            public MovementPoint balCross;
            public MovementPoint jobbCross;
            public MeshRenderer[] lamps;
        }
        public class HelpLine
        {
            public Vector3[] mainline;
            public Vector3[] sideline;
            public Vector3 roadedgecross;
            public Vector3 sidecross;
        }

        public Crossing(Vector3 centerpoint, bool inputmain, bool tramInput, GameObjectGenerator generatorbe)
        {
            center = centerpoint;
            szomszedok = new List<Neighbor>();
            main = inputmain;
            tram = tramInput;
            generator = generatorbe;
        }

        public void AddSzomszed(Road ujszomszed)
        {
            if (szomszedok == null)
            {
                Debug.Log("Crossing.AddSzomszed Initialization ERROR");
                return;
            }
            szomszedok.Add(new Neighbor(ujszomszed));
        }
        public void AddLines(Vector3[] mainline, Vector3[] sideline, Vector3 roadedgecross, Vector3 sidewalkcorss, Road road)
        {
            if (mainline == null || sideline == null) return;
            foreach (Neighbor szomszed in szomszedok)
            {
                if (szomszed.szomszedRoad.Equals(road))
                {
                    szomszed.helpline = new HelpLine();
                    szomszed.helpline.mainline = mainline;
                    szomszed.helpline.sideline = sideline;
                    szomszed.helpline.roadedgecross = roadedgecross;
                    szomszed.helpline.sidecross = sidewalkcorss;
                }
            }
        }

        private void ujraRendez()
        {
            for (int i = 0; i < szomszedok.Count - 1; i++)
            {
                Vector3 eddigiirany = szomszedok[i].szomszedRoad.NextCros(this).center - center;
                float eddigiszog = 360;
                int z = i;
                for (int j = i + 1; j < szomszedok.Count; j++)
                {
                    Vector3 masirany = szomszedok[j].szomszedRoad.NextCros(this).center - center;
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
                    Neighbor tmp = szomszedok[i + 1];
                    szomszedok[i + 1] = szomszedok[z];
                    szomszedok[z] = tmp;
                }
            }
        }
        // Calculating and Connecting the Movement Points ... how can a Vehicle get throw here
        public void carsPathSetting()
        {
            ujraRendez();
            for (int i = szomszedok.Count - 1; i >= 0; i--)
            {
                int jobbra = i + 1;
                if (jobbra > szomszedok.Count - 1) jobbra = 0;
                MakeMovePoints(i, jobbra);
            }

            for (int i = szomszedok.Count - 1; i >= 0; i--)
            {
                int jobbra = i + 1;
                if (jobbra > szomszedok.Count - 1) jobbra = 0;
                MakeRoads(i, jobbra);
            }

            if (szomszedok.Count > 2)
                for (int i = 0; i < szomszedok.Count; i++)
                {
                    for (int j = 0; j < szomszedok.Count; j++)
                    {
                        if (j == i) continue;
                        Vector3 iDir = szomszedok[i].szomszedRoad.getDir(this);
                        Vector3 jDir = szomszedok[j].szomszedRoad.getDir(this);
                        szomszedok[i].carpath.others = szomszedok[i].carpath.others.Concat(
                            MovementPoint.Connect(szomszedok[i].carpath.bemenet, szomszedok[j].carpath.kimenet, szomszedok[i].szomszedRoad.tram, szomszedok[j].szomszedRoad.tram, iDir, jDir * -1))
                            .ToArray();
                    }
                }
            if (szomszedok.Count == 2)
            {
                Vector3 iDir = szomszedok[0].szomszedRoad.getDir(this);
                Vector3 jDir = szomszedok[1].szomszedRoad.getDir(this);
                szomszedok[0].carpath.others =
                    MovementPoint.Connect(szomszedok[0].carpath.bemenet, szomszedok[1].carpath.kimenet, szomszedok[0].szomszedRoad.tram, szomszedok[1].szomszedRoad.tram, iDir, jDir * -1);
                szomszedok[1].carpath.others =
                    MovementPoint.Connect(szomszedok[1].carpath.bemenet, szomszedok[0].carpath.kimenet, szomszedok[1].szomszedRoad.tram, szomszedok[0].szomszedRoad.tram, jDir, iDir * -1);
            }
            /* if (szomszedok.Count == 1)
             {
                 Vector3 iDir = szomszedok[0].szomszedRoad.getDir();
                 MovementPoint.Connect(szomszedok[0].carpath.bemenet, szomszedok[0].carpath.kimenet, szomszedok[0].szomszedRoad.tram, szomszedok[0].szomszedRoad.tram, iDir, iDir);
             }*/
        }

        public int nyitott = 0;
        public bool stop = false;
        public void Valt()
        {
            if (szomszedok.Count < 3) return;
            foreach (Neighbor szomszed in szomszedok)
            {
                for (int i = 0; i < szomszed.carpath.bemenet.Length; i++)
                {
                    MovementPoint point = szomszed.carpath.bemenet[i];
                    point.Nyitott(false);
                    if (szomszed.szomszedRoad.tram && i == szomszed.carpath.bemenet.Length - 1)
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
                if (nyitott > szomszedok.Count - 1) nyitott = 0;
                Neighbor szomszed = szomszedok[nyitott];
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
                Neighbor szomszed = szomszedok[nyitott];
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
                if (next > szomszedok.Count - 1) next = 0;
                szomszed = szomszedok[next];
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

        void MakeMovePoints(int i, int jobb)
        {
            Vector3[] line = szomszedok[i].helpline.mainline;
            Vector3 othercenter = szomszedok[i].szomszedRoad.NextCros(this).center;
            Vector3 direction = (othercenter - center).normalized;
            if (line != null)
            {
                CarPath carpath = new CarPath();
                int thissavok = szomszedok[i].szomszedRoad.Savok();
                carpath.balCross = new MovementPoint((szomszedok[i].helpline.roadedgecross + szomszedok[i].helpline.sidecross) / 2);
                carpath.jobbCross = new MovementPoint((szomszedok[jobb].helpline.roadedgecross + szomszedok[jobb].helpline.sidecross) / 2);
                carpath.bemenet = new MovementPoint[thissavok];
                carpath.kimenet = new MovementPoint[thissavok];
                carpath.others = new MovementPoint[0];
                for (int j = 0; j < thissavok; j++)
                {
                    int a = (1 + j * 2);
                    int b = thissavok * 4 - a;
                    float realZebra = (szomszedok.Count == 2 || (szomszedok[i].szomszedRoad.tram && j== thissavok - 1)) ? 0 : zebra;
                    carpath.bemenet[j] = new MovementPoint((line[0] * a + line[1] * b) / (thissavok * 4) + direction * realZebra);
                    if (szomszedok.Count > 2) carpath.bemenet[j].Nyitott(false);
                    carpath.kimenet[j] = new MovementPoint((line[0] * b + line[1] * a) / (thissavok * 4));
                }
                szomszedok[i].carpath = carpath;
            }
        }

        void MakeRoads(int i, int jobbra)
        {
            CarPath carpath = szomszedok[i].carpath;
            List<MovementPoint> kimenet = new List<MovementPoint>(carpath.kimenet)
            {
                carpath.jobbCross
            };
            List<MovementPoint> bemenet = new List<MovementPoint>(carpath.bemenet)
            {
                carpath.balCross
            };
            carpath.jobbCross.ConnectPoint(szomszedok[jobbra].carpath.balCross);
            szomszedok[jobbra].carpath.balCross.ConnectPoint(carpath.jobbCross);
            carpath.balCross.ConnectPoint(carpath.jobbCross);
            carpath.jobbCross.ConnectPoint(carpath.balCross);
            szomszedok[i].szomszedRoad.addMovePoint(this, kimenet.ToArray(), bemenet.ToArray());
        }

        public void Draw(bool helplines_draw, bool depthtest)
        {
            List<Vector3> polygon = new List<Vector3>();
            Vector3 uplittle = new Vector3(0, 0.1f, 0);
            for (int j = 0; j < szomszedok.Count; j++)
            {
                Neighbor szomszed = szomszedok[j];
                int x = j + 1;
                if (x >= szomszedok.Count) x = 0;
                if (helplines_draw)
                {
                    szomszed.carpath.balCross.Draw(depthtest);
                    szomszed.carpath.jobbCross.Draw(depthtest);
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
                        distance ? szomszedok[x].helpline.sidecross : szomszed.helpline.sidecross,
                        distance ? szomszedok[x].helpline.sidecross : szomszed.helpline.sidecross, 0);
                }
                if (szomszedok.Count > 2)
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
            generator.CreateCrossing(polygon, 4);

            if (tram)
            {
                List<Neighbor> trams = szomszedok.FindAll(TramNeighbour);
                Vector3 up = new Vector3(0, 0.35f, 0);
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
                    generator.AddLine(egyik.center + up, tmp.center + up, 0.2f);
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
                    generator.AddLine(egyik.center + up, tmp.center + up, 0.2f);
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
                    generator.CreateRails(tmpCenter2, tmpCenter, tmpEgyik2, tmpEgyik, 3);
                    generator.CreateRails(tmpCenter, tmpCenter2, tmpMasik, tmpMasik2, 3);
                }
            }
        }

        private bool TramNeighbour(Neighbor szomszed)
        {
            return szomszed.szomszedRoad.tram;
        }

        private void makeLamps(Neighbor szomszed)
        {
            Vector3 forward = Vector3.Cross(szomszed.helpline.mainline[0] - szomszed.helpline.mainline[1], new Vector3(0, 1, 0));
            szomszed.carpath.lamps = new MeshRenderer[szomszed.carpath.bemenet.Length];
            if (szomszed.szomszedRoad.tram) szomszed.carpath.lamps = new MeshRenderer[szomszed.carpath.bemenet.Length - 1];
            for (int i = 0; i < szomszed.carpath.bemenet.Length; i++)
            {
                if (szomszed.szomszedRoad.tram && i == szomszed.carpath.bemenet.Length - 1) break;
                Vector3 pos = szomszed.carpath.bemenet[i].center;
                Vector3 meroleges = Vector3.Cross(szomszed.helpline.mainline[1] - szomszed.helpline.mainline[0], new Vector3(0, 1, 0));
                Vector3 intersect = MyMath.Intersect(szomszed.helpline.mainline[0], szomszed.helpline.mainline[1] - szomszed.helpline.mainline[0], pos, meroleges);
                GameObject lamp = generator.createCrossLamp(intersect + new Vector3(0, 0.3f, 0), forward);
                MeshRenderer renderer = lamp.GetComponent<MeshRenderer>();
                Material[] tomb = renderer.materials;
                RYG = tomb.ToArray();
                tomb[2] = tomb[0];
                tomb[3] = tomb[0];
                renderer.materials = tomb;
                generator.AddLine(intersect + new Vector3(0, 0.3f, 0), intersect + new Vector3(0, 0.4f, 0), 0.2f);
                if (szomszed.szomszedRoad.tram && i == szomszed.carpath.bemenet.Length - 1) break;
                szomszed.carpath.lamps[i] = renderer;
            }
            generator.AddLine(szomszed.helpline.mainline[1] + new Vector3(0, 0.4f, 0), (szomszed.helpline.mainline[1] + szomszed.helpline.mainline[0]) / 2 + new Vector3(0, 0.4f, 0), 0.2f);
            generator.AddLine(szomszed.helpline.mainline[1], szomszed.helpline.mainline[1] + new Vector3(0, 0.4f, 0), 0.6f);
        }

        private Material[] RYG;

        private int cars = 0;
        private int kimenetIndex = 0;
        public bool AddVehicle(Vehicle car)
        {
            if (szomszedok == null) return false;
            if (szomszedok.Count > cars)
            {
                car.setPoint(szomszedok[cars].carpath.kimenet[kimenetIndex++]);
                if (kimenetIndex >= szomszedok[cars].carpath.kimenet.Length || (kimenetIndex >= szomszedok[cars].carpath.kimenet.Length - 1 && szomszedok[cars].szomszedRoad.tram))
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
            if (szomszedok == null) return false;
            if (szomszedok.Count > peoples)
            {
                car.setPoint(szomszedok[peoples].carpath.jobbCross);
                peoples++;
                return true;
            }
            return false;
        }

        public void AddTram(Vehicle car, Vehicle car2)
        {
            bool carbool = false;
            foreach (Neighbor szomszed in szomszedok)
            {
                if (szomszed.szomszedRoad.tram && !carbool)
                {
                    carbool = true;
                    int max = szomszed.carpath.kimenet.Length - 1;
                    car.setPoint(szomszed.carpath.kimenet[max]);
                    continue;
                }
                if (szomszed.szomszedRoad.tram && carbool)
                {
                    int max = szomszed.carpath.kimenet.Length - 1;
                    car2.setPoint(szomszed.carpath.kimenet[max]);
                    return;
                }
            }
        }
        public bool HavePlace()
        {
            return szomszedok.Count > cars && szomszedok.Count > 1;
        }

        public bool HavePlaceForPeople()
        {
            return szomszedok.Count > peoples && szomszedok.Count > 1;
        }

        public bool isCrossing()
        {
            if (szomszedok == null) return false;
            return szomszedok.Count > 2;
        }

        public Road getSzomszedRoad(Vector3 to)
        {
            int i = 0;
            while (i < szomszedok.Count)
            {
                Vector3 masik = szomszedok[i].szomszedRoad.NextCros(this).center;
                if (masik.Equals(to))
                {
                    break;
                }
                i++;
            }
            if (i < szomszedok.Count)
                return szomszedok[i].szomszedRoad;
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
                return szomszedok[i].helpline.sidecross;
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
                return szomszedok[i].helpline.mainline[1];
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
                if (i < szomszedok.Count - 1)
                    return szomszedok[i + 1].helpline.sidecross;
                else
                    return szomszedok[0].helpline.sidecross;
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
                if (i < szomszedok.Count - 1)
                    return szomszedok[i].helpline.mainline[0];
                else
                    return szomszedok[i].helpline.mainline[0];
            }
            else
                return center;
        }
        public List<Crossing> getSzomszedok()
        {
            if (szomszedok == null) return null;
            List<Crossing> kimenet = new List<Crossing>();
            for (int i = 0; i < szomszedok.Count; i++)
                kimenet.Add(szomszedok[i].szomszedRoad.NextCros(this));
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
