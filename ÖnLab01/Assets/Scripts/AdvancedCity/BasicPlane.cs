using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlane : MonoBehaviour {

    public RoadGeneratingValues values;
    // Use this for initialization
    void Start () {
        float xvalue = values.size.xMax - values.size.xMin;
        float zvalue = values.size.zMax - values.size.zMin;
        float centerx = (values.size.xMax + values.size.xMin) / 2;
        float centerz = (values.size.zMax + values.size.zMin) / 2;
        transform.position = new Vector3(centerx, -0.02f, centerz);
        transform.localScale = new Vector3(xvalue/10, 1, zvalue/10);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
