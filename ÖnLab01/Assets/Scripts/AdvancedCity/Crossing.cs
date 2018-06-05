
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
        public Vector3 center;
        List<Neighbor> szomszedok;
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

        public Crossing(Vector3 centerpoint, bool inputmain)
        {
            center = centerpoint;
            szomszedok = new List<Neighbor>();
            main = inputmain;
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
                int balra = i + 1;
                if (balra > szomszedok.Count - 1) balra = 0;
                Vector3[] line = szomszedok[i].helpline.mainline;
                if (line != null)
                {
                    CarPath carpath = new CarPath();
                    if (szomszedok[i].szomszedRoad.NextCros(this).main && main)
                    {
                        carpath.bemenet = new MovementPoint[2];
                        carpath.kimenet = new MovementPoint[2];
                        carpath.felezo = new MovementPoint[2];
                        carpath.felezo[0] = new MovementPoint((szomszedok[i].helpline.roadedgecross * 3 + center) / 4);
                        carpath.felezo[1] = new MovementPoint((szomszedok[i].helpline.roadedgecross + center * 3) / 4);
                        carpath.bemenet[0] = new MovementPoint((line[0] + line[1] * 7) / 8);
                        carpath.kimenet[0] = new MovementPoint((line[0] * 7 + line[1]) / 8);
                        carpath.bemenet[1] = new MovementPoint((line[0] * 3 + line[1] * 5) / 8);
                        carpath.kimenet[1] = new MovementPoint((line[0] * 5 + line[1] * 3) / 8);
                    }
                    else
                    {
                        if (szomszedok[jobbra].szomszedRoad.NextCros(this).main && main)
                        {
                            carpath.felezo = new MovementPoint[2];
                            carpath.felezo[0] = new MovementPoint((szomszedok[i].helpline.roadedgecross * 3 + center) / 4);
                            carpath.felezo[1] = new MovementPoint((szomszedok[i].helpline.roadedgecross + center * 3) / 4);
                        }
                        else
                        {
                            carpath.felezo = new MovementPoint[1];
                            carpath.felezo[0] = new MovementPoint((szomszedok[i].helpline.roadedgecross + center) / 2);
                        }
                        carpath.bemenet = new MovementPoint[1];
                        carpath.kimenet = new MovementPoint[1];
                        carpath.bemenet[0] = new MovementPoint((line[0] + line[1] * 3) / 4);
                        carpath.kimenet[0] = new MovementPoint((line[0] * 3 + line[1]) / 4);
                    }
                    
                    szomszedok[i].szomszedRoad.addMovePoint(this, carpath.kimenet, carpath.bemenet);
                    szomszedok[i].carpath = carpath;
                }
            }

            if (szomszedok.Count > 2)
                for (int i = 0; i < szomszedok.Count; i++)
                {
                    int j = i - 1;
                    if (j < 0) j = szomszedok.Count - 1;

                    int x = i + 1;
                    if (x > szomszedok.Count - 1) x = 0;
                    Vector3[] line = szomszedok[i].helpline.mainline;
                    if (line != null)
                    {
                        CarPath carpath = szomszedok[i].carpath;
                        Vector3 to = (szomszedok[i].szomszedRoad.NextCros(this).center - center).normalized * 0.2f;
                        MovementPoint nextfelezo = szomszedok[j].carpath.felezo[0];
                        MovementPoint elozofelezo = szomszedok[x].carpath.felezo[0];
                        carpath.bemenet[0].ConnectPoint(carpath.felezo[0]);
                        carpath.felezo[0].ConnectPoint(nextfelezo);
                        elozofelezo.ConnectPoint(carpath.kimenet[0]);
                        if (carpath.bemenet.Length > 1 && carpath.felezo.Length > 1)
                        {
                            carpath.bemenet[1].ConnectPoint(carpath.felezo[1]);
                        }
                        if (szomszedok[x].carpath.felezo.Length > 1)
                        {
                            if (carpath.kimenet.Length > 1)
                                szomszedok[x].carpath.felezo[1].ConnectPoint(carpath.kimenet[1]);
                        }
                        if (szomszedok[j].carpath.felezo.Length > 1)
                        {
                            if (carpath.felezo.Length > 1)
                                carpath.felezo[1].ConnectPoint(szomszedok[j].carpath.felezo[1]);
                            else
                                carpath.felezo[0].ConnectPoint(szomszedok[j].carpath.felezo[1]);
                        } else if (carpath.felezo.Length > 1)
                            carpath.felezo[1].ConnectPoint(szomszedok[j].carpath.felezo[0]);

                    }
                }
            if (szomszedok.Count == 2)
            {
                bool nullmain = szomszedok[0].carpath.bemenet.Length > 1;
                bool egymain = szomszedok[1].carpath.bemenet.Length > 1;
                szomszedok[0].carpath.bemenet[0].ConnectPoint(szomszedok[0].carpath.felezo[0]);
                szomszedok[0].carpath.felezo[0].ConnectPoint(szomszedok[1].carpath.kimenet[0]);
                szomszedok[1].carpath.bemenet[0].ConnectPoint(szomszedok[1].carpath.felezo[0]);
                szomszedok[1].carpath.felezo[0].ConnectPoint(szomszedok[0].carpath.kimenet[0]);
                if (nullmain && egymain)
                {
                    szomszedok[0].carpath.bemenet[1].ConnectPoint(szomszedok[0].carpath.felezo[1]);
                    szomszedok[0].carpath.felezo[1].ConnectPoint(szomszedok[1].carpath.kimenet[1]);
                    szomszedok[1].carpath.bemenet[1].ConnectPoint(szomszedok[1].carpath.felezo[1]);
                    szomszedok[1].carpath.felezo[1].ConnectPoint(szomszedok[0].carpath.kimenet[1]);
                }
                else if(nullmain)
                {
                    if (szomszedok[0].carpath.felezo.Length > 1)
                    {
                        szomszedok[0].carpath.bemenet[1].ConnectPoint(szomszedok[0].carpath.felezo[1]);
                        szomszedok[0].carpath.felezo[1].ConnectPoint(szomszedok[1].carpath.kimenet[0]);
                    } else
                    {
                        szomszedok[0].carpath.bemenet[1].ConnectPoint(szomszedok[0].carpath.felezo[0]);
                        szomszedok[0].carpath.felezo[0].ConnectPoint(szomszedok[1].carpath.kimenet[0]);
                    }
                    if (szomszedok[1].carpath.felezo.Length > 1)
                    {
                        szomszedok[1].carpath.bemenet[0].ConnectPoint(szomszedok[1].carpath.felezo[1]);
                        szomszedok[1].carpath.felezo[1].ConnectPoint(szomszedok[0].carpath.kimenet[1]);
                    } else
                    {
                        szomszedok[1].carpath.bemenet[0].ConnectPoint(szomszedok[1].carpath.felezo[0]);
                        szomszedok[1].carpath.felezo[0].ConnectPoint(szomszedok[0].carpath.kimenet[1]);
                    }
                }
                else if (egymain)
                {
                    if (szomszedok[0].carpath.felezo.Length > 1)
                    {
                        szomszedok[0].carpath.bemenet[0].ConnectPoint(szomszedok[0].carpath.felezo[1]);
                        szomszedok[0].carpath.felezo[1].ConnectPoint(szomszedok[1].carpath.kimenet[1]);
                    } else
                    {
                        szomszedok[0].carpath.bemenet[0].ConnectPoint(szomszedok[0].carpath.felezo[0]);
                        szomszedok[0].carpath.felezo[0].ConnectPoint(szomszedok[1].carpath.kimenet[1]);
                    }
                    if (szomszedok[1].carpath.felezo.Length > 1)
                    {
                        szomszedok[1].carpath.bemenet[1].ConnectPoint(szomszedok[1].carpath.felezo[1]);
                        szomszedok[1].carpath.felezo[1].ConnectPoint(szomszedok[0].carpath.kimenet[0]);
                    } else
                    {
                        szomszedok[1].carpath.bemenet[1].ConnectPoint(szomszedok[1].carpath.felezo[0]);
                        szomszedok[1].carpath.felezo[0].ConnectPoint(szomszedok[0].carpath.kimenet[0]);
                    }
                }
            }
            if (szomszedok.Count == 1)
            {
                szomszedok[0].carpath.bemenet[0].ConnectPoint(szomszedok[0].carpath.kimenet[0]);
                if (szomszedok[0].carpath.bemenet.Length > 1)
                szomszedok[0].carpath.bemenet[1].ConnectPoint(szomszedok[0].carpath.kimenet[1]);
            }
        }
        

        public void Draw(bool helplines_draw, bool depthtest)
        {
            foreach (Neighbor szomszed in szomszedok)
            {
                Debug.DrawLine(szomszed.helpline.sideline[0], szomszed.helpline.sideline[1], Color.black, 1000, depthtest);
                if (helplines_draw)
                {
                    for (int i=0; i< szomszed.carpath.felezo.Length; i++)
                        szomszed.carpath.felezo[i].Draw(depthtest);
                    for (int i = 0; i < szomszed.carpath.kimenet.Length; i++)
                        szomszed.carpath.kimenet[i].Draw(depthtest);
                    for (int i = 0; i < szomszed.carpath.bemenet.Length; i++)
                        szomszed.carpath.bemenet[i].Draw(depthtest);
                }
            }
        }

        public void AddVehicle(Vehicle car)
        {
            if (szomszedok == null) return;
            if (szomszedok.Count > 0)
                car.setPoint(szomszedok[0].carpath.bemenet[0]);
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
