using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGeneratingValues : MonoBehaviour {

    [System.Serializable]
    public class Size
    {
        public float xMin = -10;
        public float zMin = -10;
        public float xMax = 10;
        public float zMax = 10;
    }
    [Header("Size of the city")]
    public Size size;
    [Space(10)]
    [Header("Two Point collapse, when its this close")]
    public float collapseRangeMainRoad = 0.5f;
    public float collapseRangeSideRoad = 0.3f;

    [Space(5)]
    [Header("TBasic road size")]
    public float roadsDistancesMainRoad = 2;
    public float roadsDistancesSideRoad = 1;

    [Space(5)]
    [Header("How Often generate Straigth Roads")]
    [Range(0, 1)]
    public float straightFreqMainRoad = 0.9f;
    [Range(0, 1)]
    public float straightFreqSideRoad = 0.9f;

    [Space(5)]
    [Header("How many Roads Cross without collapse")]
    public int maxCrossings = 4;

    [Space(5)]
    [Header("Straigth Road rotate from the basic direction")]
    [Range(0, 1)]
    public float rotationRandomMainRoad = 0.2f;
    [Range(0, 1)]
    public float rotationRandomSideRoad = 0;

    [Space(5)]
    [Header("How Often make Side Road from Main Road")]
    [Range(0, 1)]
    public float sideRoadFreq = 0.5f;

    [Space(5)]
    [Header("Road Smoothing variable")]
    [Range(0, 0.5f)]
    public float smootIntensity = 0.1f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
