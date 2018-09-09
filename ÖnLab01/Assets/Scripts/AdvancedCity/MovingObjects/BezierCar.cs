using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity.monoBeheviors.interactiveObjects
{
    class BezierCar : Car
    {
        public override void Step()
        {
            if (canMove())
            {
                float length = (nextPoint.center - transform.position).magnitude;
                if (length < 0.1f || elozoPoint == nextPoint)
                {
                    elozoPoint = nextPoint;
                    nextPoint = nextPoint.getNextPoint();
                    distanceTime = 0;
                }
                Move();
            } else
            {
                if (isLoop(new List<Car>() { this }))
                {
                    if (!hasCamera)
                        Destroy(gameObject, 0.1f);
                }
            }
            
        }
        protected float distanceTime = 0;
        public override void Move()
        {
            transform.rotation = Quaternion.LookRotation(nextPoint.direction);
            if (elozoPoint != nextPoint)
            {
                distanceTime += speed * 0.1f * Time.deltaTime;
                float length = (elozoPoint.center - nextPoint.center).magnitude;
                float angle = Vector3.Angle(elozoPoint.direction, nextPoint.direction);
                if (angle > 60) length *= angle / 60;
                float thisTime = distanceTime / length;
                if (thisTime > 1) thisTime = 1;
                Vector3 cross;
                if (Vector3.Angle(elozoPoint.direction, nextPoint.direction) < 30)
                {
                    cross = (elozoPoint.center + nextPoint.center) / 2;
                }
                else
                {
                    cross = MyMath.Intersect(elozoPoint.center, elozoPoint.direction, nextPoint.center, nextPoint.direction);
                }
                Vector3 pos = MovementPoint.BezierCurve(elozoPoint.center, cross, nextPoint.center, thisTime);
                transform.position = pos;
                transform.rotation = Quaternion.LookRotation(MovementPoint.directionBezierCurve(elozoPoint.center, cross, nextPoint.center, thisTime));
            } else
            {
                transform.position = transform.position + (elozoPoint.center - transform.position)*0.1f;
            }
        }
    }
}
