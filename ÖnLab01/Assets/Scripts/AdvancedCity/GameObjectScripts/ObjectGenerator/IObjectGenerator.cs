using UnityEngine;
using Assets.Scripts.AdvancedCity;
using System.Collections.Generic;

public interface IObjectGenerator
{
    void SetValues(IValues values, IGameObjects gameObjects);

    void AddStoppingMesh(Vector3 a, Vector3 b, Vector3 c, Vector3 d);

    void AddLine(Vector3 a, Vector3 b, float scale, float magassag, float magassag2);

    void CreateRails(Vector3 oneFrom, Vector3 oneToward, Vector3 otherFrom, Vector3 otherToward);

    void CreateRails(MovementPoint a, MovementPoint b, float size, Vector3 distanceVector, int mat);

    void AddRail(Vector3 a, Vector3 b, float scale);

    BuildingObject CreateBuilding(Vector3[] polygon);

    void CreateGreenPlace(Vector3[] polygon);

    GameObject CreateCrossLamp(Vector3 position, Vector3 forward, float magassag);

    GameObject CreateSideLamp(Vector3 position, Vector3 forward);

    void CreateCrossing(List<Vector3> polygon);

    void CreateRoad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int savok, bool tram, bool sideway, float zebra = 0.0f, float otherzebra = 0.0f);

    RoadPhysicalObject CreatRoadMesh();

}
