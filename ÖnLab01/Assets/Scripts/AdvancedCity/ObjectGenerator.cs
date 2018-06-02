
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    
    class ObjectGenerator
    {
        MyMath math = new MyMath();
        List<GraphPoint> controlPoints;
        List<Crossing> crossings;
        List<Road> roads;
        public ObjectGenerator(List<GraphPoint> points)
        {
            controlPoints = points;
        }

        public void GenerateObjects()
        {
            crossings = new List<Crossing>();
            roads = new List<Road>();
            for(int i=0; i<controlPoints.Count; i++)
            {
                GraphPoint point = controlPoints[i];
                Crossing cros = new Crossing(point);
                crossings.Add(cros);
            }
            for(int i=0; i<controlPoints.Count; i++)
            {
                GraphPoint point = controlPoints[i];
                List<GraphPoint> szomszedok = point.Szomszedok;
                for (int j=0; j < szomszedok.Count; j++)
                {
                    int x = controlPoints.IndexOf(szomszedok[j]);
                    if (x > i)
                    {
                        Road r = new Road();
                        r.setSzomszedok(crossings[i], crossings[x]);
                        crossings[i].AddSzomszed(r);
                        crossings[x].AddSzomszed(r);
                        roads.Add(r);
                    }
                }
            }
        }

        public void GenerateRoadMesh()
        {
            for (int x = 0; x < controlPoints.Count; x++)
            {
                GraphPoint road = controlPoints[x];
                Crossing cros = crossings[x];
                List<GraphPoint> szomszedok = road.Szomszedok;

                List<List<Vector3>> lista = new List<List<Vector3>>();
                for (int i = 0; i < szomszedok.Count; i++)
                {
                    lista.Add(new List<Vector3>());
                    for (int j = 0; j < 4; j++) lista[i].Add(new Vector3(0, 0, 0));
                }

                List<Vector3> kor = new List<Vector3>();
                List<bool> ute = new List<bool>();

                Vector3 ez = road.position;
                bool ezbool = !road.isSideRoad();
                for (int i = 0; i < szomszedok.Count; i++)
                {
                    int kov = i + 1;
                    if (i == szomszedok.Count - 1) kov = 0;
                    Vector3 elozo = szomszedok[i].position;
                    Vector3 kovetkezo = szomszedok[kov].position;
                    float utelozo = 0.6f + ((!szomszedok[i].isSideRoad() && ezbool) ? 0.8f : 0.0f);
                    float utekov = 0.6f + ((!szomszedok[kov].isSideRoad() && ezbool) ? 0.8f : 0.0f);

                    Vector3 merolegeselozo = math.Meroleges(ez, elozo).normalized * utelozo;
                    Vector3 merolegeskovetkezo = math.Meroleges(kovetkezo, ez).normalized * utekov;
                    Vector3 tmpelozo = ez + (elozo - ez).normalized;
                    Vector3 tmpkovetkezo = ez + (kovetkezo - ez).normalized;

                    Vector3 P = tmpelozo + merolegeselozo;
                    Vector3 V = (elozo - ez).normalized;
                    Vector3 Q = tmpkovetkezo + merolegeskovetkezo;
                    Vector3 U = (kovetkezo - ez).normalized;
                    Vector3 kereszt = math.Intersect(P, V, Q, U);
                    
                    Vector3 meroleges_elozo = math.Intersect(kereszt, merolegeselozo, ez, (elozo - ez).normalized);
                    lista[i][2] = meroleges_elozo;
                    lista[i][3] = kereszt;

                    Vector3 meroleges_kov = math.Intersect(kereszt, merolegeskovetkezo, ez, (kovetkezo - ez).normalized);
                    lista[kov][0] = meroleges_kov;
                    lista[kov][1] = kereszt;
                }
                for (int i = 0; i < lista.Count; i++)
                {
                    
                    Vector3 szomszed = szomszedok[i].position;
                    float a = Vector3.Dot((szomszed - ez).normalized, lista[i][0] - ez);
                    float b = Vector3.Dot((szomszed - ez).normalized, lista[i][2] - ez);
                    Road r = cros.getSzomszedRoad(szomszedok[i]);
                    if (r!=null)
                    if (a > b)
                    {
                        ute.Add(true);
                        Vector3 masikkereszt = math.Intersect(lista[i][3], (szomszed - ez).normalized, lista[i][1], lista[i][0] - lista[i][1]);
                        kor.Add(lista[i][1]);
                        kor.Add(masikkereszt);
                        r.addLine(cros, lista[i][1], masikkereszt);
                        Vector3[] line = { lista[i][1], masikkereszt };
                        Vector3[] helpline = { lista[i][3], masikkereszt };
                        cros.AddLines(line, helpline, r);
                        //Debug.DrawLine(lista[i][3], masikkereszt, Color.blue, 1000, false);
                        //Debug.DrawLine(lista[i][1], masikkereszt, Color.green, 1000, false);
                    }
                    else
                    {
                        Vector3 masikkereszt = math.Intersect(lista[i][1], (szomszed - ez).normalized, lista[i][3], lista[i][2] - lista[i][3]);
                        ute.Add(false);
                        kor.Add(lista[i][1]);
                        kor.Add(masikkereszt);
                        Vector3[] line = { lista[i][3], masikkereszt };
                        Vector3[] helpline = { lista[i][1], masikkereszt };
                        cros.AddLines(line, helpline, r);
                        r.addLine(cros, masikkereszt, lista[i][3]);
                        //Debug.DrawLine(lista[i][1], masikkereszt, Color.blue, 1000, false);
                        //Debug.DrawLine(lista[i][3], masikkereszt, Color.green, 1000, false);
                    }
                }
                
            }
        }
        public void DrawRoads()
        {
            foreach(Road road in roads)
            {
                road.Draw();
            }
            foreach (Crossing cros in crossings)
            {
                cros.Draw();
            }
        }
    }
}
