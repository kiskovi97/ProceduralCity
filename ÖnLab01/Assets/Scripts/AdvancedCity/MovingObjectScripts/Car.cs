using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.AdvancedCity.monoBeheviors.interactiveObjects
{
    class Car : Vehicle
    {
        public float speed = 5.0f;
        public bool hasCamera = false;
        public int time = 0;
        public float carsize = 1.0f;

        public override void Step()
        {
            if (CanMove())
            {
                Move();
			}
			else
			{
                if (waitedcar != null) actualSpeed = waitedcar.actualSpeed;
                else actualSpeed = 0;
                if (actualSpeed > speed) actualSpeed = speed;
                if (IsLoop(new List<Car>() { this }))
				{
					if (!hasCamera)
						Destroy(gameObject, 0.1f);
				}
            }
        }

        public override void Move()
        {
            if (anim != null)
            {
                anim.SetFloat("Blend", 1);
            }
            SpeedCalculate();
            actualSpeed = point.Step(actualSpeed * Time.deltaTime) / Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(point.Forward);
            SetPosition(point.pos);
        }

        protected void SpeedCalculate()
        {
            if (waitedcar != null) actualSpeed = waitedcar.actualSpeed;
            else
            {
                if ((speed - actualSpeed) > 0.05f) actualSpeed += 0.05f;
                else actualSpeed = speed;
            }
            if (actualSpeed > speed) actualSpeed = speed;
        }

        Car waitedcar = null;

        protected bool IsLoop(List<Car> cars)
        {
            if (waitedcar == null) return false;
            if (cars.Contains(waitedcar)) return true;
            cars.Add(this);
            return waitedcar.IsLoop(cars);
        }

        protected bool CanMove()
        {
            
            Vector3 newDir = transform.forward;
            Collider[] hits = Physics.OverlapBox(transform.position + newDir * 0.2f * carsize, new Vector3(0.01f, 0.01f, 0.05f), Quaternion.LookRotation(newDir, new Vector3(0, 1, 0)));

            foreach (Collider hit in hits)
            {
                if (hit.gameObject != gameObject)
                {
                    Car[] cars = hit.gameObject.GetComponents<Car>();
                    if (cars != null && cars.Length > 0)
                    {
                        waitedcar = cars[0];
                    }
                    else
                    {
                        waitedcar = null;
                    }
                    return false;
                }
            }
            waitedcar = null;
            return true;
        }
    }
}
