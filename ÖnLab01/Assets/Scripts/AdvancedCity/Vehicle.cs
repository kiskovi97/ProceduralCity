using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    class Vehicle : MonoBehaviour
    {
        public MovementPoint nextPoint;
        public float speed = 10.0f;
        private float actualspeed = 2.0f;
        public void setPoint(MovementPoint next)
        {
            nextPoint = next;
        }
        public void Start()
        {

        }
        public void Update()
        {
            if (nextPoint == null) return;
            float length = (nextPoint.center - transform.position).magnitude;
            if (length < 0.4f)
            {
                nextPoint = nextPoint.getNextPoint();
            }
            else {
                Move();
            }
            if (length > 5.0f)
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
            Vector3 newDir = Vector3.RotateTowards(transform.forward, toward, 0.05f, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            transform.position += newDir.normalized * actualspeed * 0.01f;
            
        }

    }
}
