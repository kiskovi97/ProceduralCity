using UnityEngine;

namespace Assets.Scripts.AdvancedCity.monoBeheviors.interactiveObjects
{
    class PlayerCar : Car
    {
        public virtual void LateUpdate()
        {
            //
        }
        public override void Step()
        {
            Move();
        }
        public override void Move()
        {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 50 * speed;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * 0.5f * speed;
            var y = Input.GetAxis("Jump") * Time.deltaTime * 0.5f * speed;
            if (y < 0) y *= 0.5f;
            float turbo = Input.GetButton("Turbo") ? 2.0f : 1.0f;
            transform.Rotate(0, x, 0, Space.World);
            Vector3 move = new Vector3(0, y * turbo, z * turbo);
            move = Quaternion.LookRotation(transform.forward, new Vector3(0, 1, 0)) * move;
            Vector3 pos = transform.position + move;
            if (pos.y < 0)
            {
                pos -= new Vector3(0, pos.y, 0);
                Vector3 RealForward = transform.forward;
                RealForward.y = 0;
                transform.rotation = Quaternion.LookRotation(RealForward);
            }
            SetPosition(pos);
        }
    }
}
