using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface BlockGenerator 
{
    void Clear();
    void SetValues(RoadGeneratingValues values);
    void GenerateBuildings(List<Vector3> vertexes, BuildingContainer buildingContainer);
}

