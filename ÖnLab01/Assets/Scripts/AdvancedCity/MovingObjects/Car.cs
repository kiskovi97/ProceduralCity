using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity.monoBeheviors.interactiveObjects
{
    class Car : Vehicle
    {
        public Car()
        {
            
        }
        public float speed = 10.0f;
        bool stop = false;

        public override void Step()
        {
            if (isLoop(new List<Car>() { this }))
            {
                Destroy(gameObject, 0.1f);
            }
            if (stop)
                isStopNecessarry();
            float length = (nextPoint.center - transform.position).magnitude;
            if (length < 0.1f)
                nextPoint = nextPoint.getNextPoint();
            else
                if (!stop && canMove())
                    Move();
            SpeedAdjustments(length);
        }

        Car waitedcar = null;

        private bool isLoop(List<Car> cars)
        {
            if (waitedcar == null) return false;
            if (cars.Contains(waitedcar)) return true;
            cars.Add(this);
            return waitedcar.isLoop(cars);
        }

        private bool canMove()
        {

            Vector3 toward = (nextPoint.center - transform.position).normalized;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, toward, 0.1f, 0.0f).normalized;
            Collider[] hits = Physics.OverlapBox(transform.position + newDir * 0.2f, new Vector3(0.01f, 0.01f, 0.05f), Quaternion.LookRotation(newDir, new Vector3(0, 1, 0)));
           
            foreach (Collider hit in hits)
            {
                if (hit.gameObject != gameObject)
                {
                    Car[] cars = hit.gameObject.GetComponents<Car>();
                    if (cars!=null && cars.Length>0)
                    {
                        waitedcar = cars[0];

                    } else
                    {
                        waitedcar = null;
                    }
                    return false;
                }
            }
            waitedcar = null;
            return true;
        }

        protected virtual void isStopNecessarry()
        {
            
        }
        protected virtual void SpeedAdjustments(float length)
        {
            if (length > 3.0f)
            {
                actualspeed += 0.03f;
            }
            else
            {
                actualspeed -= 0.03f;
            }
            if (actualspeed > speed) actualspeed = speed;
            if (actualspeed < 2.0f) actualspeed = 2.0f;
        }
        public override void Move()
        {
            Vector3 toward = (nextPoint.center - (transform.position));
            Vector3 newDir = Vector3.RotateTowards(transform.forward, toward, 0.1f, 0.0f);
            float angle = Vector3.Angle(toward, transform.forward);
            float slower;
            if (angle < 5.0f) slower = 1.0f;
            else slower = 1.0f / (angle * 10.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            transform.position += newDir.normalized * actualspeed * 0.01f * slower;
        }
    }
}
