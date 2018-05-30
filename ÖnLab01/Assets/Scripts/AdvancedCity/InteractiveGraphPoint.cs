
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class InteractiveGraphPoint : GraphPoint
    {
        float randomTurn = 0.2f;
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

        // ---------------- Generating -------------
        public List<InteractiveGraphPoint> generatePoints(float distance, float straightFreq, float rotationRandom, int maxelagazas)
        {
            if (Random.value < straightFreq)
            {
                List<InteractiveGraphPoint> kimenet = new List<InteractiveGraphPoint>();
                kimenet.Add(generateStraight(distance, rotationRandom));
                return kimenet;
            }
            else
            {
                MakeIranyok(maxelagazas);
                return generateCrossing(distance, straightFreq, rotationRandom);
            }
        }
        InteractiveGraphPoint generateStraight(float distance, float rotationRandom)
        {
            InteractiveGraphPoint uj = new InteractiveGraphPoint();
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
        List<InteractiveGraphPoint> generateCrossing(float distance, float straightFreq, float rotationRandom)
        {
            List<InteractiveGraphPoint> kimenet = new List<InteractiveGraphPoint>();
            foreach (Vector3 irany in tovabb_irany)
            {
                InteractiveGraphPoint ad = new InteractiveGraphPoint();
                ad.position = position + irany * distance;
                ad.setElozo(this);
                ad.setType(type);
                kimenet.Add(ad);
                addSzomszed(ad);
            }
            return kimenet;
        }
        public List<InteractiveGraphPoint> generateSidePoints(float distance)
        {
            List<InteractiveGraphPoint> ki = new List<InteractiveGraphPoint>();
            MakeSideIrany();
            foreach (Vector3 irany in tovabb_irany)
            {
                InteractiveGraphPoint ad = new InteractiveGraphPoint();
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
