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
        public Slider slider;
        public Camera camera;
        public Toggle toggle;
        public Slider sliderBlocks;
        bool graph = false;
        bool road = false;
        bool block = false;
        bool cars = false;
        private void Start()
        {
            mainCityGenerator.MakeLamps = toggle.isOn;
            toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(toggle); });
            Debug.Log(toggle.isOn);
        }
        void ToggleValueChanged(Toggle change)
        {
            mainCityGenerator.MakeLamps = change.isOn;
            Debug.Log(toggle.isOn);
        }
        public void StartGame()
        {
            sliderBlocks.value = 0;
            mainCityGenerator.GenerateEverything(Step);
            backgroundMenu.SetActive(false);
            camera.enabled = false;
        }
        public void ExportObjects()
        {
            mainCityGenerator.Export();
        }
        public void CreateGraph()
        {
            mainCityGenerator.GenerateOnlyGraph(true);
            graph = true;
        }
        public void CreateRoads()
        {
            if (!graph) mainCityGenerator.GenerateOnlyGraph(false);
            if (!road) mainCityGenerator.DrawRoads();
            graph = true;
            road = true;
        }

        public void Step(float step)
        {
            sliderBlocks.value += step;
        }

        public void CreateBlocks()
        {
            sliderBlocks.value = 0;
            if (!graph) mainCityGenerator.GenerateOnlyGraph(false);
            if (!block) mainCityGenerator.DrawBlocks(Step);
            graph = true;
            block = true;
        }
        public void GenerateCars()
        {
            if (!graph) mainCityGenerator.GenerateOnlyGraph(false);
            if (!cars) mainCityGenerator.GenerateCars();
            backgroundMenu.SetActive(false);
            camera.enabled = false;
            graph = true;
            //cars = true;
        }
        public void Clear()
        {
            mainCityGenerator.Clear();
            GameObject[] list = GameObject.FindGameObjectsWithTag("Generated");
            StartCoroutine(AllDestroy(list));
        }
        System.Collections.IEnumerator AllDestroy(GameObject[] list)
        {
            slider.value = 0;
            float step = 1.0f / list.Length;
            int refresh = 0;
            foreach (GameObject obj in list)
            {
                Destroy(obj);
                slider.value += step;
                refresh++;
                if (refresh > 100)
                {
                    yield return null;
                    refresh = 0;
                }
            }
            slider.value = 1;
            graph = false;
            road = false;
            block = false;
            cars = false;
            camera.enabled = true;
        }

        public void EndGame()
        {
            Debug.Log("Exit");
            Application.Quit();
        }
    }
}
