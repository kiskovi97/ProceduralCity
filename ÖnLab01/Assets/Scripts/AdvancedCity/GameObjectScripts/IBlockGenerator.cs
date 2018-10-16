using UnityEngine;

public interface IBlockGenerator 
{
    void Clear();
    void SetValues(IValues values);
    void GenerateBuildings(Vector3[] vertexes, BuildingContainer buildingContainer);
}

