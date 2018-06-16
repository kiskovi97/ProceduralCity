using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    class Vehicle : MonoBehaviour
    {
        public Vehicle()
        {
            enterd = new List<GameObject>();
        }
        public MovementPoint nextPoint;
        public float speed = 10.0f;
        private float actualspeed = 2.0f;
        bool stop = false;
        List<GameObject> enterd;
        public void setPoint(MovementPoint next)
        {
            nextPoint = next;
        }
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
        }
        public void Go(GameObject said)
        {
            stop = false;
        }

        int stopTime = 0;
        public void Update()
        {
            if (stop)
            {
                stopTime++;
                if (stopTime > 400)
                {
                    stop = false;
                    stopTime = 0;
                }
            } 
            if (nextPoint == null) return;
            float length = (nextPoint.center - transform.position).magnitude;
            if (length < 0.3f)
            {
                nextPoint = nextPoint.getNextPoint();
            }
            else {
                if (!stop)
                    Move();
            }
            if (length > 3.0f)
            {
                actualspeed += 0.03f;
            } else
            {
                actualspeed -= 0.03f;
            }
            if (actualspeed > speed) actualspeed = speed;
            if (actualspeed < 2.0f) actualspeed = 2.0f;
        }
        public void Move()
        {
            Vector3 toward = (nextPoint.center - transform.position);
            Vector3 newDir = Vector3.RotateTowards(transform.forward, toward, 0.1f, 0.0f);
            float angle = Vector3.Angle(toward, transform.forward);
            float slower;
            if (angle < 5.0f) slower = 1.0f;
            else slower = 1.0f / (angle*10.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            transform.position += newDir.normalized * actualspeed * 0.01f * slower;
        }

    }
}
