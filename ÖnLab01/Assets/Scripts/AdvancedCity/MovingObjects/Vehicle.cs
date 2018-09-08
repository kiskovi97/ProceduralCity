using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    public class Vehicle : MonoBehaviour
    {

        public MovementPoint nextPoint;
        public MovementPoint elozoPoint;
        protected float actualspeed = 10.0f;
        public virtual void setPoint(MovementPoint next)
        {
            nextPoint = next.getNextPoint();
            if (nextPoint == null)
                nextPoint = next;
            if (nextPoint != next)
                elozoPoint = next;
            transform.rotation = Quaternion.LookRotation(nextPoint.direction);
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
            {
                MovementPoint tmp = nextPoint;
                nextPoint = nextPoint.getNextPoint();
                if (tmp != nextPoint && nextPoint != null)
                {
                    elozoPoint = tmp;
                }
            }
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
