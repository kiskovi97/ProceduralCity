using UnityEngine;

[System.Serializable]
public class GameObjects : System.Object, IGameObjects
{
    public GameObject RoadObject;
    public GameObject CrossLamp;
    public GameObject SideLamp;
    public GameObject WireObject;
    public GameObject RailObject;
    public GameObject StoppingObj;
    public GameObject[] buildings;
    public GameObject[] greenPlace;
    public GameObject Building
    {
        get
        {
            int i = (int)(Random.value * (buildings.Length));
            return buildings[i];
        }
    }
    public GameObject BuildingBySize(float size)
    {
        if (size > 1) size = 0;
        if (size < 0) size = 0;
        int i = (int)(size * (buildings.Length));
        return buildings[i];
    }
    public GameObject Place
    {
        get
        {
            int i = (int)(Random.value * (greenPlace.Length));
            return greenPlace[i];
        }
    }
    public GameObject roadObject {
        get
        {
            return RoadObject;
        }
    }
    public GameObject crossLamp
    {
        get
        {
            return CrossLamp;
        }
    }
    public GameObject sideLamp
    {
        get
        {
            return SideLamp;
        }
    }
    public GameObject wireObject
    {
        get
        {
            return WireObject;
        }
    }
    public GameObject railObject
    {
        get
        {
            return RailObject;
        }
    }
    public GameObject stoppingObj
    {
        get
        {
            return StoppingObj;
        }
    }
}