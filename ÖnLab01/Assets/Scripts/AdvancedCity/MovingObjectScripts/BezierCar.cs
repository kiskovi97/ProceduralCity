using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.AdvancedCity.monoBeheviors.interactiveObjects
{
    class BezierCar : Car
    {
        public override void Move()
        {
            if (anim != null)
            {
                anim.SetFloat("Blend", 1);
            }
            SpeedCalculate();
            actualSpeed = point.BezierStep(actualSpeed * Time.deltaTime) / Time.deltaTime;
            transform.rotation = point.GetBezierDir();
            SetPosition(point.pos);
        }
    }
}
