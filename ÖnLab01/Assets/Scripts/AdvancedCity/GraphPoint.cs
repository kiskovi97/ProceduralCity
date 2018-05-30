
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class GraphPoint
    {

        float randomTurn = 0.2f;

        // ---------------- Basic Initialization -----------
        public Vector3 position;
        public enum POINTTYPE { MAIN, SIDE};
        private POINTTYPE type;
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
        public List<GraphPoint> szomszedok;
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
        public void sorbaRendez()
        {
            for (int i = 0; i < szomszedok.Count - 1; i++)
            {
                Vector3 eddigiirany = szomszedok[i].position - position;
                float eddigiszog = 360;
                int z = i;
                for (int j = i + 1; j < szomszedok.Count; j++)
                {
                    Vector3 masirany = szomszedok[j].position - position;
                    float szog = Vector3.SignedAngle(eddigiirany, masirany, new Vector3(0, 1, 0));
                    if (szog < 0) szog += 360;
                    if (eddigiszog > szog)
                    {
                        z = j;
                        eddigiszog = szog;
                    }
                }
                if (z > i + 1)
                {
                    GraphPoint tmp = szomszedok[i + 1];
                    szomszedok[i + 1] = szomszedok[z];
                    szomszedok[z] = tmp;
                }
            }
        }
        public GraphPoint()
        {
            szomszedok = new List<GraphPoint>();
        }

        // ---------------- Generating -------------
        public List<GraphPoint> generatePoints(float distance, float straightFreq, float rotationRandom, int maxelagazas)
        {
            if (Random.value < straightFreq)
            {
                List<GraphPoint> kimenet = new List<GraphPoint>();
                kimenet.Add(generateStraight(distance, rotationRandom));
                return kimenet;
            }
            else
            {
                MakeIranyok(maxelagazas);
                return generateCrossing(distance, straightFreq, rotationRandom);
            }
        }
        GraphPoint generateStraight( float distance, float rotationRandom)
        {
            GraphPoint uj = new GraphPoint();
            Vector3 irany = new Vector3(0, 0, 1.5f);
            if (szomszedok.Count > 0) irany = position - szomszedok[0].position;

            
            float Rotation = Random.value * rotationRandom * 2 - rotationRandom;
            Vector3 random_irany = new Vector3(
            irany.x * Mathf.Cos(Rotation) - irany.z * Mathf.Sin(Rotation), irany.y * -1,
            irany.x * Mathf.Sin(Rotation) + irany.z * Mathf.Cos(Rotation)).normalized;

            uj.position = position + random_irany * distance;
            uj.setElozo(this);
            uj.setType(type);
            addSzomszed(uj);
            return uj;
        }
        List<GraphPoint> generateCrossing(float distance, float straightFreq, float rotationRandom)
        {
            List<GraphPoint> kimenet = new List<GraphPoint>();
            foreach (Vector3 irany in tovabb_irany)
            {
                GraphPoint ad = new GraphPoint();
                ad.position = position + irany * distance;
                ad.setElozo(this);
                ad.setType(type);
                kimenet.Add(ad);
                addSzomszed(ad);
            }
            return kimenet;
        }    
        public List<GraphPoint> generateSidePoints(float distance)
        {
            List<GraphPoint> ki = new List<GraphPoint>();
            MakeSideIrany();
            foreach (Vector3 irany in tovabb_irany)
            {
                GraphPoint ad = new GraphPoint();
                ad.position = position + irany * distance;
                ad.setElozo(this);
                ad.setAsSideRoad();
                ki.Add(ad);
                addSzomszed(ad);
                //szomszedok.Add(ad);
            }
            return ki;
        }

        private List<Vector3> tovabb_irany;
        void MakeIranyok(int maxelagazas)
        {
            tovabb_irany = new List<Vector3>();
            Vector3 elozo_irany = new Vector3(0, 0, 1);
            if (szomszedok.Count > 0) elozo_irany = szomszedok[0].position - position;

            int elagazasok = (int)(Random.value * (maxelagazas - 2) + 2);
            if (elagazasok < 2) elagazasok = 2;
            if (isSideRoad()) elagazasok = 3;
            for (int i = 1; i < elagazasok + 1; i++)
            {
                Vector3 uj = new Vector3();
                float Rotation = 3.14f * (2 * (-i / (elagazasok + 1.0f)));
                Rotation += -randomTurn + Random.value * randomTurn * 2;
                uj.Set(
                elozo_irany.x * Mathf.Cos(Rotation) - elozo_irany.z * Mathf.Sin(Rotation), elozo_irany.y * -1,
                elozo_irany.x * Mathf.Sin(Rotation) + elozo_irany.z * Mathf.Cos(Rotation));
                tovabb_irany.Add(uj.normalized);
            }
        }
        void MakeSideIrany()
        {
            tovabb_irany = new List<Vector3>();
            // Only straight roads can make sideroads
            if (szomszedok.Count != 2) return;
            Vector3 irany = szomszedok[0].position - szomszedok[1].position;
            Vector3 meroleges1 = new Vector3(irany.z, irany.y, irany.x * -1);
            Vector3 meroleges2 = new Vector3(irany.z * -1, irany.y, irany.x);

            tovabb_irany.Add(meroleges1.normalized);
            tovabb_irany.Add(meroleges2.normalized);
        }

        // ---------------- Kor kereses -------------
        public GraphPoint kovetkezo(GraphPoint elozo, bool jobbra)
        {
            if (szomszedok == null) return null;
            if (szomszedok.Count < 1) return null;
            if (szomszedok.Count == 1) return szomszedok[0];
            if (!szomszedok.Contains(elozo))  return null;
            
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

        // ---------------- Debug DrawLine ---------
        public void DrawLines(Color c)
        {
            foreach (GraphPoint road in szomszedok)
            {
                Debug.DrawLine(position, road.position, c, 100, false);
            }
        }

        // ---------------- Smooth Funtcion ----------
        public void Smooth(float intensity)
        {
            if (szomszedok.Count < 2) return;
            Vector3 center = new Vector3(0, 0, 0);
            foreach (GraphPoint szomszed in szomszedok) center += szomszed.position;
            center /= szomszedok.Count;
            Vector3 irany = center - position;
            position += irany * intensity;
        }

    }
}
