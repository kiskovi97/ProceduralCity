using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    public class Vehicles : MonoBehaviour
    {
        public List<GameObject> cars;
        public GameObject cameraCar;
        public GameObject tram;
        public GameObject Tram
        {
            get
            {
                GameObject ki = Instantiate(tram);
                return ki;
            }
        }
        public GameObject Car {
            get
            {
                
                int i = (int)( Random.value * cars.Count);
                GameObject real = Instantiate(cars[i]);
                return real;
            }
        }
    }
}
