using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGeneratingValues : MonoBehaviour {

    // Palya merete
    public float xMin = -10;
    public float zMin = -10;
    public float xMax = 10;
    public float zMax = 10;
    // merge adatok
    public float kozelseg = 0.5f;
    public float kozelsegS = 0.3f;
    public float RoadsDistances = 2;
    public float SRoadsDistances = 1;
    // main road adatok
    public float straightFreq = 0.9f;
    public int MaxElagazas = 4;
    public float RotationRandom = 0.2f;
    // side road adatok
    public float straightFreqS = 0.9f;
    public float RotationRandomS = 0;
    public float SideRoadfreq = 0.5f;
    // Smooth
    public float smootIntensity = 0.1f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
