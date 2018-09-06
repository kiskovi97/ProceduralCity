using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BlockGeneratorBasic : BlockGenerator
{
    public class KontrolLine
    {
        public Vector3 normalPoint;
        public Vector3 deepPoint;
        public KontrolLine(Vector3 normalPoint, Vector3 deepPoint)
        {
            this.normalPoint = normalPoint;
            this.deepPoint = deepPoint;
        }
    }

    List<KontrolLine> elozoLine = new List<KontrolLine>();
    List<KontrolLine> nextLine = new List<KontrolLine>();
    List<BuildingObject> buildings = new List<BuildingObject>();
    RoadGeneratingValues values;
    public void Clear()
    {
        elozoLine.Clear();
        nextLine.Clear();
        foreach (BuildingObject building in buildings)
        {
            building.DestorySelf();
        }
        buildings.Clear();
    }

    public void GenerateBuildings(List<Vector3> vertexes, BuildingContainer buildingContainer)
    {
        bool sikeres = true;
        if (vertexes.Count == 3)
        {
            float area = MyMath.Area(vertexes[0], vertexes[1], vertexes[2]);
            if (area < values.HouseDeepmax * values.HouseDeepmax) return;
        }
        elozoLine.Clear();
        nextLine.Clear();
        List<Vector3> crossings = new List<Vector3>();
        for (int i=0; i<vertexes.Count; i++)
        {
            List<Vector3> kontrolpoints = new List<Vector3>();
            int elozo = i - 1;
            if (elozo < 0) elozo = vertexes.Count - 1;
            int kovetkezo = i + 1;
            if (kovetkezo >= vertexes.Count) kovetkezo = 0;
            Vector3 merolegesKovetkezo = MyMath.Meroleges(vertexes[i], vertexes[kovetkezo]).normalized * values.minHouse;
            Vector3 nextDir = (vertexes[kovetkezo] - vertexes[i]).normalized * values.minHouse;
            Vector3 merolegesElozo = MyMath.Meroleges(vertexes[elozo], vertexes[i]).normalized * values.minHouse;
            Vector3 elozoDir = (vertexes[elozo] - vertexes[i]).normalized * values.minHouse;
            Vector3 crossingPoint = MyMath.Intersect(vertexes[i] + merolegesElozo, elozoDir,
                vertexes[i] + merolegesKovetkezo, nextDir);
            Vector3 elozoHousePoint;
            Vector3 nextHousePoint;
            Vector3 elozoDeep;
            Vector3 nextDeep;
            float angle = Vector3.SignedAngle(elozoDir, nextDir, new Vector3(0,1,0));
            if (angle > 90 || angle < -90)
            {
                elozoHousePoint = vertexes[i] + elozoDir;
                nextHousePoint = vertexes[i] + nextDir;
                elozoDeep = elozoHousePoint + merolegesElozo;
                nextDeep = nextHousePoint + merolegesKovetkezo;
                kontrolpoints.Add(nextDeep);
                kontrolpoints.Add(nextHousePoint);
                kontrolpoints.Add(vertexes[i]);
                kontrolpoints.Add(elozoHousePoint);
                kontrolpoints.Add(elozoDeep);
                kontrolpoints.Add(crossingPoint);
                kontrolpoints.Reverse();
                elozoLine.Add(new KontrolLine(elozoHousePoint, elozoDeep));
                nextLine.Add(new KontrolLine(nextHousePoint, nextDeep));
            } else
            {
                if (angle < 0)
                {
                    elozoHousePoint = crossingPoint - merolegesElozo;
                    nextHousePoint = crossingPoint - merolegesKovetkezo;
                    elozoDeep = elozoHousePoint - elozoDir;
                    nextDeep = nextHousePoint - nextDir;
                    if (!MyMath.Between(vertexes[elozo], vertexes[i], elozoDeep) || !MyMath.Between(vertexes[elozo], vertexes[i], elozoHousePoint))
                    {
                        Debug.DrawLine(elozoHousePoint, elozoDeep, Color.black, 100.0f);
                        sikeres = false;
                    } 
                    if (!MyMath.Between(vertexes[kovetkezo], vertexes[i], nextHousePoint) || !MyMath.Between(vertexes[kovetkezo], vertexes[i], nextDeep))
                    {
                        Debug.DrawLine(nextDeep, nextHousePoint, Color.black, 100.0f);
                        sikeres = false;
                    } 
                    kontrolpoints.Add(elozoHousePoint);
                    kontrolpoints.Add(elozoDeep);
                    kontrolpoints.Add(nextDeep);
                    kontrolpoints.Add(nextHousePoint);
                    kontrolpoints.Add(crossingPoint);
                        GameObject me = buildingContainer.building;
                        me.name = "BASE";
                        me.transform.position = vertexes[i];
                        BuildingObject baseBuild = me.GetComponent<BuildingObject>();
                        baseBuild.MakeBase(elozoDeep, vertexes[i], nextDeep);
                    buildings.Add(baseBuild);
                    elozoLine.Add(new KontrolLine(elozoHousePoint, crossingPoint));
                    nextLine.Add(new KontrolLine(nextHousePoint, crossingPoint));

                } else
                {
                    elozoHousePoint = vertexes[i] + merolegesElozo;
                    nextHousePoint = vertexes[i] + merolegesKovetkezo;
                    elozoDeep = elozoHousePoint - elozoDir;
                    nextDeep = nextHousePoint - nextDir;
                    kontrolpoints.Add(elozoHousePoint);
                    kontrolpoints.Add(elozoDeep);
                    kontrolpoints.Add(nextDeep);
                    kontrolpoints.Add(nextHousePoint);
                    kontrolpoints.Add(vertexes[i]);
                    kontrolpoints.Reverse();
                    elozoLine.Add(new KontrolLine(vertexes[i], elozoHousePoint));
                    nextLine.Add(new KontrolLine(vertexes[i], nextHousePoint));
                }
            }
            crossings.Add(crossingPoint);
            GameObject gameObject = buildingContainer.building;
            gameObject.transform.position = vertexes[0];
            BuildingObject building = gameObject.GetComponent<BuildingObject>();
            building.MakeBuilding(kontrolpoints, (int)values.HouseUpmin, (int)values.HouseUpmax, values.floor, values);
            buildings.Add(building);
        }
        for (int i=0; i< vertexes.Count; i++)
        {
            int elozo = i - 1;
            if (elozo < 0) elozo = vertexes.Count - 1;
            int kovetkezo = i + 1;
            if (kovetkezo >= vertexes.Count) kovetkezo = 0;
            MakeSideBuildings(nextLine[elozo], elozoLine[i], buildingContainer);
        }
        if (!sikeres)
            Clear();
    }
    private void MakeSideBuildings(KontrolLine elozo, KontrolLine kovetkezo, BuildingContainer buildingContainer)
    {
        List<Vector3> kontrolpoints = new List<Vector3>();

        float angle = Vector3.SignedAngle(elozo.deepPoint - elozo.normalPoint, kovetkezo.normalPoint - elozo.normalPoint, new Vector3(0,1,0));
        if (angle > 0)
        {
            return;
        }

        float length = (elozo.deepPoint - kovetkezo.deepPoint).magnitude;
        if (length > values.minHouse*2)
        {
            Vector3 tmpDeep = kovetkezo.deepPoint + (elozo.deepPoint - kovetkezo.deepPoint).normalized * values.minHouse;
            Vector3 tmpNormal = kovetkezo.normalPoint + (elozo.normalPoint - kovetkezo.normalPoint).normalized * values.minHouse;
            kontrolpoints.Add(tmpDeep);
            kontrolpoints.Add(tmpNormal);
            kontrolpoints.Add(kovetkezo.normalPoint);
            kontrolpoints.Add(kovetkezo.deepPoint);
            MakeSideBuildings(elozo , new KontrolLine(tmpNormal, tmpDeep), buildingContainer);
        } else
        {
            kontrolpoints.Add(elozo.deepPoint);
            kontrolpoints.Add(elozo.normalPoint);
            kontrolpoints.Add(kovetkezo.normalPoint);
            kontrolpoints.Add(kovetkezo.deepPoint);
        }
        GameObject gameObject = buildingContainer.building;
        gameObject.transform.position = elozo.normalPoint;
        BuildingObject building = gameObject.GetComponent<BuildingObject>();
        building.MakeBuilding(kontrolpoints, (int)values.HouseUpmin, (int)values.HouseUpmax, values.floor, values);
        buildings.Add(building);
    }
   
    public void SetValues(RoadGeneratingValues values)
    {
        this.values = values;
    }
}
