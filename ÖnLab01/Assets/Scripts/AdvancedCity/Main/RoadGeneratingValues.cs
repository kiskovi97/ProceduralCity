﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGeneratingValues : MonoBehaviour {

    public Texture2D map;
    public float getTextureValue(Vector3 position)
    {
        float x_arany = (position.x - Sizes[0] * sizeRatio) / (Sizes[2] * sizeRatio - Sizes[0] * sizeRatio);
        float y_arany = (position.z - Sizes[1] * sizeRatio) / (Sizes[3] * sizeRatio - Sizes[1] * sizeRatio);
        return map.GetPixelBilinear(x_arany, y_arany).r;
    }

    [Header("Size of the city")]
    public float[] Sizes =
        {
            -10, -10,  10, 10
        };
    public Vector3 StartingPoint()
    {
        return new Vector3((Sizes[0] * sizeRatio + Sizes[2] * sizeRatio)/2, 0, Sizes[1] * sizeRatio + 1);

    }
    public bool PalyanBelulVane(Vector3 position)
    {
        return !(position.x < Sizes[0] * sizeRatio || position.x > Sizes[2] * sizeRatio
              || position.z < Sizes[1] * sizeRatio || position.z > Sizes[3] * sizeRatio);
    }

    [Header("Size of the city")]
    public float sizeRatio = 10;
    [Space(10)]
    
    [Space(10)]
    [Header("Two Point collapse, when its this close")]
    public float CollapseMainRoad = 0.5f;
    public float collapseRangeMainRoad {
        get
        {
            return CollapseMainRoad * sizeRatio;
        }
    }
    public float CollapseSideRoad = 0.3f;
    public float collapseRangeSideRoad {
        get
        {
            return CollapseSideRoad * sizeRatio;
        }
    }
    
    public float RoadSize = 0.1f;
    public float roadSize {
        get
        {
            return RoadSize * sizeRatio;
        }
    }
    [Space(5)]
    [Header("TBasic road size")]
    public float MainRoadDistance = 2;
    public float roadsDistancesMainRoad
    {
        get
        {
            return MainRoadDistance * sizeRatio;
        }
    }
    public float SideRoadsDistance = 1;
    public float roadsDistancesSideRoad
    {
        get
        {
            return SideRoadsDistance * sizeRatio;
        }
    }

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

    public float minHouseSize = 0.2f;
    public float HouseUpminSize = 0.2f;
    public float HouseUpmaxSize = 0.3f;
    public float HouseDeepminSize = 0.2f;
    public float HouseDeepmaxSize = 0.3f;


    public float floorSize = 0.1f;
    public float floor
    {
        get
        {
            return floorSize * sizeRatio;
        }
    }

    public float minHouse {
        get
        {
            return minHouseSize * sizeRatio;
        }
    }
    public float HouseUpmin
    {
        get
        {
            return HouseUpminSize ;
        }
    }
    public float HouseUpmax
    {
        get
        {
            return HouseUpmaxSize ;
        }
    }
    public float HouseDeepmin
    {
        get
        {
            return HouseDeepminSize * sizeRatio;
        }
    }
    public float HouseDeepmax
    {
        get
        {
            return HouseDeepmaxSize * sizeRatio;
        }
    }
}
