using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    class Vehicles : MonoBehaviour
    {
        public List<GameObject> cars;
        public List<GameObject> trailers;
        public GameObject cameraCar;
        public GameObject Car {
            get
            {
                
                int i = (int)( Random.value * cars.Count);
                int j = (int)(Random.value * trailers.Count);
                GameObject real = Instantiate(cars[i]);
                if (trailers.Count != 0)
                {
                    GameObject masolat = real;
                    for (int x = 0; x < 2; x++)
                    {
                        Transform trs = masolat.transform;
                        foreach (Transform t in trs)
                        {
                            if (t.tag == "Connect")
                            {
                                GameObject obj = Instantiate(trailers[j]);
                                Trailer tr = obj.GetComponent<Trailer>();
                                tr.toObject = t;
                                masolat = obj;
                            }
                        }
                    }
                    
                }
                return real;
            }
        }
    }
}
