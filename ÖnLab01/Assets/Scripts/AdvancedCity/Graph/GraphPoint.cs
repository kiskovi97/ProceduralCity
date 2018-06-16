
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class GraphPoint
    {
        // ---------------- Basic Initialization -----------
        public Vector3 position;
        public enum POINTTYPE { MAIN, SIDE};
        protected POINTTYPE type;
        public bool isMainRoad()
        {
            return type == POINTTYPE.MAIN;
        }
        public bool isSideRoad()
        {
            return type == POINTTYPE.SIDE;
        }
        public void setAsSideRoad()
        {
            type = POINTTYPE.SIDE;
        }
        public void setType(POINTTYPE in_type)
        {
            type = in_type;
        }
        // elso szomszed az a pont ami letrehozta
        protected List<GraphPoint> szomszedok;
        public List<GraphPoint> Szomszedok
        {
            get
            {
                List<GraphPoint> uj = new List<GraphPoint>();
                uj.AddRange(szomszedok);
                return uj;
            }
        }
        public void addSzomszed(GraphPoint be)
        {
            if (!szomszedok.Contains(be))
                szomszedok.Add(be);
        }
        public void removeSzomszed(GraphPoint ki)
        {
            if (szomszedok.Contains(ki))
                szomszedok.Remove(ki);
        }
        public void setElozo(GraphPoint be)
        {
            if (szomszedok == null) szomszedok = new List<GraphPoint>();

            if (szomszedok.Count == 0)
                szomszedok.Add(be);
            else
                szomszedok[0] = be;
        }
        public GraphPoint getElozo()
        {
            if (szomszedok == null) return null;
            if (szomszedok.Count < 1) return null;
            return szomszedok[0];
        }
        public void csere(GraphPoint uj, GraphPoint regi)
        {
            if (szomszedok == null) return;
            if (szomszedok.Contains(regi))
            {
                int index = szomszedok.IndexOf(regi);
                if (szomszedok.Contains(uj))
                {
                    szomszedok.Remove(regi);
                }
                else szomszedok[index] = uj;
            }
            else  return;
            
        }
        // ora mutato jarasaba rendez
        public GraphPoint()
        {
            szomszedok = new List<GraphPoint>();
        }
        public GraphPoint kovetkezo(GraphPoint elozo, bool jobbra)
        {
            if (szomszedok == null) return null;
            if (szomszedok.Count < 1) return null;
            if (szomszedok.Count == 1) return szomszedok[0];
            if (!szomszedok.Contains(elozo)) return null;

            GraphPoint ki = szomszedok[0];
            Vector3 ki_irany = (ki.position - position).normalized;
            Vector3 elozo_irany = (elozo.position - position).normalized;
            float angleNow;
            if (jobbra)
                angleNow = 360;
            else
                angleNow = -360;
            foreach (GraphPoint road in szomszedok)
            {
                if (road == elozo) continue;
                Vector3 kovetkezo_irany = (road.position - position).normalized;
                float angleNew = Vector3.SignedAngle(elozo_irany, kovetkezo_irany, Vector3.up);
                if (jobbra)
                {
                    if (angleNew < 0) angleNew += 360;
                }
                else
                     if (angleNew > 0) angleNew -= 360;


                if (angleNow > angleNew && jobbra)
                {
                    ki = road;
                    elozo_irany = elozo.position - position;
                    angleNow = angleNew;
                }
                if (angleNow < angleNew && !jobbra)
                {
                    ki = road;
                    elozo_irany = elozo.position - position;
                    angleNow = angleNew;
                }
            }
            if (ki == elozo) return null;
            return ki;
        }

    }
}
