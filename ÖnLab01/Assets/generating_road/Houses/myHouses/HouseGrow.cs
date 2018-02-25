using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGrow : MonoBehaviour {
    public List<GameObject> felettem;
    public float magas=2.0f;
    public float timing = 0.1f;
    public float merce = 2.0f;
	// Use this for initialization
	void Start () {
        Invoke("Grow", timing);
	}
	
	
	void Grow () {
        int i = (int)(Random.value * felettem.Count);
        if (i == felettem.Count) i--;
        GameObject ki = Instantiate(felettem[i]);
        ki.transform.position = transform.position + new Vector3(0, magas*merce/2.0f, 0);
        HouseGrow h = ki.GetComponent<HouseGrow>();
        if (h != null)
        {
            h.merce = merce;
        }
        ki.transform.rotation = transform.rotation;
        ki.transform.localScale = transform.localScale;
    }
}
