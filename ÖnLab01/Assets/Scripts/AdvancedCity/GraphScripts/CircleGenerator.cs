
using System.Collections.Generic;
namespace Assets.Scripts.AdvancedCity
{
    public class CircleGenerator
    {
        List<List<GraphPoint>> circles = new List<List<GraphPoint>>();
        public List<GraphPoint> MaxCircle(List<GraphPoint> list)
        {
            for (int i = 0; i < list.Count; i++)
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
            foreach (GraphPoint neighbour in begining.Neighbours)
            {
                Next(new List<GraphPoint>() { begining, neighbour }, begining, neighbour, true);
            }
        }

        void Next(List<GraphPoint> list, GraphPoint before, GraphPoint actual, bool right)
        {
            GraphPoint next = actual.Next(before, right);
            if (next == list[0])
            {
                circles.Add(list);
                return;
            }
            if (list.Contains(next)) return;
            bool isGood = false;
            foreach (GraphPoint point in next.Neighbours)
            {
                int i = list.IndexOf(point);
                if (i == 0)
                {
                    isGood = true;
                }
                if (i > 0 && point != actual)
                {
                    list.RemoveRange(i + 1, list.Count - i - 1);
                }
            }
            if (isGood)
            {
                list.Add(next);
                circles.Add(list);
                return;
            }
            list.Add(next);
            Next(list, actual, next, right);
        }
    }
}