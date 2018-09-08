
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class Road
    {
        Crossing egyik;
        Vector3[] line_egyik;
        MovementPoint[] egyik_be;
        MovementPoint[] egyik_ki;
        int sav = 0;
        Crossing masik;
        Vector3[] line_masik;
        MovementPoint[] masik_be;
        MovementPoint[] masik_ki;
        public bool tram;
        GameObjectGenerator generator;
        public int Savok()
        {
            return sav;
        }
        public Road(GameObjectGenerator generatorbe)
        {
            egyik = null;
            line_egyik = null;
            line_masik = null;
            masik = null;
            generator = generatorbe;
        }
        public void setSzomszedok(Crossing a, Crossing b)
        {
            egyik = a;
            masik = b;
            if (a.main && b.main)
            {
                sav = 3;
            }
            else
                sav = 1;
            if (a.tram && b.tram) sav++;
            tram = (a.tram && b.tram);
        }
        public void addLine(Crossing be, Vector3 a, Vector3 b)
        {
            if (be.Equals(egyik))
            {
                line_egyik = new Vector3[2];
                line_egyik[0] = a;
                line_egyik[1] = b;
            }
            if (be.Equals(masik))
            {
                line_masik = new Vector3[2];
                line_masik[0] = a;
                line_masik[1] = b;
            }
        }
        public void addMovePoint(Crossing be, MovementPoint[] befele, MovementPoint[] kifele)
        {
            if (be.Equals(egyik))
            {
                egyik_be = befele;
                egyik_ki = kifele;
            }
            if (be.Equals(masik))
            {
                masik_be = befele;
                masik_ki = kifele;
            }
            if (egyik_be != null && masik_ki != null)
            {
                for (int i = sav; i > 0; i--)
                {
                    int j = egyik_be.Length - i;
                    if (j < 0) j = 0;
                    int x = masik_ki.Length - i;
                    if (x < 0) j = 0;
                    egyik_be[j].ConnectPoint(masik_ki[x]);
                    egyik_be[j].setDirection(masik.center - egyik.center);
                    masik_ki[x].setDirection(masik.center - egyik.center);

                    j = egyik_ki.Length - i;
                    if (j < 0) j = 0;
                    x = masik_be.Length - i;
                    if (x < 0) j = 0;
                    masik_be[x].ConnectPoint(egyik_ki[j]);
                    masik_be[x].setDirection(egyik.center - masik.center);
                    egyik_ki[j].setDirection(egyik.center - masik.center);
                }
            }
        }

        public Vector3[] getTram(Crossing cros)
        {
            if (!tram) return null;
            Vector3 dir = (line_egyik[0] - line_masik[1]).normalized * 0.2f;
            if (cros == masik)
            {
                return new Vector3[]
                {
                    (masik_ki[sav - 2].center + masik_ki[sav - 1].center) / 2 - dir, (masik_be[sav - 2].center + masik_be[sav - 1].center) / 2 - dir
                };
            }
            if (cros == egyik)
            {
                return new Vector3[]
                {
                    (egyik_ki[sav - 2].center + egyik_ki[sav - 1].center) / 2 + dir, (egyik_be[sav - 2].center + egyik_be[sav - 1].center) / 2 + dir
                };
            }
            return null;
        }

        public Vector3 getDir(Crossing cros)
        {
            if (cros == egyik)
                return egyik.center - masik.center;
            else
                return masik.center - egyik.center;
        }

        public void Draw(bool depthtest)
        {
            if (line_egyik != null && line_masik != null)
            {

                generator.CreateRoad(masik.KeresztRoad(egyik), egyik.KeresztRoadMasik(masik), masik.Kereszt(egyik), egyik.KeresztMasik(masik), 0);
                generator.CreateRoad(egyik.KeresztRoad(masik), masik.KeresztRoadMasik(egyik), egyik.Kereszt(masik), masik.KeresztMasik(egyik), 0);
                Vector3 centerMasik = (line_masik[0] + line_masik[1]) / 2;
                Vector3 centerEgyik = (line_egyik[0] + line_egyik[1]) / 2;
                if (sav == 1 || sav == 0)
                {
                    generator.CreateRoad(line_egyik[0], line_masik[1], centerEgyik, centerMasik, 2);
                    generator.CreateRoad(line_masik[0], line_egyik[1], centerMasik, centerEgyik, 2);
                }
                else
                {
                    Vector3 dir = (line_egyik[0] - line_masik[1]).normalized * 0.2f;
                    generator.CreateRoad(line_egyik[0], line_masik[1], (egyik_be[0].center + egyik_be[1].center) / 2 + dir, (masik_ki[0].center + masik_ki[1].center) / 2 - dir, 2);
                    generator.CreateRoad(line_masik[0], line_egyik[1], (masik_be[0].center + masik_be[1].center) / 2 - dir, (egyik_ki[0].center + egyik_ki[1].center) / 2 + dir, 2);
                    for (int i = 1; i < sav - 1; i++)
                    {
                        generator.CreateRoad((masik_ki[i + 1].center + masik_ki[i].center) / 2 - dir, (egyik_be[i].center + egyik_be[i + 1].center) / 2 + dir,
                         (masik_ki[i - 1].center + masik_ki[i].center) / 2 - dir, (egyik_be[i].center + egyik_be[i - 1].center) / 2 + dir, 1);
                        generator.CreateRoad((egyik_ki[i].center + egyik_ki[i + 1].center) / 2 + dir, (masik_be[i + 1].center + masik_be[i].center) / 2 - dir,
                        (egyik_ki[i].center + egyik_ki[i - 1].center) / 2 + dir, (masik_be[i - 1].center + masik_be[i].center) / 2 - dir, 1);
                    }


                    {
                        generator.CreateRoad(centerMasik, centerEgyik,
                         (masik_ki[sav - 2].center + masik_ki[sav - 1].center) / 2 - dir, (egyik_be[sav - 2].center + egyik_be[sav - 1].center) / 2 + dir,
                         2);

                        generator.CreateRoad(centerEgyik, centerMasik,
                              (egyik_ki[sav - 2].center + egyik_ki[sav - 1].center) / 2 + dir, (masik_be[sav - 2].center + masik_be[sav - 1].center) / 2 - dir,
                              2);
                    }
                    if (tram)
                    {
                        generator.CreateRails(centerMasik + dir, centerEgyik - dir,
                         (masik_ki[sav - 2].center + masik_ki[sav - 1].center) / 2, (egyik_be[sav - 2].center + egyik_be[sav - 1].center) / 2,
                         3);

                        generator.CreateRails(centerEgyik - dir, centerMasik + dir,
                              (egyik_ki[sav - 2].center + egyik_ki[sav - 1].center) / 2, (masik_be[sav - 2].center + masik_be[sav - 1].center) / 2,
                              3);

                    }

                    if (egyik.tram && masik.tram)
                    {
                        Vector3 up = new Vector3(0, 0.4f, 0);
                        generator.AddLine(egyik_be.Last().center + up, masik_ki.Last().center + up, 0.15f);
                        generator.AddLine(masik_be.Last().center + up, egyik_ki.Last().center + up, 0.15f);
                    }
                }
            }
        }

        public bool isSame(Crossing a, Crossing b)
        {
            return ((egyik == a && masik == b) || (egyik == b && masik == a));
        }

        public Crossing NextCros(Crossing a)
        {
            if (egyik.Equals(a))
            {
                return masik;
            }
            if (masik.Equals(a))
            {
                return egyik;
            }
            return null;
        }
    }
}
