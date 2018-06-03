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
        private float actualspeed = 0;
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
            Vector3 toward = (nextPoint.center - transform.position);
            float length = toward.magnitude;
            actualspeed += (length * speed  - actualspeed)/30;
            if (actualspeed > speed) actualspeed = speed;
            if (length < 0.2f)
            {
                nextPoint = nextPoint.getNextPoint();
                actualspeed = 0.0f;
            }
            else
            {
                
                Vector3 newDir = Vector3.RotateTowards(transform.forward, toward * (-1.0f), 0.1f, 0.0f);
                transform.rotation = Quaternion.LookRotation(toward * (-1.0f));
                transform.position += toward.normalized* speed * 0.01f;
            } 
        }

    }
}
