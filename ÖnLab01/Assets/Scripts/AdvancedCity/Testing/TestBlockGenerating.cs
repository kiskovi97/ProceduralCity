using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoadGeneratingValues))]
[RequireComponent(typeof(BuildingContainer))]
public class TestBlockGenerating : MonoBehaviour {

    public List<GameObject> vertexObjects;
    private RoadGeneratingValues values;
    public BuildingContainer buildingContainer;
    private bool update = true;
    public bool something = true;
    GameObject actual;
    BlockGenerator generator;
    // Use this for initialization
    void Start () {
        if (values == null)
        {
            values = GetComponent<RoadGeneratingValues>();
            if (values == null || !values.isActiveAndEnabled) throw new System.Exception("Need Values");
            
        }
        if (buildingContainer == null)
        {
            buildingContainer = GetComponent<BuildingContainer>();
            if (buildingContainer == null || !buildingContainer.isActiveAndEnabled) throw new System.Exception("Need BuildingContainer");
        }
        generator = new BlockGeneratorBasic();
        List<Vector3> vertexes = new List<Vector3>();
        foreach (GameObject road in vertexObjects)
        {
            vertexes.Add(road.transform.position);
        }
        Vector3 kozeppont = new Vector3(0, 0, 0);
        foreach (GameObject road in vertexObjects)
        {
            kozeppont += road.transform.position;
        }
        kozeppont /= vertexes.Count;
        generator.SetValues(values);
        generator.GenerateBuildings(vertexes, buildingContainer);
        update = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (something)
        {
            update = false;
            foreach (GameObject obj in vertexObjects)
            {
                if (obj.transform.hasChanged)
                {
                    update = true;
                    obj.transform.hasChanged = false;
                }
            }
            if (update)
            {
                generator.Clear();
                List<Vector3> vertexes = new List<Vector3>();
                foreach (GameObject road in vertexObjects)
                {
                    vertexes.Add(road.transform.position);
                }
                Vector3 kozeppont = new Vector3(0, 0, 0);
                foreach (GameObject road in vertexObjects)
                {
                    kozeppont += road.transform.position;
                }
                kozeppont /= vertexes.Count;
                generator.GenerateBuildings(vertexes, buildingContainer);
            }
        }
        
    }
}
