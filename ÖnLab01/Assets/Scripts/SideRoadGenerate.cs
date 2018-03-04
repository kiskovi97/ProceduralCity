using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideRoadGenerate : MonoBehaviour {
    public SideRoadObjects objects;
	// Use this for initialization
	void Start () {
        Invoke("GeneratePadokX", 0.5f);
        Invoke("GeneratePadokY", 1.0f);
        Invoke("GenerateLampaX", 0.1f);
        Invoke("GenerateLampaY", 0.1f);
    }
	
	// Update is called once per frame
	void GeneratePadokX () {
        GameObject pad = objects.PadX;
        float z = Random.value * 1.5f - 0.75f;
        pad.transform.SetParent(this.transform);
        pad.transform.localScale = new Vector3(0.75f,0.75f,0.75f);
        pad.transform.localRotation = new Quaternion(0, 0, 0, 0);
        pad.transform.Rotate(0, 90, 0);
        pad.transform.position = new Vector3(0,0,0);
        pad.transform.localPosition = new Vector3(-0.9f, 0.02f, z);
        
	}
    void GeneratePadokY()
    {
        GameObject pad = objects.PadX;
        float z = Random.value * 1.5f - 0.75f;
        pad.transform.SetParent(this.transform);
        pad.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        pad.transform.localRotation = new Quaternion(0, 0, 0, 0);
        pad.transform.Rotate(0, 270, 0);
        pad.transform.position = new Vector3(0, 0, 0);
        pad.transform.localPosition = new Vector3(0.9f, 0.02f, z);
    }
    void GenerateLampaY()
    {
        GameObject pad = objects.LampaX;
        float z = Random.value * 1.5f - 0.75f;
        pad.transform.SetParent(this.transform);
        pad.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        pad.transform.localRotation = new Quaternion(0, 0, 0, 0);
        pad.transform.Rotate(0, 270, 0);
        pad.transform.position = new Vector3(0, 0, 0);
        pad.transform.localPosition = new Vector3(0.9f, 0.02f, z);
    }
    void GenerateLampaX()
    {
        GameObject pad = objects.LampaX;
        float z = Random.value * 1.5f - 0.75f;
        pad.transform.SetParent(this.transform);
        pad.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        pad.transform.localRotation = new Quaternion(0, 0, 0, 0);
        pad.transform.Rotate(0, 90, 0);
        pad.transform.position = new Vector3(0, 0, 0);
        pad.transform.localPosition = new Vector3(-0.9f, 0.02f, z);

    }
}
