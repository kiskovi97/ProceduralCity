using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.AdvancedCity
{
    public class MenuScript : MonoBehaviour
    {
        public MainCityGenerator mainCityGenerator;
        public Slider slider;
        public new Camera camera;
        public Toggle toggle;
        public Toggle toggleTram;
        public Slider sliderBlocks;
        public InputField inputNumber;
        public Image background;
        readonly float viewSize = 7f;
        bool graph = false;
        bool road = false;
        bool block = false;
        bool cars = false;
        public bool pause = true;
        private void Start()
        {
            mainCityGenerator.MakeLamps = toggle.isOn;
            toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
            mainCityGenerator.trams = toggleTram.isOn;
            toggleTram.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
            inputNumber.text = mainCityGenerator.GetSize().ToString();
            inputNumber.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
            background.gameObject.SetActive(pause);
            camera.enabled = pause;
        }
        private void Update()
        {
            if (Input.GetButtonDown("Pause"))
            {
                pause = !pause;
            }
            background.gameObject.SetActive(pause);
            Cursor.visible = pause;
        }
        void ToggleValueChanged()
        {
            mainCityGenerator.MakeLamps = toggle.isOn;
            if (!cars && !road)
                mainCityGenerator.trams = toggleTram.isOn;
            else
                toggleTram.isOn = mainCityGenerator.trams;
        }
        public void StartGame()
        {
            sliderBlocks.value = 0;
            mainCityGenerator.GenerateEverything(Step);
            camera.enabled = false;
            pause = false;
        }
        public void ExportObjects()
        {
            mainCityGenerator.Export(100);
        }
        public void CreateGraph()
        {
            float size = float.Parse(inputNumber.text);
            mainCityGenerator.SetSize(size);
            camera.orthographicSize = size * viewSize;
            mainCityGenerator.GenerateOnlyGraph(true);
            graph = true;
        }
        private void CreateGraphOther()
        {

            float size = float.Parse(inputNumber.text);
            mainCityGenerator.SetSize(size);
            camera.orthographicSize = size * viewSize;
            mainCityGenerator.GenerateOnlyGraph(false);
            graph = true;
        }
        public void CreateRoads()
        {
            if (!graph) CreateGraphOther();
            if (!road) mainCityGenerator.DrawRoads();
            graph = true;
            road = true;
        }

        public void Step(float step)
        {
            pause = true;
            sliderBlocks.value += step;
            if (sliderBlocks.value >= 0.99f && cars) pause = false;
        }

        public void CreateBlocks()
        {
            sliderBlocks.value = 0;
            if (!graph) CreateGraphOther();
            if (!block)
                mainCityGenerator.DrawBlocks(Step);
            graph = true;
            block = true;
        }
        public void GenerateCars()
        {
            if (!graph) CreateGraphOther();
            mainCityGenerator.GenerateCars();
            camera.enabled = false;
            graph = true;
            cars = true;
            pause = false;
        }
        public void Clear()
        {
            mainCityGenerator.Clear();
            GameObject[] list = GameObject.FindGameObjectsWithTag("Generated");
            StartCoroutine(AllDestroy(list));
            mainCityGenerator.MakeLamps = toggle.isOn;
            mainCityGenerator.trams = toggleTram.isOn;
            float size = float.Parse(inputNumber.text);
            mainCityGenerator.SetSize(size);
            camera.orthographicSize = size * viewSize;
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
            pause = true;
        }

        public void EndGame()
        {
            Debug.Log("Exit");
            Application.Quit();
        }
    }
}
