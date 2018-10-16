using UnityEngine;
using System.Collections;

public interface IGraphValues
{

    Vector3 StartingPoint();

    bool WithinRange(Vector3 position);

    float CollapseRangeMainRoad { get; }

    float CollapseRangeSideRoad { get; }

    float RoadsDistancesMainRoad { get; }

    float RoadsDistancesSideRoad { get; }

    float StraightFreqMainRoad { get; }

    float StraightFreqSideRoad { get; }

    int MaxCrossings { get; }

    float RotationRandomMainRoad { get; }

    float RotationRandomSideRoad { get; }

    float SideRoadFreq { get; }

    float SmootIntensity { get; }

}
