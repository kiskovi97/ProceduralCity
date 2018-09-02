
using Assets.Scripts.AdvancedCity.monoBeheviors.interactiveObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{

    class RoadandCrossingGenerator
    {
        float RoadSize = 0.05f;
        MyMath math = new MyMath();
        List<Crossing> crossings;
        List<Road> roads;
        

        public List<Crossing> GenerateObjects(GameObjectGenerator generator, List<GraphPoint> controlPoints, float sizeRatio)
        {
            crossings = new List<Crossing>();

            roads = new List<Road>();
            for (int i = 0; i < controlPoints.Count; i++)
            {
                GraphPoint point = controlPoints[i];
                crossings.Add(new Crossing(point.position, point.isMainRoad(), point.isTram(), generator));
            }
            for (int i = 0; i < controlPoints.Count; i++)
            {
                List<GraphPoint> szomszedok = controlPoints[i].Szomszedok;
                for (int j = 0; j < szomszedok.Count; j++)
                {
                    int x = controlPoints.IndexOf(szomszedok[j]);
                    if (x > i)
                    {
                        Road r = new Road(generator);
                        r.setSzomszedok(crossings[i], crossings[x]);
                        crossings[i].AddSzomszed(r);
                        crossings[x].AddSzomszed(r);
                        roads.Add(r);
                    }
                }
            }
            MakeMovementPoint(controlPoints, sizeRatio);
            return crossings;
        }
        private void MakeMovementPoint(List<GraphPoint> controlPoints, float sizeRatio)
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


                    float utelozo = RoadSize * sizeRatio;
                    if (!szomszedok[i].isSideRoad() && ezbool) utelozo += sizeRatio * RoadSize * 2;

                    float utekov = RoadSize * sizeRatio;
                    if (!szomszedok[kov].isSideRoad() && ezbool) utekov += sizeRatio * RoadSize * 2;
                    float sidewalk = RoadSize * sizeRatio;

                    Vector3 merolegeselozo = MyMath.Meroleges(ez, elozo).normalized * utelozo;
                    Vector3 merolegeskovetkezo = MyMath.Meroleges(kovetkezo, ez).normalized * utekov;
                    Vector3 merolegeselozo_side = MyMath.Meroleges(ez, elozo).normalized * (utelozo + sidewalk);
                    Vector3 merolegeskovetkezo_side = MyMath.Meroleges(kovetkezo, ez).normalized * (utekov + sidewalk);

                    Vector3 kereszt = MyMath.Intersect(
                        ez + merolegeselozo
                        , (elozo - ez).normalized
                        , ez + merolegeskovetkezo
                        , (kovetkezo - ez).normalized);
                    Vector3 kereszt_side = MyMath.Intersect(
                        ez + merolegeselozo_side
                        , (elozo - ez).normalized
                        , ez + merolegeskovetkezo_side
                        , (kovetkezo - ez).normalized);
                    sidewalks.Add(kereszt_side);

                    Vector3 meroleges_elozo = MyMath.Intersect(kereszt, merolegeselozo, ez, (elozo - ez).normalized);
                    lista[i][2] = meroleges_elozo;
                    lista[i][3] = kereszt;

                    Vector3 meroleges_kov = MyMath.Intersect(kereszt, merolegeskovetkezo, ez, (kovetkezo - ez).normalized);
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

                    if (r != null)
                        if (a > b)
                        {
                            Vector3 masikkereszt = MyMath.Intersect(lista[i][3], (szomszed - ez).normalized, lista[i][1], lista[i][0] - lista[i][1]);
                            r.addLine(cros, masikkereszt, lista[i][1]);
                            Vector3[] line = { masikkereszt, lista[i][1] };
                            Vector3[] helpline = { lista[i][3], masikkereszt };
                            cros.AddLines(line, helpline, lista[i][1], sidewalks[elozo], r);
                        }
                        else
                        {
                            Vector3 masikkereszt = MyMath.Intersect(lista[i][1], (szomszed - ez).normalized, lista[i][3], lista[i][2] - lista[i][3]);
                            Vector3[] line = { lista[i][3], masikkereszt };
                            Vector3[] helpline = { masikkereszt, lista[i][1] };
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
            foreach (Road road in roads)
            {
                road.Draw(depthtest);
            }
            foreach (Crossing cros in crossings)
            {
                cros.Draw(draw_helplines, depthtest);
            }
        }

        public void SetCarsStartingPosition(GameObject[] cars)
        {
            int i = 0;
            if (crossings == null) return;
            foreach (Crossing cros in crossings)
            {
                Vehicle vehicle;
                while (cros.HavePlace())
                {
                    if (i < cars.Length) vehicle = cars[i].GetComponent<Vehicle>();
                    else return;
                    if (cros.AddVehicle(vehicle))
                    {
                        cars[i].transform.position = vehicle.nextPoint.center;
                        i++;
                    }
                }
            }
            for (int j = i; j < cars.Length; j++)
            {
                cars[j].transform.position += new Vector3(0, 10, 0);
                GameObject.Destroy(cars[j]);
            }

        }

        public void SetTram(GameObject cars, GameObject cars2)
        {
            int i = 0;
            if (crossings == null) return;
            foreach (Crossing cros in crossings)
            {
                if (cros.tram)
                {
                    Tram vehicle = cars.GetComponent<Tram>();
                    Tram vehicle2 = cars2.GetComponent<Tram>();
                    cros.AddTram(vehicle, vehicle2);
                    cars.transform.position = cros.center;
                    cars2.transform.position = cros.center;
                    vehicle.setDirection();
                    vehicle.generateMore(5);
                    vehicle2.setDirection();
                    vehicle2.generateMore(5);
                    return;
                }
            }
        }

    }
}
