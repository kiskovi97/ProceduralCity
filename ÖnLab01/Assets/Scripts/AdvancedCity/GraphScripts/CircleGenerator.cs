
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts.AdvancedCity
{
    public class CircleGenerator
    {
        List<List<GraphPoint>> circles = new List<List<GraphPoint>>();
        public List<GraphPoint> maxCircle(List<GraphPoint> list)
        {
            for (int i=0; i< list.Count; i++)
            {
                GenerateCirlces(list[i]);
            }
            List<GraphPoint> output = new List<GraphPoint>();
            foreach (List<GraphPoint> circle in circles)
            {
                if (output.Count < circle.Count) output = circle;
            }
            return output;
        }

         void GenerateCirlces(GraphPoint begining)
        {
            foreach (GraphPoint gp in begining.Szomszedok)
            {
                Next(new List<GraphPoint>() { begining, gp }, begining, gp,true);
            }
        }

        void Next(List<GraphPoint> list, GraphPoint elozo, GraphPoint mostani, bool jobbra)
        {
            GraphPoint next = mostani.kovetkezo(elozo, jobbra);
            if (next == list[0])
            {
                circles.Add(list);
                return;
            }
            if (list.Contains(next)) return;
            bool isGood = false;
            foreach (GraphPoint point in next.Szomszedok)
            {
                int i = list.IndexOf(point);
                if (i == 0)
                {
                    isGood = true;
                }
                if (i > 0 && point!=mostani)
                {
                    list.RemoveRange(i+1, list.Count - i-1);
                }
            }
            if (isGood)
            {
                list.Add(next);
                circles.Add(list);
                return;
            }
            list.Add(next);
                Next(list, mostani, next, jobbra);     
        }
    }
}