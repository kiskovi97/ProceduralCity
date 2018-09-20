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
			if (nextPoint == null) {
                allapot = "nextPoint = null";
                return;
            }
			float length = (nextPoint.center - transform.position).magnitude;
            if (length < 0.1f || elozoPoint == nextPoint)
			{
				elozoPoint = nextPoint;
				nextPoint = nextPoint.getNextPoint();
				distanceTime = 0;
			}
            if (canMove())
            {
                allapot = "canMove";
                Move();
            } 
			else
            {
				allapot = "cantMove";
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
                allapot = "MOVING";
                distanceTime += speed * 0.1f * Time.deltaTime;
                Vector3 cross;
				Vector3 nDir = nextPoint.direction;
				Vector3 nCenter = nextPoint.center;
				Vector3 eDir = elozoPoint.direction;
				Vector3 eCenter = elozoPoint.center;
                if (Vector3.Angle(eDir, nDir) < 30)
                {
                    cross = (eCenter + nCenter) / 2;
                }
                else
                {
                    cross = MyMath.Intersect(eCenter, eDir, nCenter, nDir);
                }
                float length = (eCenter - cross).magnitude + (cross - nCenter).magnitude;
                float thisTime = distanceTime / length;
                if (thisTime > 1) thisTime = 1;
                Vector3 pos = MovementPoint.BezierCurve(eCenter, cross, nCenter, thisTime);
                transform.position = pos;
                transform.rotation = Quaternion.LookRotation(MovementPoint.directionBezierCurve(eCenter, cross, nCenter, thisTime));
            } 
			else
            {
                transform.position = transform.position + (elozoPoint.center - transform.position) * 0.1f;
            }
        }
    }
}
