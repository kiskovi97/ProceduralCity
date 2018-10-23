using UnityEngine;
namespace Assets.Scripts.AdvancedCity
{
    public interface IVehicles
    {
        GameObject Tram { get; }
        GameObject Car { get; }
        GameObject CameraCar { get; }
    }
}
