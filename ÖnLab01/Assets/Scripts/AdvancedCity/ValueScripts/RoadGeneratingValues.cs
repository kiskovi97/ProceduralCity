using UnityEngine;

[System.Serializable]
public class RoadGeneratingValues : System.Object, IValues
{
    
    public Texture2D map;

    public float GetTextureValue(Vector3 position)
    {
        float x_arany = (position.x - sizes[0] * sizeRatio) / (sizes[2] * sizeRatio - sizes[0] * sizeRatio);
        float y_arany = (position.z - sizes[1] * sizeRatio) / (sizes[3] * sizeRatio - sizes[1] * sizeRatio);
        if (map == null) return 0.5f;
        return map.GetPixelBilinear(x_arany, y_arany).r;
    }

    [Header("Size of the city")]
    [SerializeField]
    private float[] sizes =
        {
            -3, -3,  3, 3
        };

    public float getSize()
    {
        return sizes[2];
    }

    public void SetSize(float[] sizes)
    {
        this.sizes = sizes;
    }

    public Vector3 StartingPoint()
    {
        return new Vector3((sizes[0] * sizeRatio + sizes[2] * sizeRatio) / 2, 0, sizes[1] * sizeRatio + 1);

    }

    public bool WithinRange(Vector3 position)
    {
        return !(position.x < sizes[0] * sizeRatio || position.x > sizes[2] * sizeRatio
              || position.z < sizes[1] * sizeRatio || position.z > sizes[3] * sizeRatio);
    }

    [Header("Size of the city")]
    [SerializeField]
    private float sizeRatio = 5;
    [Space(20)]
    [Header("Two Point collapse, when its this close")]
    [SerializeField]
    private float collapseMainRoad = 1.8f;
    public float CollapseRangeMainRoad
    {
        get
        {
            return collapseMainRoad * sizeRatio;
        }
    }
    [SerializeField]
    private float CollapseSideRoad = 1.8f;
    public float CollapseRangeSideRoad
    {
        get
        {
            return CollapseSideRoad * sizeRatio;
        }
    }
    [SerializeField]
    private float roadSize = 0.1f;
    public float RoadSize
    {
        get
        {
            return roadSize * sizeRatio;
        }
    }
    [Space(5)]
    [Header("TBasic road size")]
    [SerializeField]
    private float MainRoadDistance = 2;
    public float RoadsDistancesMainRoad
    {
        get
        {
            return MainRoadDistance * sizeRatio;
        }
    }
    [SerializeField]
    private float SideRoadsDistance = 2;
    public float RoadsDistancesSideRoad
    {
        get
        {
            return SideRoadsDistance * sizeRatio;
        }
    }
    [Space(5)]
    [Header("How Often generate Straigth Roads")]
    [Range(0, 1)]
    [SerializeField]
    private float straightFreqMainRoad = 0.9f;
    [Range(0, 1)]
    [SerializeField]
    private float straightFreqSideRoad = 0.3f;
    [Space(5)]
    [Header("How many Roads Cross without collapse")]
    [SerializeField]
    private int maxCrossings = 4;
    [Space(5)]
    [Header("Straigth Road rotate from the basic direction")]
    [Range(0, 1)]
    [SerializeField]
    private float rotationRandomMainRoad = 0.2f;
    [Range(0, 1)]
    [SerializeField]
    private float rotationRandomSideRoad = 0;
    [Space(5)]
    [Header("How Often make Side Road from Main Road")]
    [Range(0, 1)]
    [SerializeField]
    private float sideRoadFreq = 0.8f;
    [Space(5)]
    [Header("Road Smoothing variable")]
    [Range(0, 0.5f)]
    [SerializeField]
    private float smootIntensity = 0.1f;

    [SerializeField]
    private float houseSize = 0.5f;
    [SerializeField]
    private float HouseUpminSize = 1f;
    [SerializeField]
    private float HouseUpmaxSize = 10f;

    [SerializeField]
    private float floorSize = 0.05f;
    public float Floor
    {
        get
        {
            return floorSize * sizeRatio;
        }
    }

    public float MinHouse
    {
        get
        {
            return houseSize * sizeRatio;
        }
    }
    public float HouseUpmin
    {
        get
        {
            return HouseUpminSize;
        }
        set
        {
            HouseUpminSize = value;
        }
    }
    public float HouseUpmax
    {
        get
        {
            return HouseUpmaxSize;
        }
        set
        {
            HouseUpmaxSize = value;
        }
    }

    public float StraightFreqMainRoad
    {
        get
        {
            return straightFreqMainRoad;
        }
    }

    public float StraightFreqSideRoad
    {
        get
        {
            return straightFreqSideRoad;
        }
    }

    public int MaxCrossings
    {
        get
        {
            return maxCrossings;
        }
    }

    public float RotationRandomMainRoad
    {
        get
        {
            return rotationRandomMainRoad;
        }
    }

    public float RotationRandomSideRoad
    {
        get
        {
            return rotationRandomSideRoad;
        }
    }

    public float SideRoadFreq
    {
        get
        {
            return sideRoadFreq;
        }
    }

    public float SmootIntensity
    {
        get
        {
            return smootIntensity;
        }
    }
}
