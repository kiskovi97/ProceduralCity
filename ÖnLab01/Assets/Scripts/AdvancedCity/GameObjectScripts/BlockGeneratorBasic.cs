using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class BlockGeneratorBasic : IBlockGenerator
{
    public class ControlLine
    {
        public Vector3 prevSidePoint;
        public Vector3 prevDeepPoint;
        public Vector3 nextSidePoint;
        public Vector3 nextDeepPoint;
        public ControlLine(Vector3 prevSidePoint, Vector3 prevDeepPoint, Vector3 nextSidePoint, Vector3 nextDeepPoint)
        {
            this.prevDeepPoint = prevDeepPoint;
            this.nextDeepPoint = nextDeepPoint;
            this.prevSidePoint = prevSidePoint;
            this.nextSidePoint = nextSidePoint;
        }
        public ControlLine()
        {
        }
    }
    private ControlLine[] lines = null;
    private List<BuildingObject> buildings = new List<BuildingObject>();
    private IValues values;
    private readonly bool export = false;
    private readonly bool elso = true;
    public void Clear()
    {
        foreach (BuildingObject building in buildings)
        {
            building.DestorySelf();
        }
        buildings.Clear();
    }

    public void SetValues(IValues values)
    {
        this.values = values;
    }

    public void GenerateBuildings(Vector3[] vertexes, BuildingContainer buildingContainer)
    {
        Clear();
        MakePlace(buildingContainer.Place, vertexes);
        lines = new ControlLine[vertexes.Length];
        for (int i = 0; i < vertexes.Length; i++) lines[i] = new ControlLine();
        if (vertexes.Length == 3)
        {
            float area = MyMath.Area(vertexes[0], vertexes[1], vertexes[2]);
            if (area < values.MinHouse * values.MinHouse) return;
        }
        bool succes = true;
        for (int i = 0; i < vertexes.Length; i++)
        {
            int prev = i - 1;
            if (prev < 0) prev = vertexes.Length - 1;
            int next = i + 1;
            if (next >= vertexes.Length) next = 0;
            Vector3[] controlPoints = MakeCorner(vertexes[prev], vertexes[i], vertexes[next], i, prev);
            if (controlPoints != null)
            {
                MakeBuilding(controlPoints, buildingContainer);
            }
            else
            {
                succes = false;
                break;
            }
        }
        for (int i = 0; i < lines.Length; i++)
        {
            MakeSideBuildings(lines[i], buildingContainer);
        }
        if (!succes)
            Clear();
    }

    private void MakePlace(GameObject placeObject, Vector3[] vertexes)
    {
        placeObject.transform.position = vertexes[0];
        GreenPlace place = placeObject.GetComponent<GreenPlace>();
        place.MakePlace(vertexes);
    }

    private Vector3[] MakeCorner(Vector3 prev, Vector3 actual, Vector3 next, int i, int elozo)
    {
        Vector3 nextDeepDir = MyMath.Meroleges(actual, next).normalized * values.MinHouse;
        Vector3 nextDir = (next - actual).normalized * values.MinHouse;
        Vector3 prevDeepDir = MyMath.Meroleges(prev, actual).normalized * values.MinHouse;
        Vector3 prevDir = (prev - actual).normalized * values.MinHouse;
        Vector3 crossingPoint = MyMath.Intersect(actual + prevDeepDir, prevDir, actual + nextDeepDir, nextDir);
        List<Vector3> controlPoints = new List<Vector3>();
        float angle = Vector3.SignedAngle(prevDir, nextDir, new Vector3(0, 1, 0));
        if (angle > 90 || angle < -90)
        {
            Vector3 prevHousePoint = actual + prevDir;
            Vector3 nextHousePoint = actual + nextDir;
            Vector3 prevDeep = prevHousePoint + prevDeepDir;
            Vector3 nextDeep = nextHousePoint + nextDeepDir;
            controlPoints.Add(crossingPoint);
            controlPoints.Add(prevDeep);
            controlPoints.Add(prevHousePoint);
            controlPoints.Add(actual);
            controlPoints.Add(nextHousePoint);
            controlPoints.Add(nextDeep);
            lines[elozo].nextSidePoint = prevHousePoint;
            lines[elozo].nextDeepPoint = prevDeep;
            lines[i].prevSidePoint = nextHousePoint;
            lines[i].prevDeepPoint = nextDeep;
        }
        else
        {
            if (angle < 0)
            {
                Vector3 prevHousePoint = crossingPoint - prevDeepDir;
                Vector3 nextHousePoint = crossingPoint - nextDeepDir;
                Vector3 prevDeep = prevHousePoint - prevDir;
                Vector3 nextDeep = nextHousePoint - nextDir;
                if (!MyMath.Between(prev, actual, prevDeep) || !MyMath.Between(prev, actual, prevHousePoint))
                {
                    Debug.DrawLine(prevHousePoint, prevDeep, Color.black, 100.0f);
                    return null;
                }
                if (!MyMath.Between(next, actual, nextHousePoint) || !MyMath.Between(next, actual, nextDeep))
                {
                    Debug.DrawLine(nextDeep, nextHousePoint, Color.black, 100.0f);
                    return null;
                }
                if (i > 0 && MyMath.Between(lines[i].prevSidePoint, prev, prevHousePoint))
                {
                    Debug.DrawLine(lines[i].prevSidePoint, prevHousePoint, Color.red, 100.0f);
                    return null;
                }
                controlPoints.Add(prevHousePoint);
                controlPoints.Add(prevDeep);
                controlPoints.Add(nextDeep);
                controlPoints.Add(nextHousePoint);
                controlPoints.Add(crossingPoint);
                lines[elozo].nextSidePoint = prevHousePoint;
                lines[elozo].nextDeepPoint = crossingPoint;
                lines[i].prevSidePoint = nextHousePoint;
                lines[i].prevDeepPoint = crossingPoint;
            }
            else
            {
                Vector3 prevHousePoint = actual + prevDeepDir;
                Vector3 nextHousePoint = actual + nextDeepDir;
                Vector3 prevDeep = prevHousePoint - prevDir;
                Vector3 nextDeep = nextHousePoint - nextDir;
                controlPoints.Add(actual);
                controlPoints.Add(nextHousePoint);
                controlPoints.Add(prevDeep);
                controlPoints.Add(nextDeep);
                controlPoints.Add(prevHousePoint);
                lines[elozo].nextSidePoint = actual;
                lines[elozo].nextDeepPoint = prevHousePoint;
                lines[i].prevSidePoint = actual;
                lines[i].prevDeepPoint = nextHousePoint;
            }
        }
        return controlPoints.ToArray();
    }

    private void MakeBuilding(Vector3[] controlPoints, BuildingContainer buildingContainer)
    {
        Vector3 actual = controlPoints[0];
        int min = (int)values.HouseUpmin;
        int max = (int)values.HouseUpmax;
        float positionValue = (values.GetTextureValue(actual)) * (values.GetTextureValue(actual));
        int floorNumber = (int)((Random.value * 0.5 + 0.5) * (max - min) * positionValue) + min;
        GameObject gameObject = buildingContainer.BuildingBySize(values.GetTextureValue(actual));
        gameObject.transform.position = actual;
        BuildingObject building = gameObject.GetComponent<BuildingObject>();
        building.MakeBuilding(controlPoints, floorNumber, values.Floor);
        buildings.Add(building);
    }

    private void MakeSideBuildings(ControlLine controlLine, BuildingContainer buildingContainer)
    {
        List<Vector3> kontrolpoints = new List<Vector3>();
        float angle = Vector3.SignedAngle(controlLine.prevDeepPoint - controlLine.prevSidePoint, controlLine.nextSidePoint - controlLine.prevSidePoint, new Vector3(0, 1, 0));
        if (angle > 0)
        {
            return;
        }
        float length = (controlLine.prevDeepPoint - controlLine.nextDeepPoint).magnitude;
        if (length > values.MinHouse * 2)
        {
            Vector3 tmpDeep = controlLine.nextDeepPoint + (controlLine.prevDeepPoint - controlLine.nextDeepPoint).normalized * values.MinHouse;
            Vector3 tmpNormal = controlLine.nextSidePoint + (controlLine.prevSidePoint - controlLine.nextSidePoint).normalized * values.MinHouse;
            kontrolpoints.Add(tmpDeep);
            kontrolpoints.Add(tmpNormal);
            kontrolpoints.Add(controlLine.nextSidePoint);
            kontrolpoints.Add(controlLine.nextDeepPoint);
            MakeSideBuildings(new ControlLine(controlLine.prevSidePoint, controlLine.prevDeepPoint, tmpNormal, tmpDeep), buildingContainer);
        }
        else
        {
            kontrolpoints.Add(controlLine.prevDeepPoint);
            kontrolpoints.Add(controlLine.prevSidePoint);
            kontrolpoints.Add(controlLine.nextSidePoint);
            kontrolpoints.Add(controlLine.nextDeepPoint);
        }
        MakeBuilding(kontrolpoints.ToArray(), buildingContainer);
    }
}
