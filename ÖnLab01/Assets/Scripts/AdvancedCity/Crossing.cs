
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class Crossing
    {
        List<Road> szomszedok;
        List<Vector3[]> lines;
        List<Vector3[]> helplines;
        public GraphPoint center;
        public Crossing(GraphPoint be)
        {
            center = be;
            szomszedok = new List<Road>();
            lines = new List<Vector3[]>();
            helplines = new List<Vector3[]>();
        }
        public void AddSzomszed(Road be)
        {
            szomszedok.Add(be);
            lines.Add(new Vector3[2]);
            helplines.Add(new Vector3[2]);
        }
        public void AddLines(Vector3[] line, Vector3[] helpline, Road road)
        {
            if (line == null || helpline == null) return;
            int x = szomszedok.IndexOf(road);
            if (x < szomszedok.Count)
            {
                lines[x] = line;
                helplines[x] = helpline;
            }
        }
        public void Draw()
        {
            foreach (Vector3[] line in helplines)
                if (line != null)
                    Debug.DrawLine(line[0], line[1], Color.black, 1000, false);
            for ( int i= 0; i<lines.Count; i++)
            {
                Vector3[] line = lines[i];
                if (line != null)
                {
                    Vector3 to = (szomszedok[i].NextCros(this).center.position - center.position).normalized*0.2f;
                    Debug.DrawLine(line[0] + to, line[1]+ to, Color.white, 1000, false);
                    Debug.DrawLine(line[0] + to*2, line[1] + to*2, Color.white, 1000, false);
                }
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
         
    }
}
