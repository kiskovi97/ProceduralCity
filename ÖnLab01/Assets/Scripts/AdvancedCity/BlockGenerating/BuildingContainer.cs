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
}
