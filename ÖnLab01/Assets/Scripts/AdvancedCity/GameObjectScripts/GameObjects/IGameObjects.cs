using UnityEngine;

public interface IGameObjects
{
    GameObject roadObject { get;}
    GameObject crossLamp { get;}
    GameObject sideLamp { get;}
    GameObject wireObject { get;}
    GameObject railObject { get;}
    GameObject stoppingObj { get; }
    GameObject Building
    {
        get;
    }
    GameObject BuildingBySize(float size);
    GameObject Place
    {
        get;
    }
}