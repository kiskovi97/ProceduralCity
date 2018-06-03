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
            {
                nextPoint = nextPoint.getNextPoint();
            }
            else
            {
                transform.position += toward.normalized * speed*0.01f;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, toward * (-1.0f), 0.3f, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
            } 
        }

    }
}
