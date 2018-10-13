using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    public class MovementCurve
    {
        MovementPoint before;
        MovementPoint next;
        readonly float speed;
        float time;
        public MovementCurve(MovementPoint before, MovementPoint next, float speed)
        {
            this.before = before;
            this.next = next;
            this.speed = speed;
            time = 0;
        }
        public Vector3 AddTime(float change)
        {
            time += change;
            return GetPosition();
        }

        private float timeToStop = 0;

        private readonly float maxTime = 200;

        public Vector3 GetFirstPosition()
        {
            if (before != next)
            {
                time += speed;
            }
            Vector3 dir = next.center - before.center;
            if (dir.magnitude < time)
            {
                if (next.megallo && timeToStop < maxTime)
                {
                    time -= speed;
                    timeToStop++;
                    return next.center;
                }
                timeToStop = 0;
                time -= dir.magnitude;
                before = next;
                next = next.GetNextPoint();
                if (before == next)
                {
                    return next.center;
                }
                dir = next.center - before.center;
            }

            return before.center + dir.normalized * time;
        }

        public Vector3 GetPosition()
        {
            if (before != next)
            {
                time += speed;
            }
            Vector3 dir = next.center - before.center;
            if (dir.magnitude < time)
            {
                time -= dir.magnitude;
                before = next;
                next = next.GetNextPoint();
                if (before == next)
                {
                    return next.center;
                }
                dir = next.center - before.center;
            }

            return before.center + dir.normalized * time;
        }

        public Vector3 GetPlusPosition(float timeplus)
        {
            float tmpTime = time;
            if (before != next)
            {
                tmpTime += timeplus; 
            }
            Vector3 dir = next.center - before.center;
            if (dir.magnitude < tmpTime)
            {
                tmpTime -= dir.magnitude;
                MovementPoint tmpElozo = next;
                MovementPoint tmpKovetkezo = next.GetNextPoint();
                if (tmpElozo == tmpKovetkezo)
                {
                    return tmpKovetkezo.center;
                }
                dir = tmpKovetkezo.center - tmpElozo.center;
            }
            return before.center + dir.normalized * tmpTime;
        }
    }
}
