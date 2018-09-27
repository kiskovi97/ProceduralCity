using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.AdvancedCity
{
    public class MenuScript : MonoBehaviour
    {
        public MainCityGenerator mainCityGenerator;
        public GameObject backgroundMenu;
        public void StartGame()
        {
            mainCityGenerator.GenerateEverything();
            backgroundMenu.SetActive(false);
        }
    }
}
