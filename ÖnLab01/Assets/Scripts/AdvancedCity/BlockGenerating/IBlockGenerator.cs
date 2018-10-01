using UnityEngine;

public interface IBlockGenerator 
{
    void Clear();
    void SetValues(RoadGeneratingValues values);
    void GenerateBuildings(Vector3[] vertexes, BuildingContainer buildingContainer);
}

