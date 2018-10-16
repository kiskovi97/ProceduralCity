using UnityEngine;
using System.Collections.Generic;
namespace Assets.Scripts.AdvancedCity
{
    [System.Serializable]
    public class VehiclesObject : System.Object, IVehicles
    {
        public List<GameObject> cars;
        public GameObject cameraCar;
        public GameObject tram;
        public GameObject Tram
        {
            get
            {
                return tram;
            }
        }
        public GameObject Car
        {
            get
            {
                int i = (int)(Random.value * cars.Count);
                return cars[i];
            }
        }

        public GameObject CameraCar
        {
            get
            {
                return cameraCar;
            }
        }
    }
}
