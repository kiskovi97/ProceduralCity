
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
            public MovementPoint[] felezo;
            public MovementPoint[] bemenet;
            public MovementPoint[] kimenet;
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
            for (int i = 0; i < szomszedok.Count; i++)
            {
                int jobbra = i - 1;
                if (jobbra < 0) jobbra = szomszedok.Count - 1;
                MakeMovePoints(i, jobbra);
            }

            if (szomszedok.Count > 2)
                for (int i = 0; i < szomszedok.Count; i++)
                {
                    for (int j= 0; j< szomszedok.Count; j++)
                    {
                        if (j == i) continue;
                        MovementPoint.Connect(szomszedok[i].carpath.bemenet, szomszedok[i].carpath.felezo);
                        MovementPoint.Connect(szomszedok[i].carpath.felezo, szomszedok[j].carpath.kimenet);
                        if (szomszedok[i].szomszedRoad.tram && szomszedok[j].szomszedRoad.tram)
                        {
                            int max = szomszedok[i].carpath.felezo.Length - 1;
                            for (int x = 0; x < szomszedok[j].carpath.kimenet.Length - 1; x++)
                            {
                                szomszedok[i].carpath.felezo[max].DisConnectPoint(szomszedok[j].carpath.kimenet[x]);
                                szomszedok[j].carpath.kimenet[x].DisConnectPoint(szomszedok[i].carpath.felezo[max]);
                            }
                        }

                        if (szomszedok[i].szomszedRoad.tram && !szomszedok[j].szomszedRoad.tram)
                        {
                            int max = szomszedok[i].carpath.felezo.Length - 1;
                            for (int x = 0; x < szomszedok[j].carpath.kimenet.Length; x++)
                            {
                                szomszedok[i].carpath.felezo[max].DisConnectPoint(szomszedok[j].carpath.kimenet[x]);
                                szomszedok[j].carpath.kimenet[x].DisConnectPoint(szomszedok[i].carpath.felezo[max]);
                            }
                        }

                        if (!szomszedok[i].szomszedRoad.tram && szomszedok[j].szomszedRoad.tram)
                        {
                            int max = szomszedok[j].carpath.kimenet.Length - 1;
                            for (int x = 0; x < szomszedok[i].carpath.felezo.Length; x++)
                            {
                                szomszedok[j].carpath.kimenet[max].DisConnectPoint(szomszedok[i].carpath.felezo[x]);
                                szomszedok[i].carpath.felezo[x].DisConnectPoint(szomszedok[j].carpath.kimenet[max]);
                            }
                            for (int x = 1; x < szomszedok[i].carpath.felezo.Length; x++)
                            {
                                szomszedok[i].carpath.felezo[x].ConnectPoint(szomszedok[i].carpath.felezo[x - 1]);
                            }
                        }
                    }
                }
            if (szomszedok.Count == 2)
            {
                MovementPoint.Connect(szomszedok[0].carpath.bemenet, szomszedok[0].carpath.felezo);
                MovementPoint.Connect(szomszedok[0].carpath.felezo, szomszedok[1].carpath.kimenet);
                MovementPoint.Connect(szomszedok[1].carpath.bemenet, szomszedok[1].carpath.felezo);
                MovementPoint.Connect(szomszedok[1].carpath.felezo, szomszedok[0].carpath.kimenet);
            }
            if (szomszedok.Count == 1)
            {
                MovementPoint.Connect(szomszedok[0].carpath.bemenet, szomszedok[0].carpath.kimenet);
            }
        }

        public void carsPathSetting2()
        {
            ujraRendez();
            for (int i = 0; i < szomszedok.Count; i++)
            {
                int jobbra = i - 1;
                if (jobbra < 0) jobbra = szomszedok.Count - 1;
                MakeMovePoints(i, jobbra);
            }

            if (szomszedok.Count > 2)
                for (int i = 0; i < szomszedok.Count; i++)
                {
                    int jobbra = i - 1;
                    if (jobbra < 0) jobbra = szomszedok.Count - 1;
                    int balra = i + 1;
                    if (balra > szomszedok.Count - 1) balra = 0;
                    MovementPoint.Connect(szomszedok[i].carpath.bemenet, szomszedok[i].carpath.felezo);
                    MovementPoint.Connect(szomszedok[i].carpath.felezo, szomszedok[jobbra].carpath.felezo);
                    MovementPoint.Connect(szomszedok[balra].carpath.felezo, szomszedok[i].carpath.kimenet);
                }
            if (szomszedok.Count == 2)
            {
                MovementPoint.Connect(szomszedok[0].carpath.bemenet, szomszedok[0].carpath.felezo);
                MovementPoint.Connect(szomszedok[1].carpath.felezo, szomszedok[0].carpath.kimenet);
                MovementPoint.Connect(szomszedok[1].carpath.bemenet, szomszedok[1].carpath.felezo);
                MovementPoint.Connect(szomszedok[0].carpath.felezo, szomszedok[1].carpath.kimenet);
            }
            if (szomszedok.Count == 1)
            {
                MovementPoint.Connect(szomszedok[0].carpath.bemenet, szomszedok[0].carpath.felezo);
                MovementPoint.Connect(szomszedok[0].carpath.felezo, szomszedok[0].carpath.kimenet);
            }
        }

        public int nyitott = 0;
        public bool stop = false;
        public void Valt()
        {
            if (szomszedok.Count < 3) return;
            foreach (Neighbor szomszed in szomszedok)
            {
                for (int i=0; i< szomszed.carpath.bemenet.Length; i++)
                {
                    MovementPoint point = szomszed.carpath.bemenet[i];
                        point.Nyitott(false);
                    if (szomszed.szomszedRoad.tram && i == szomszed.carpath.bemenet.Length-1)
                        point.Nyitott(true);
                }
            }
            if (!stop)
            {
                nyitott++;
                if (nyitott > szomszedok.Count - 1) nyitott = 0;
                Neighbor neighbour = szomszedok[nyitott];
                foreach (MovementPoint point in neighbour.carpath.bemenet)
                {
                    point.Nyitott(true);
                }
            }
            stop = !stop;
        }

        void MakeMovePoints(int i, int jobbra)
        {
            Vector3[] line = szomszedok[i].helpline.mainline;
            Vector3 othercenter = szomszedok[i].szomszedRoad.NextCros(this).center;
            Vector3 direction = (othercenter - center).normalized;
            if (line != null)
            {
                CarPath carpath = new CarPath();
                int thissavok = szomszedok[i].szomszedRoad.Savok();
                int thatsavok = Math.Max(szomszedok[jobbra].szomszedRoad.Savok(),thissavok);
                carpath.bemenet = new MovementPoint[thissavok];
                carpath.kimenet = new MovementPoint[thissavok];
                carpath.felezo = new MovementPoint[thatsavok];
                for (int j = 0; j < thissavok; j++)
                {
                    int a = (1 + j * 2);
                    int b = thissavok * 4 - a;
                    carpath.bemenet[j] = new MovementPoint((line[0] * a + line[1] * b) / (thissavok * 4) + direction * 0.2f);
                    if (szomszedok.Count > 2) carpath.bemenet[j].Nyitott(false);
                    carpath.kimenet[j] = new MovementPoint((line[0] * b + line[1] * a) / (thissavok * 4) + direction * 0.2f);
                }
                for (int j = 0; j < thatsavok; j++)
                {
                    int a = (1 + j * 2);
                    int b = thatsavok * 2 - a;
                    carpath.felezo[j] = new MovementPoint((szomszedok[i].helpline.roadedgecross * b + center * a) / (thatsavok * 2));
                }
                szomszedok[i].szomszedRoad.addMovePoint(this, carpath.kimenet, carpath.bemenet);
                szomszedok[i].carpath = carpath;
            }
        }

        public void Draw(bool helplines_draw, bool depthtest)
        {
            for (int j = 0; j < szomszedok.Count; j++)
            {
                Neighbor szomszed = szomszedok[j];
                int x = j + 1;
                if (x >= szomszedok.Count) x = 0;
                if (helplines_draw)
                {
                    for (int i = 0; i < szomszed.carpath.felezo.Length; i++)
                        szomszed.carpath.felezo[i].Draw(depthtest);
                    for (int i = 0; i < szomszed.carpath.kimenet.Length; i++)
                        szomszed.carpath.kimenet[i].Draw(depthtest);
                    for (int i = 0; i < szomszed.carpath.bemenet.Length; i++)
                        szomszed.carpath.bemenet[i].Draw(depthtest);
                }
                int sav = szomszedok[x].carpath.felezo.Length;
                if (szomszed.helpline.mainline[0] != szomszed.helpline.sideline[1])
                {
                    generator.CreateRoad(szomszed.helpline.mainline[0], szomszed.helpline.mainline[1], center, szomszed.helpline.sideline[1], 
                          1);
                }
                else
                {
                    generator.CreateRoad(szomszed.helpline.mainline[0], szomszed.helpline.mainline[1], 
                        szomszed.helpline.sideline[0],center, 1);
                }
            }

        }
        private int cars = 0;
        private int kimenetIndex = 0;
        public bool AddVehicle(Vehicle car)
        {
            if (szomszedok == null) return false;
            if (szomszedok.Count > cars)
            {
                car.setPoint(szomszedok[cars].carpath.kimenet[kimenetIndex++]);
                if (kimenetIndex>= szomszedok[cars].carpath.kimenet.Length || (kimenetIndex >= szomszedok[cars].carpath.kimenet.Length - 1 && szomszedok[cars].szomszedRoad.tram))
                {
                    cars++;
                    kimenetIndex = 0;
                }
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
                    int max = szomszed.carpath.kimenet.Length-1;
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
                return szomszedok[i].helpline.roadedgecross;
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
                if (i < szomszedok.Count-1)
                    return szomszedok[i+1].helpline.sidecross;
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
                    return szomszedok[i + 1].helpline.roadedgecross;
                else
                    return szomszedok[0].helpline.roadedgecross;
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
