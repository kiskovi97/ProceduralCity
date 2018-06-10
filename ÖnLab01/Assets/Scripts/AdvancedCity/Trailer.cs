using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    class Trailer : MonoBehaviour
    {

        public Transform toObject;
        void Update()
        {
            if (toObject != null)
            {
                Vector3 toVector = (toObject.position - transform.position);
                float speed = toVector.magnitude / 5.0f;
                if (speed < 0.1f) speed /= 5.0f;
                else
                {
                    speed = 0.1f;
                }
                
                transform.position += toVector.normalized * speed;

                Vector3 newDir = Vector3.RotateTowards(transform.forward, toVector, 0.1f, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }
}
