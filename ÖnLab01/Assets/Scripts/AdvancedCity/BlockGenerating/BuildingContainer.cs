using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingContainer : MonoBehaviour {

    public GameObject[] buildings;
	public GameObject building{
        get {
            int i = (int)(Random.value * (buildings.Length));
            return Instantiate(buildings[i]);
        }
    }
    public GameObject buildingBySize(float size)
    {
        int i = (int)(size * (buildings.Length));
        return Instantiate(buildings[i]);
    }
    public GameObject[] greenPlace;
    public GameObject place
    {
        get
        {
            int i = (int)(Random.value * (greenPlace.Length));
            return Instantiate(greenPlace[i]);
        }
    }
}
