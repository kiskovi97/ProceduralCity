using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity.monoBeheviors.interactiveObjects
{
    class PlayerCar : Car
    {
        public override void Step()
        {
            Move();
        }
        public override void Move()
        {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 10 * speed;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * 0.1f * speed;
            float turbo = Input.GetButton("Turbo") ? 2.0f : 1.0f;
            transform.Rotate(0, x, 0);
            Vector3 move = new Vector3(0, 0, z * turbo);
            move = Quaternion.LookRotation(transform.forward, transform.up) * move;
            transform.position += move;
        }
    }
}
