
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
        
        public ObjectGenerator(List<GraphPoint> points)
        {
            controlPoints = points;
        }

        public List<Crossing> GenerateRoadMesh()
        {
            List<Crossing> crossings = new List<Crossing>();
            foreach (GraphPoint road in controlPoints)
            {
                Crossing cros = new Crossing(road.position);
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
                    float utelozo = 0.6f + ((!szomszedok[i].isSideRoad() && ezbool) ? 0.5f : 0.0f);
                    float utekov = 0.6f + ((!szomszedok[kov].isSideRoad() && ezbool) ? 0.5f : 0.0f);

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
                    if (a > b)
                    {
                        ute.Add(true);
                        Vector3 masikkereszt = math.Intersect(lista[i][3], (szomszed - ez).normalized, lista[i][1], lista[i][0] - lista[i][1]);
                        kor.Add(lista[i][1]);
                        kor.Add(masikkereszt);
                        cros.AddSzomszed(lista[i][1], masikkereszt, szomszedok[i]);
                        //Debug.DrawLine(lista[i][3], masikkereszt, Color.blue, 1000, false);
                        //Debug.DrawLine(lista[i][1], masikkereszt, Color.green, 1000, false);
                    }
                    else
                    {
                        Vector3 masikkereszt = math.Intersect(lista[i][1], (szomszed - ez).normalized, lista[i][3], lista[i][2] - lista[i][3]);
                        ute.Add(false);
                        kor.Add(lista[i][1]);
                        kor.Add(masikkereszt);
                        cros.AddSzomszed(masikkereszt, lista[i][3], szomszedok[i]);
                        //Debug.DrawLine(lista[i][1], masikkereszt, Color.blue, 1000, false);
                        //Debug.DrawLine(lista[i][3], masikkereszt, Color.green, 1000, false);
                    }
                }
                //for (int i = 0; i < kor.Count; i++)
                //{
                //    int kov = i + 1;
                //    if (kov >= kor.Count) kov = 0;
                //    if (i % 2 == 0 && ute[i / 2])
                //        Debug.DrawLine(kor[i], kor[kov], Color.blue, 1000, false);
                //    else
                //    if (i % 2 == 1 && !ute[i / 2])
                //        Debug.DrawLine(kor[i], kor[kov], Color.blue, 1000, false);
                //    else
                //        Debug.DrawLine(kor[i], kor[kov], Color.green, 1000, false);
                //    /*
                //        ute jelzi, hogy a kor listaban az elso vagy masdik ut amelyik az ut resze 
                //        a masik fele a kiegeszito resz
                //    */
                //}
                crossings.Add(cros);
            }
            return crossings;
        }
    }
}
