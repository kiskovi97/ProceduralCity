using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingContainer))]
public class TestBlockGenerating : MonoBehaviour {

    public List<GameObject> vertexObjects;
    public RoadGeneratingValues values = new RoadGeneratingValues();
    public BuildingContainer buildingContainer;
    private bool update = true;
    public bool something = true;
    GameObject actual;
    IBlockGenerator generator;
    // Use this for initialization
    void Start () { 
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
        generator.GenerateBuildings(vertexes.ToArray(), buildingContainer);
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
                generator.GenerateBuildings(vertexes.ToArray(), buildingContainer);
            }
        }
        
    }
}
