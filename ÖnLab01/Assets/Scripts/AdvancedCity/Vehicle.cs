using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    class Vehicle : MonoBehaviour
    {
        public MovementPoint nextPoint;
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
            if (length < 0.2f)
                nextPoint = nextPoint.getNextPoint();
            else
                transform.position += toward.normalized * 0.1f;
        }

    }
}
