using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class InteractiveGraphPoint : GraphPoint
    {
        readonly float randomTurn = 0.2f;
        public void Order()
        {
            for (int i = 0; i < neighbours.Count - 1; i++)
            {
                Vector3 beforeDirection = neighbours[i].position - position;
                float beforeAngle = 360;
                int z = i;
                for (int j = i + 1; j < neighbours.Count; j++)
                {
                    Vector3 otherDirection = neighbours[j].position - position;
                    float angle = Vector3.SignedAngle(beforeDirection, otherDirection, new Vector3(0, 1, 0));
                    if (angle < 0) angle += 360;
                    if (beforeAngle > angle)
                    {
                        z = j;
                        beforeAngle = angle;
                    }
                }
                if (z > i + 1)
                {
                    GraphPoint tmp = neighbours[i + 1];
                    neighbours[i + 1] = neighbours[z];
                    neighbours[z] = tmp;
                }
            }
        }

        // ---------------- Generating -------------
        public List<InteractiveGraphPoint> GeneratePoints(float distance, float straightFreq, float rotationRandom, int maxNeighbours)
        {
            if (Random.value < straightFreq)
            {
                List<InteractiveGraphPoint> kimenet = new List<InteractiveGraphPoint>
                {
                    GenerateStraight(distance, rotationRandom)
                };
                return kimenet;
            }
            else
            {
                MakeDirection(maxNeighbours);
                return GenerateCrossing(distance, straightFreq, rotationRandom);
            }
        }
        InteractiveGraphPoint GenerateStraight(float distance, float rotationRandom)
        {
            InteractiveGraphPoint newPoint = new InteractiveGraphPoint();
            Vector3 direction = new Vector3(0, 0, 1.5f);
            if (neighbours.Count > 0) direction = position - neighbours[0].position;
            float rotation = Random.value * rotationRandom * 2 - rotationRandom;
            Vector3 randomDirection = new Vector3(
            direction.x * Mathf.Cos(rotation) - direction.z * Mathf.Sin(rotation), direction.y * -1,
            direction.x * Mathf.Sin(rotation) + direction.z * Mathf.Cos(rotation)).normalized;
            newPoint.position = position + randomDirection * distance;
            newPoint.SetBefore(this);
            newPoint.SetType(type);
            AddNeighbour(newPoint);
            return newPoint;
        }
        List<InteractiveGraphPoint> GenerateCrossing(float distance, float straightFreq, float rotationRandom)
        {
            List<InteractiveGraphPoint> output = new List<InteractiveGraphPoint>();
            foreach (Vector3 direction in nextDirections)
            {
                InteractiveGraphPoint ad = new InteractiveGraphPoint
                {
                    position = position + direction * distance
                };
                ad.SetBefore(this);
                ad.SetType(type);
                output.Add(ad);
                AddNeighbour(ad);
            }
            return output;
        }
        public List<InteractiveGraphPoint> GenerateSidePoints(float distance)
        {
            List<InteractiveGraphPoint> output = new List<InteractiveGraphPoint>();
            MakeSideDirection();
            foreach (Vector3 direction in nextDirections)
            {
                InteractiveGraphPoint ad = new InteractiveGraphPoint
                {
                    position = position + direction * distance
                };
                ad.SetBefore(this);
                ad.SetAsSideRoad();
                output.Add(ad);
                AddNeighbour(ad);
            }
            return output;
        }

        private List<Vector3> nextDirections;
        void MakeDirection(int maxRoads)
        {
            nextDirections = new List<Vector3>();
            Vector3 beforeDirection = new Vector3(0, 0, 1);
            if (neighbours.Count > 0) beforeDirection = neighbours[0].position - position;

            int roadCount = (int)(Random.value * (maxRoads - 2) + 2);
            if (roadCount < 2) roadCount = 2;
            if (IsSideRoad()) roadCount = 3;
            for (int i = 1; i < roadCount + 1; i++)
            {
                Vector3 newPoint = new Vector3();
                float rotation = 3.14f * (2 * (-i / (roadCount + 1.0f)));
                rotation += -randomTurn + Random.value * randomTurn * 2;
                newPoint.Set(
                beforeDirection.x * Mathf.Cos(rotation) - beforeDirection.z * Mathf.Sin(rotation), beforeDirection.y * -1,
                beforeDirection.x * Mathf.Sin(rotation) + beforeDirection.z * Mathf.Cos(rotation));
                nextDirections.Add(newPoint.normalized);
            }
        }
        void MakeSideDirection()
        {
            nextDirections = new List<Vector3>();
            if (neighbours.Count != 2) return;
            Vector3 direction = neighbours[0].position - neighbours[1].position;
            Vector3 left = new Vector3(direction.z, direction.y, direction.x * -1);
            Vector3 right = new Vector3(direction.z * -1, direction.y, direction.x);
            nextDirections.Add(left.normalized);
            nextDirections.Add(right.normalized);
        }
        

        // ---------------- Smooth Funtcion ----------
        public void Smooth(float intensity)
        {
            if (neighbours.Count < 2) return;
            Vector3 center = new Vector3(0, 0, 0);
            foreach (GraphPoint neighbour in neighbours) center += neighbour.position;
            center /= neighbours.Count;
            Vector3 direction = center - position;
            position += direction * intensity;
        }

        public bool IsDeadEnd()
        {
            return neighbours == null || neighbours.Count < 2;
        }

        public void RemoveFromNeighbours()
        {
            foreach (GraphPoint szomszed in neighbours)
            {
                szomszed.RemoveNeighbour(this);
            }
        }
    }
}
