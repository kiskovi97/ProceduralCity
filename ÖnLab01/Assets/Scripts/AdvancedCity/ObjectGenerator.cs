
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
                Crossing cros = new Crossing(point.position,point.isMainRoad());
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
        public void GenerateRoadandCros()
        {
            for (int x = 0; x < controlPoints.Count; x++)
            {
                GraphPoint road = controlPoints[x];
                Crossing cros = crossings[x];
                List<GraphPoint> szomszedok = road.Szomszedok;
                List<Vector3> sidewalks = new List<Vector3>();
                List<List<Vector3>> lista = new List<List<Vector3>>();
                for (int i = 0; i < szomszedok.Count; i++)
                {
                    lista.Add(new List<Vector3>());
                    for (int j = 0; j < 4; j++) lista[i].Add(new Vector3(0, 0, 0));
                }

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
                    float sidewalk = 0.4f;
                    Vector3 merolegeselozo = math.Meroleges(ez, elozo).normalized * utelozo;
                    Vector3 merolegeskovetkezo = math.Meroleges(kovetkezo, ez).normalized * utekov;
                    Vector3 merolegeselozo_side = math.Meroleges(ez, elozo).normalized * (utelozo + sidewalk);
                    Vector3 merolegeskovetkezo_side = math.Meroleges(kovetkezo, ez).normalized * (utekov + sidewalk);

                    Vector3 kereszt = math.Intersect(
                        ez + merolegeselozo
                        , (elozo - ez).normalized
                        , ez + merolegeskovetkezo
                        , (kovetkezo - ez).normalized);
                    Vector3 kereszt_side = math.Intersect(
                        ez + merolegeselozo_side
                        , (elozo - ez).normalized
                        , ez + merolegeskovetkezo_side
                        , (kovetkezo - ez).normalized);
                    sidewalks.Add(kereszt_side);

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
                    Road r = cros.getSzomszedRoad(szomszedok[i].position);

                    int elozo = i - 1;
                    if (elozo < 0) elozo = lista.Count - 1;

                    if (r!=null)
                    if (a > b)
                    {
                        Vector3 masikkereszt = math.Intersect(lista[i][3], (szomszed - ez).normalized, lista[i][1], lista[i][0] - lista[i][1]);
                        r.addLine(cros, masikkereszt, lista[i][1]);
                        Vector3[] line = {  masikkereszt , lista[i][1]};
                        Vector3[] helpline = { lista[i][3], masikkereszt };
                        cros.AddLines(line, helpline, lista[i][1], sidewalks[elozo], r);
                    }
                    else
                    {
                        Vector3 masikkereszt = math.Intersect(lista[i][1], (szomszed - ez).normalized, lista[i][3], lista[i][2] - lista[i][3]);
                        Vector3[] line = { lista[i][3], masikkereszt };
                        Vector3[] helpline = { lista[i][1], masikkereszt };
                        cros.AddLines(line, helpline, lista[i][1], sidewalks[elozo], r);
                        r.addLine(cros, lista[i][3], masikkereszt);
                    }
                }
            }
            foreach (Crossing cros in crossings)
            {
                cros.carsPathSetting();
            }
        }

        public void DrawRoads(bool draw_helplines, bool depthtest)
        {
            foreach(Road road in roads)
            {
                road.Draw(depthtest);
            }
            foreach (Crossing cros in crossings)
            {
                cros.Draw(draw_helplines,depthtest);
            }
        }
        public void MakeBlocks(BlockGenerator generator)
        {
            generator.GenerateBlocks(crossings);
        }
        public void CreateCars(GameObject[] cars)
        {

            int i = 0;
            if (crossings == null) return;
            while( i < cars.Length)
            {
                foreach (Crossing cros in crossings)
                {
                    if (i < cars.Length)
                    {
                        Vehicle vehicle = cars[i].GetComponent<Vehicle>();

                        cros.AddVehicle(vehicle);
                        cars[i].transform.position = cros.center;
                        i++;
                    }
                    else break;
                }
            }
            
        }

    }
}
