using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlockGenerating : MonoBehaviour {

    public List<GameObject> vertexObjects;
    public GameObject blockObject;
    GameObject actual;
    BlockObjectScript bos;
    // Use this for initialization
    void Start () {
        actual = GameObject.Instantiate(blockObject);
        bos = actual.GetComponent<BlockObjectScript>();
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
        bos.GenerateBlockMesh(vertexes, kozeppont);
        bos.CreateMesh();
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
