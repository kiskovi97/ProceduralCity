
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class Crossing
    {
        List<Road> szomszedok;
        List<CarPath> carpaths;
        List<HelpLine> helplines;
        public class CarPath
        {
            public MovementPoint felezo;
            public MovementPoint bemenet;
            public MovementPoint kimenet;
        }
        public class HelpLine
        {
            public Vector3[] mainline;
            public Vector3[] sideline;
            public Vector3 crosPoint;
        }
        public bool isCrossing()
        {
            return szomszedok.Count > 2;
        }
        public GraphPoint center;
        public Crossing(GraphPoint be)
        {
            center = be;
            szomszedok = new List<Road>();
            helplines = new List<HelpLine>();
            carpaths = new List<CarPath>();
        }
        public void AddSzomszed(Road be)
        {
            szomszedok.Add(be);
            helplines.Add(new HelpLine());
        }
        public void AddLines(Vector3[] mainline, Vector3[] sideline, Vector3 crossingpoint, Road road)
        {
            if (mainline == null || sideline == null) return;
            int x = szomszedok.IndexOf(road);
            if (x < szomszedok.Count)
            {
                helplines[x].mainline = mainline;
                helplines[x].sideline = sideline;
                helplines[x].crosPoint = crossingpoint;
            }
        }
        public void carLineSetting()
        {
            
            for (int i = 0; i < helplines.Count; i++)
            {
                int jobbra = i - 1;
                if (jobbra < 0) jobbra = helplines.Count - 1;
                int balra = i + 1;
                if (balra > helplines.Count - 1) balra = 0;
                Vector3[] line = helplines[i].mainline;
                if (line != null)
                {
                    CarPath carpath = new CarPath();
                    carpath.felezo = new MovementPoint( (helplines[i].crosPoint + center.position) / 2);
                    carpath.bemenet = new MovementPoint((line[0] + line[1] * 3) / 4);
                    carpath.kimenet = new MovementPoint((line[0] * 3 + line[1]) / 4);
                    szomszedok[i].addMovePoint(this, carpath.kimenet, carpath.bemenet);
                    carpaths.Add(carpath);
                }
            }

            
                for (int i = 0; i < helplines.Count; i++)
                {
                    int j = i - 1;
                    if (j < 0) j = helplines.Count - 1;

                    int x = i + 1;
                    if (x > helplines.Count - 1) x = 0;
                    Vector3[] line = helplines[i].mainline;
                    if (line != null)
                    {
                        CarPath carpath = carpaths[i];
                        Vector3 to = (szomszedok[i].NextCros(this).center.position - center.position).normalized * 0.2f;
                        Debug.DrawLine(line[0] + to, line[1] + to, Color.white, 1000, false);
                        Debug.DrawLine(line[0] + to * 2, line[1] + to * 2, Color.white, 1000, false);
                        MovementPoint nextfelezo = carpaths[j].felezo;
                        MovementPoint elozofelezo = carpaths[x].felezo;

                        carpath.bemenet.ConnectPoint(carpath.felezo);
                        carpath.felezo.ConnectPoint(nextfelezo);
                        elozofelezo.ConnectPoint(carpath.kimenet);
                    }

                }

        }
        public void Draw()
        {

            foreach (HelpLine line in helplines)
                if (line != null)
                    Debug.DrawLine(line.sideline[0], line.sideline[1], Color.black, 1000, false);
            foreach (CarPath carpath in carpaths)
            {
               // carpath.felezo.Draw();
               // carpath.kimenet.Draw();
               // carpath.bemenet.Draw();
            }
        }
        public Road getSzomszedRoad(GraphPoint to)
        {
            int i = 0;
            while (i < szomszedok.Count)
            {
                GraphPoint masik = szomszedok[i].NextCros(this).center;
                if (masik.Equals(to))
                {
                    break;
                }
                i++;
            }
            if (i < szomszedok.Count)
                return szomszedok[i];
            else
                return null;
        }
        public void ujraRendez()
        {
            
            for (int i = 0; i < szomszedok.Count - 1; i++)
            {
                Vector3 szomszed = szomszedok[i].NextCros(this).center.position;
                Vector3 eddigiirany = szomszed - center.position;
                float eddigiszog = 360;
                int z = i;
                for (int j = i + 1; j < szomszedok.Count; j++)
                {
                    Vector3 masikszomszed = szomszedok[j].NextCros(this).center.position;
                    Vector3 masirany = masikszomszed - center.position;
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
                    Road tmp = szomszedok[i + 1];
                    HelpLine tmphelpline = helplines[i + 1];
                    szomszedok[i + 1] = szomszedok[z];
                    helplines[i + 1] = helplines[z];
                    szomszedok[z] = tmp;
                    helplines[z] = tmphelpline;
                }
            }
        }
        public void AddVehicle(Vehicle car)
        {
            if (carpaths == null) return;
            if (carpaths.Count>0)
            car.setPoint(carpaths[0].bemenet);
        }
    }
}
