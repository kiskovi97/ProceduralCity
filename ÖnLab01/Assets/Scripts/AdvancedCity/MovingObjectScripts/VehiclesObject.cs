﻿using UnityEngine;
using System.Collections.Generic;
namespace Assets.Scripts.AdvancedCity
{
    [System.Serializable]
    public class VehiclesObject : System.Object, IVehicles
    {
        public GameObject[] cars;
        public GameObject cameraCar;
        public GameObject tram;
        public GameObject[] people;
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
                int i = (int)(Random.value * cars.Length);
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

        public GameObject Person
        {
            get
            {
                int i = (int)(Random.value * people.Length);
                return people[i];
            }
        }
    }
}
