using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity.monoBeheviors.interactiveObjects
{
    class Car : Vehicle
    {
        public AudioSource audioSource;
        public float speed = 10.0f;
        public bool hasCamera = false;
        public int time = 0;
        public float carsize = 1.0f;
        public string allapot = "";

        public override void Step()
        {
            if (nextPoint == null) {
                allapot = "nextPoint = null";
                return;
            }
            float length = (nextPoint.center - transform.position).magnitude;
            if (length < 0.1f || elozoPoint == nextPoint)
			{
				elozoPoint = nextPoint;
				nextPoint = nextPoint.GetNextPoint();
			}
            if (canMove())
			{
				Move();
			}
			else
			{
				allapot = "NOCANMOVE";
				if (isLoop(new List<Car>() { this }))
				{
					if (!hasCamera)
						Destroy(gameObject, 0.1f);
				}
            }
        }

        Car waitedcar = null;

        protected bool isLoop(List<Car> cars)
        {
            if (waitedcar == null) return false;
            if (cars.Contains(waitedcar)) return true;
            cars.Add(this);
            return waitedcar.isLoop(cars);
        }

        protected bool canMove()
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

                    allapot = "SomethingOnTheWay "+hit.gameObject.name;
                    return false;
                }
            }
            waitedcar = null;
            return true;
        }
        public override void Move()
        {
            Vector3 toward = (nextPoint.center - (transform.position));
            Vector3 newDir = Vector3.RotateTowards(transform.forward, toward, speed * Time.deltaTime * 0.2f, 0.0f);
            float angle = Vector3.Angle(toward, transform.forward);
            if (angle < 30.0f)
            {
                transform.rotation = Quaternion.LookRotation(newDir);
                transform.position += newDir.normalized * speed * 0.1f * Time.deltaTime;
            }
            else 
			{
                transform.rotation = Quaternion.LookRotation(newDir);
                transform.position += toward.normalized * 20 * 0.1f * Time.deltaTime;
            }
            
        }
    }
}
