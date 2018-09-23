
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class Road
    {
        readonly float zebra = 0.7f;
        Crossing egyik;
        Vector3[] line_egyik;
        MovementPoint[] egyik_be;
        MovementPoint[] egyik_ki;
        int sav = 0;
        Crossing masik;
        Vector3[] line_masik;
        MovementPoint[] masik_be;
        MovementPoint[] masik_ki;
        List<MovementPoint> others = new List<MovementPoint>();
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
                for (int i=0; i< egyik_be.Length; i++)
                {
                    MovementPoint point01 = new MovementPoint((egyik_be[i].center * 2 + masik_ki[i].center) / 3);
                    MovementPoint point02 = new MovementPoint((egyik_be[i].center + masik_ki[i].center * 2) / 3);
                    egyik_be[i].ConnectPoint(point01);
                    point01.ConnectPoint(point02);
                    point02.ConnectPoint(masik_ki[i]);
                    others.Add(point01);
                    others.Add(point02);
                    egyik_be[i].setDirection(masik.center - egyik.center);
                    masik_ki[i].setDirection(masik.center - egyik.center);
                    point01.setDirection(masik.center - egyik.center);
                    point02.setDirection(masik.center - egyik.center);

                    MovementPoint point03 = new MovementPoint((masik_be[i].center * 2 + egyik_ki[i].center) / 3);
                    MovementPoint point04 = new MovementPoint((masik_be[i].center + egyik_ki[i].center * 2) / 3);
                    masik_be[i].ConnectPoint(point03);
                    point03.ConnectPoint(point04);
                    point04.ConnectPoint(egyik_ki[i]);
                    others.Add(point03);
                    others.Add(point04);
                    masik_be[i].setDirection(egyik.center - masik.center);
                    egyik_ki[i].setDirection(egyik.center - masik.center);
                    point03.setDirection(masik.center - egyik.center);
                    point04.setDirection(masik.center - egyik.center);
                }

                if (egyik_be.Length > 3 || (egyik_be.Length>2 && !tram))
                {
                    others[0].ConnectPoint(others[5]);
                    others[4].ConnectPoint(others[1]);
                    others[2].ConnectPoint(others[7]);
                    others[6].ConnectPoint(others[3]);
                }

                if (egyik_be.Length > 4 || (egyik_be.Length > 3 && !tram))
                {
                    others[4].ConnectPoint(others[9]);
                    others[8].ConnectPoint(others[5]);
                    others[6].ConnectPoint(others[11]);
                    others[10].ConnectPoint(others[7]);
                }
            }
        }

        public Vector3 getDir(Crossing cros)
        {
            if (cros == egyik)
                return egyik.center - masik.center;
            else
                return masik.center - egyik.center;
        }

        public void Draw(bool helplines_draw, bool depthtest, bool lamps, bool trams)
        {
            if (line_egyik != null && line_masik != null)
            {

                if (helplines_draw)
                {
                    foreach (MovementPoint point in others)
                    {
                        point.Draw(depthtest);
                    }
                }

                int db = 3;
                if (lamps)
                    for (int i = 0; i < db; i++)
                    {
                        Vector3 center = (line_masik[0] * (i + 1) + line_egyik[1] * (db - i)) / (db + 1);
                        Vector3 centerOther = (line_egyik[0] * (i + 1) + line_masik[1] * (db - i)) / (db + 1);
                        generator.createSideLamp(center, line_masik[1] - line_masik[0]);
                        generator.createSideLamp(centerOther, line_masik[0] - line_masik[1]);
                    }
                generator.CreateRoad(masik.KeresztRoad(egyik), egyik.KeresztRoadMasik(masik), masik.Kereszt(egyik), egyik.KeresztMasik(masik), 1, false, true);
                generator.CreateRoad(egyik.KeresztRoad(masik), masik.KeresztRoadMasik(egyik), egyik.Kereszt(masik), masik.KeresztMasik(egyik), 1, false, true);
                Vector3 centerMasik = (line_masik[0] + line_masik[1]) / 2;
                Vector3 centerEgyik = (line_egyik[0] + line_egyik[1]) / 2;
                generator.CreateRoad(line_egyik[0], line_masik[1], line_egyik[1], line_masik[0], sav, tram, false, zebra);
                int max = sav * 2;
                if (tram && trams)
                {
                    generator.CreateRails(centerMasik, centerEgyik,
                     (line_masik[0] * (max / 2 - 1) + line_masik[1] * (max / 2 + 1)) / max, (line_egyik[0] * (max / 2 + 1) + line_egyik[1] * (max / 2 - 1)) / max,
                     3);

                    generator.CreateRails(centerEgyik, centerMasik,
                         (line_egyik[0] * (max / 2 - 1) + line_egyik[1] * (max / 2 + 1)) / max, (line_masik[0] * (max / 2 + 1) + line_masik[1] * (max / 2 - 1)) / max,
                          3);
                    Vector3 up = new Vector3(0, 0.35f, 0);
                    generator.AddLine(egyik_be[egyik_be.Length - 2].center + up, masik_ki[masik_ki.Length - 2].center + up, 0.15f);
                    generator.AddLine(masik_be[masik_be.Length - 2].center + up, egyik_ki[egyik_ki.Length - 2].center + up, 0.15f);
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
