using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    class Vehicle : MonoBehaviour
    {

        public MovementPoint nextPoint;
        private float actualspeed = 10.0f;
        public void setPoint(MovementPoint next)
        {
            nextPoint = next;
        }

        public virtual void Update()
        {
            if (nextPoint == null) return;
            Step();
        }
        public virtual void Step()
        {
            float length = (nextPoint.center - transform.position).magnitude;
            if (length < 0.1f)
                nextPoint = nextPoint.getNextPoint();
            else
                Move();
        }
        public virtual void Move()
        {
            Vector3 toward = (nextPoint.center - transform.position);
            transform.rotation = Quaternion.LookRotation(toward);
            transform.position += toward.normalized * actualspeed * 0.01f;
        }

    }
}
