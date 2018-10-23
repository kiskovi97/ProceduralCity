using UnityEngine;

public interface IValues : IGraphValues
{
    float GetTextureValue(Vector3 position);

    float getSize();

    void SetSize(float[] sizes);

    float RoadSize { get; }

    float Floor { get; }

    float MinHouse { get; }

    float HouseUpmin { get; set; }

    float HouseUpmax { get; set; }
}
