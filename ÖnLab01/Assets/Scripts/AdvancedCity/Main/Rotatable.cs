using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.AdvancedCity
{
    public class Rotatable : MonoBehaviour
    {
        RectTransform loader;
        private void Update()
        {
            if (loader == null)
            {
                loader = GetComponent<RectTransform>();
            }
            loader.Rotate(0, 0, 10.0f);
        }
    }
}
