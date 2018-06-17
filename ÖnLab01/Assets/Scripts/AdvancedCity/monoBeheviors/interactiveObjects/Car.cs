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
        void OnTriggerEnter(Collider other)
        {
            Vector3 toward = other.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, toward);
            if (angle > 30) return;
            stop = true;
        }
        void OnTriggerExit(Collider other)
        {
            stop = false;
            stopTime = 0;
        }
        public void Go(GameObject said)
        {
            stop = false;
            stopTime = 0;
        }

        int stopTime = 0;

        public override void Step()
        {
            if (stop)
                isStopNecessarry();
            float length = (nextPoint.center - transform.position).magnitude;
            if (length < 0.1f)
                nextPoint = nextPoint.getNextPoint();
            else
                if (!stop)
                    Move();
            SpeedAdjustments(length);
        }
        protected virtual void isStopNecessarry()
        {
            stopTime++;
            if (stopTime > 400)
            {
                stop = false;
                stopTime = 0;
            }
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
            Vector3 toward = (nextPoint.center - transform.position);
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
