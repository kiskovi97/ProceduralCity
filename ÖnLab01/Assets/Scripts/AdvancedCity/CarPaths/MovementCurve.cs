using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    public class MovementCurve
    {
        MovementPoint elozo;
        MovementPoint kovetkezo;
        float speed;
        float time;
        public MovementCurve(MovementPoint elozo, MovementPoint kovetkezo, float speed)
        {
            this.elozo = elozo;
            this.kovetkezo = kovetkezo;
            this.speed = speed;
            time = 0;
        }
        public Vector3 addTime(float change)
        {
            time += change;
            return getPosition();
        }

        public Vector3 getPosition()
        {
            if (elozo != kovetkezo)
            {
                time += speed;
            }
            Vector3 dir = kovetkezo.center - elozo.center;
            if (dir.magnitude < time)
            {
                time -= dir.magnitude;
                elozo = kovetkezo;
                kovetkezo = kovetkezo.getNextPoint();
                if (elozo == kovetkezo)
                {
                    return kovetkezo.center;
                }
                dir = kovetkezo.center - elozo.center;
            }

            return elozo.center + dir.normalized * time;
        }
    }
}
