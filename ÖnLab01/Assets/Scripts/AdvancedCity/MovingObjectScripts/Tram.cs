using System;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity.monoBeheviors.interactiveObjects
{
    public class Tram : Vehicle
    {
        public Vector3 A_point;
        public Vector3 B_point;
        public MovementPosition A = new MovementPosition();
        public MovementPosition B = new MovementPosition();
        public Tram nextTram = null;
        public float maxLength = 0.5f;
        public float speed = 10.0f;

        public override void Step()
        {
            CalculateA();
            CalculateB();
            Move();
        }

        public override void SetPoint(MovementPoint next)
        {
            base.SetPoint(next);
            A.nextPoint = point.nextPoint;
            A.prevPoint = point.prevPoint;
            A.pos = point.prevPoint.center;
            A_point = A.pos;
            B.nextPoint = point.nextPoint;
            B.prevPoint = point.prevPoint;
            B.pos = point.prevPoint.center;
            B_point = A.pos;
        }

        public override void Start()
        {
            base.Start();
            A.nextPoint = point.nextPoint;
            A.prevPoint = point.prevPoint;
            A.pos = point.prevPoint.center;
            A_point = A.pos;
            B.nextPoint = point.nextPoint;
            B.prevPoint = point.prevPoint;
            B.pos = point.prevPoint.center;
            B_point = A.pos;
            transform.position = (A_point + B_point) / 2;
        }

        public void SetDirection()
        {
            transform.rotation = Quaternion.LookRotation(point.Forward);
        }

        public void GenerateMore(int db)
        {
            if (db <= 0) return;
            GameObject newObj = Instantiate(this.gameObject);
            Tram vehicle = newObj.GetComponent<Tram>();
            vehicle.SetPoint(point.prevPoint);
            vehicle.SetDirection();
            vehicle.SetTram(this);
            vehicle.GenerateMore(db - 1);
        }

        public void SetTram(Tram kovetkezo)
        {
            nextTram = kovetkezo;
        }

        private int stoppingTick = 0;
        private void CalculateA()
        {
            if (nextTram == null)
            {
                if (A.Stopping(Time.deltaTime * speed))
                {
                    if (stoppingTick > 100)
                    {
                        stoppingTick = 0;
                        A.Step(Time.deltaTime * speed);
                    }
                    else
                    {
                        stoppingTick++;
                    }
                } else
                {
                    A.Step(Time.deltaTime * speed);
                }
            } else
            {
                float length = (nextTram.B_point - A_point).magnitude;
                if (length > 0)
                {
                    float actSpeed = speed;
                    if (length > maxLength * 2) actSpeed *= 2;
                    A.Step(Time.deltaTime * actSpeed);
                }
            }
            A_point = A.pos;
        }

        private void CalculateB()
        {
            float length = (B_point - A_point).magnitude;
            if (length > maxLength)
            {
                float actSpeed = speed;
                if (length > maxLength * 2) actSpeed *= 2;
                B.Step(Time.deltaTime * actSpeed);
            }
            B_point = B.pos;
        }

        public override void Move()
        {
            Debug.DrawLine(A_point, A_point + new Vector3(0, 1, 0), Color.red);
            Debug.DrawLine(B_point, B_point + new Vector3(0, 1, 0), Color.blue);
            Vector3 toward = (A_point - B_point).normalized;
            transform.rotation = Quaternion.LookRotation(toward);
            Vector3 pos = (A_point + B_point) / 2;
            transform.position = pos;
            //SetPosition(pos);
        }
    }
}
