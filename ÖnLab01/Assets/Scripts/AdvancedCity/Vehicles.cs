using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    class Vehicles : MonoBehaviour
    {
        public List<GameObject> cars;
        public GameObject Car {
            get
            {
                int i = (int)( Random.value * cars.Count);
                return cars[i];
            }
        }
    }
}
