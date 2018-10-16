﻿using System;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity.monoBeheviors.interactiveObjects
{
    public class Tram : Vehicle
    {
        public Vector3 A_point;
        public Vector3 B_point;
        private Tram next_tram = null;
        private MovementCurve A_nextPoint;
        private MovementCurve B_nextPoint;
        public float speed = 10.0f;

        public override void Step()
        {
            CalculateA();
            CalculateB();
            Move();
        }

        public override void SetPoint(MovementPoint next)
        {
            nextPoint = next;
            A_nextPoint = new MovementCurve(next,next.GetNextPoint(),speed*0.01f);
            B_nextPoint = new MovementCurve(next, next.GetNextPoint(), speed * 0.01f);
            A_nextPoint.AddTime(0.6f);
        }

        public void setDirection()
        {
            Vector3 toward = (nextPoint.center - transform.position);
            transform.rotation = Quaternion.LookRotation(toward);
        }

        public void generateMore(int db)
        {
            if (db <= 0) return;
            GameObject newObj = Instantiate(this.gameObject);
            Tram vehicle = newObj.GetComponent<Tram>();
            vehicle.SetPoint(nextPoint);
            vehicle.setDirection();
            vehicle.setTram(this);
            vehicle.generateMore(db - 1);
        }

        public void setTram(Tram kovetkezo)
        {
            next_tram = kovetkezo;
        }

        private void CalculateA()
        {
            if (next_tram == null)
            {
                A_point = A_nextPoint.GetFirstPosition();
                return;
            }
            if (((next_tram.B_point - A_point).magnitude > 0 && ((next_tram.A_point - A_point).magnitude > 0.5f)))
                 A_point = A_nextPoint.GetPosition();
           
        }

        private void CalculateB()
        {
            if ((B_point - A_point).magnitude > 0.5f)
                B_point = B_nextPoint.GetPosition();
        }

        public override void Move()
        {
            /*Debug.DrawLine(A_point, A_point + new Vector3(0, 1, 0), Color.red);
            Debug.DrawLine(B_point, B_point + new Vector3(0, 1, 0), Color.blue);*/
            Vector3 toward = (A_point - B_point).normalized;
            transform.rotation = Quaternion.LookRotation(toward);
            transform.position = (A_point + B_point)/2;
        }
    }
}
