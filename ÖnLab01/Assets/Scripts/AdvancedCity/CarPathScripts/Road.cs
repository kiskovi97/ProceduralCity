
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    public class Road
    {
        readonly float zebra = 0.7f;
        public int sav = 0;
        public bool tram;
        bool stopping = false;

        Crossing oneCrossing;
        HelpLine oneLine;
        CarPath oneCarpath;

        Crossing otherCrossing;
        HelpLine otherLine;
        CarPath otherCarpath;

        List<MovementPoint> others = new List<MovementPoint>();
        IObjectGenerator generator;

        public int Savok()
        {
            return sav;
        }

        public Road(IObjectGenerator generator)
        {
            oneCrossing = null;
            oneLine = null;
            otherLine = null;
            otherCrossing = null;
            this.generator = generator;
        }

        public void SetCrossings(Crossing a, Crossing b, float roadSize)
        {
            oneCrossing = a;
            otherCrossing = b;
            if (a.main && b.main)
            {
                sav = 3;
            }
            else
                sav = 1;
            tram = a.tram && b.tram;
            if (tram) sav++;
            this.roadSize = roadSize;
        }

        public void AddHelpLine(Crossing crossing, HelpLine line)
        {
            if (crossing.Equals(oneCrossing))
            {
                oneLine = line;
            }
            if (crossing.Equals(otherCrossing))
            {
                otherLine = line;
            }
        }
        float roadSize;
        public void AddCarpath(Crossing be, CarPath carPath)
        {
            if (be.Equals(oneCrossing))
            {
                oneCarpath = carPath;
            }
            if (be.Equals(otherCrossing))
            {
                otherCarpath = carPath;
            }
            if (oneCarpath != null && otherCarpath != null)
            {
                MovementPoint[] egyik_be = oneCarpath.input;
                MovementPoint[] masik_ki = otherCarpath.output;
                MovementPoint[] egyik_ki = oneCarpath.output;
                MovementPoint[] masik_be = otherCarpath.input;
                for (int i = 0; i < oneCarpath.input.Length; i++)
                {
                    MovementPoint point01 = new MovementPoint((egyik_be[i].center * 2 + masik_ki[i].center) / 3);
                    MovementPoint point02 = new MovementPoint((egyik_be[i].center + masik_ki[i].center * 2) / 3);
                    egyik_be[i].ConnectPoint(point01);
                    point01.ConnectPoint(point02);
                    point02.ConnectPoint(masik_ki[i]);
                    others.Add(point01);
                    others.Add(point02);
                    egyik_be[i].SetDirection(otherCrossing.center - oneCrossing.center);
                    masik_ki[i].SetDirection(otherCrossing.center - oneCrossing.center);
                    point01.SetDirection(otherCrossing.center - oneCrossing.center);
                    point02.SetDirection(otherCrossing.center - oneCrossing.center);

                    MovementPoint point03 = new MovementPoint((masik_be[i].center * 2 + egyik_ki[i].center) / 3);
                    MovementPoint point04 = new MovementPoint((masik_be[i].center + egyik_ki[i].center * 2) / 3);
                    masik_be[i].ConnectPoint(point03);
                    point03.ConnectPoint(point04);
                    point04.ConnectPoint(egyik_ki[i]);
                    others.Add(point03);
                    others.Add(point04);
                    masik_be[i].SetDirection(oneCrossing.center - otherCrossing.center);
                    egyik_ki[i].SetDirection(oneCrossing.center - otherCrossing.center);
                    point03.SetDirection(otherCrossing.center - oneCrossing.center);
                    point04.SetDirection(otherCrossing.center - oneCrossing.center);
                }

                if (oneCarpath.tramInput != null && oneCarpath.tramOutput != null && otherCarpath.tramInput != null && otherCarpath.tramOutput != null)
                {
                    Vector3 dir = (otherCrossing.center - oneCrossing.center).normalized;
                    if (Random.value < 0.5f)
                    {
                        Vector3 meroleges = MyMath.Meroleges(oneCrossing.center, otherCrossing.center).normalized * (roadSize / 4);
                        MakeStopping(oneCarpath.tramInput, otherCarpath.tramOutput, dir, -meroleges);
                        MakeStopping(otherCarpath.tramInput, oneCarpath.tramOutput, -dir, meroleges);
                        stopping = true;
                    }
                    else
                    {
                        oneCarpath.tramInput.ConnectPoint(otherCarpath.tramOutput);
                        otherCarpath.tramInput.ConnectPoint(oneCarpath.tramOutput);
                        oneCarpath.tramInput.SetDirection(dir);
                        otherCarpath.tramOutput.SetDirection(dir);
                        otherCarpath.tramInput.SetDirection(-dir);
                        oneCarpath.tramOutput.SetDirection(-dir);
                    }
                }
                if (egyik_be.Length > 1)
                {
                    others[0].ConnectPoint(others[5]);
                    others[4].ConnectPoint(others[1]);
                    others[2].ConnectPoint(others[7]);
                    others[6].ConnectPoint(others[3]);
                }

                if (egyik_be.Length > 2)
                {
                    others[4].ConnectPoint(others[9]);
                    others[8].ConnectPoint(others[5]);
                    others[6].ConnectPoint(others[11]);
                    others[10].ConnectPoint(others[7]);
                }
            }
        }

        private void MakeStopping(MovementPoint one, MovementPoint other, Vector3 dir, Vector3 meroleges)
        {
            MovementPoint point01 = new MovementPoint((one.center * 3 + other.center) / 4);
            MovementPoint point02 = new MovementPoint((one.center + other.center * 3) / 4);
            MovementPoint point01in = new MovementPoint((one.center * 2 + other.center) / 3 - meroleges);
            MovementPoint point02in = new MovementPoint((one.center + other.center * 2) / 3 - meroleges);
            one.ConnectPoint(point01);
            point01.ConnectPoint(point01in);
            point01in.ConnectPoint(point02in);
            point02in.ConnectPoint(point02);
            point02.ConnectPoint(other);
            others.Add(point01);
            others.Add(point02);
            others.Add(point01in);
            others.Add(point02in);
            one.SetDirection(dir);
            other.SetDirection(dir);
            point01.SetDirection(dir);
            point02.SetDirection(dir);
            point01in.SetDirection(dir);
            point02in.SetDirection(dir);
            point02in.megallo = true;
        }

        public Vector3 GetDir(Crossing cros)
        {
            if (cros == oneCrossing)
                return oneCrossing.center - otherCrossing.center;
            else
                return otherCrossing.center - oneCrossing.center;
        }

        public void Draw(bool helplines_draw, bool depthtest, bool lamps, bool trams)
        {
            if (oneLine != null && otherLine != null)
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
                    for (int i = 0; i < 3; i++)
                    {
                        Vector3 center = (otherLine.mainLine[0] * (i + 1) + oneLine.mainLine[1] * (db - i)) / (db + 1);
                        Vector3 centerOther = (oneLine.mainLine[0] * (i + 1) + otherLine.mainLine[1] * (db - i)) / (db + 1);
                        generator.CreateSideLamp(center, oneLine.mainLine[0] - oneLine.mainLine[1]);
                        generator.CreateSideLamp(centerOther, oneLine.mainLine[1] - oneLine.mainLine[0]);
                    }
                generator.CreateRoad(otherCrossing.MainCross(oneCrossing), oneCrossing.OtherMainCross(otherCrossing), otherCrossing.SideCross(oneCrossing), oneCrossing.OtherSideCross(otherCrossing),
                    1, false, true);
                generator.CreateRoad(oneCrossing.MainCross(otherCrossing), otherCrossing.OtherMainCross(oneCrossing), oneCrossing.SideCross(otherCrossing), otherCrossing.OtherSideCross(oneCrossing),
                    1, false, true);
                generator.CreateRoad(oneLine.mainLine[0], otherLine.mainLine[1], oneLine.mainLine[1], otherLine.mainLine[0], sav, tram, false,
                    oneCrossing.IsCorssing() ? zebra : 0, otherCrossing.IsCorssing() ? zebra : 0);
                if (tram && trams)
                {
                    Vector3 dir = (otherCrossing.center - oneCrossing.center).normalized;
                    Vector3 meroleges = MyMath.Meroleges(oneCrossing.center, otherCrossing.center).normalized;
                    generator.CreateRails(oneCarpath.tramInput, otherCarpath.tramOutput, roadSize / 4.0f, meroleges, 3);
                    generator.CreateRails(otherCarpath.tramInput, oneCarpath.tramOutput, roadSize / 4.0f, -meroleges, 3);
                    if (stopping)
                    {
                        Vector3 a = (oneCarpath.tramInput.center * 2 + otherCarpath.tramOutput.center) / 3 - meroleges * roadSize / 3;
                        Vector3 c = (oneCarpath.tramInput.center + otherCarpath.tramOutput.center * 2) / 3 - meroleges * roadSize / 3;
                        Vector3 b = (otherCarpath.tramInput.center + oneCarpath.tramOutput.center * 2) / 3 + meroleges * roadSize / 3;
                        Vector3 d = (otherCarpath.tramInput.center * 2 + oneCarpath.tramOutput.center) / 3 + meroleges * roadSize / 3;
                        generator.AddStoppingMesh(a, b, c, d);
                    }
                    generator.AddLine(oneCarpath.tramInput.center, otherCarpath.tramOutput.center, 0.15f, 0.35f, 0.35f);
                    generator.AddLine(otherCarpath.tramInput.center, oneCarpath.tramOutput.center, 0.15f, 0.35f, 0.35f);
                }

            }
        }

        public Crossing NextCros(Crossing a)
        {
            if (oneCrossing.Equals(a))
            {
                return otherCrossing;
            }
            if (otherCrossing.Equals(a))
            {
                return oneCrossing;
            }
            return null;
        }
    }
}
