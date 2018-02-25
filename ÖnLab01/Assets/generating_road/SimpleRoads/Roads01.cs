using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roads01 : MonoBehaviour {

    public List<GameObject> xRoad = new List<GameObject>();
    public GameObject XRoad
    {
        get
        {
            int i = (int)(Random.value * xRoad.Count);
            if (i == xRoad.Count) i--;
            GameObject ki = Instantiate(xRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            return ki;
        }
    }
    public GameObject YRoad {
        get {
            int i = (int) (Random.value * xRoad.Count);
            if (i == xRoad.Count) i--;
            GameObject ki = Instantiate(xRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(new Vector3(0, 90, 0));
            return ki;
        }
    }

    //----------------------------------------------------------------
    public List<GameObject> crossRoad = new List<GameObject>();
    public GameObject CrossRoad
    {
        get
        {
            int i = (int)(Random.value * crossRoad.Count);
            if (i == crossRoad.Count) i--;
            GameObject ki = Instantiate(crossRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            return ki;
        }
    }

    //------------------------------------------------------------------
    public List<GameObject> tRoad = new List<GameObject>();
    public GameObject TRoad07
    {
        get
        {
            int i = (int)(Random.value * tRoad.Count);
            if (i == tRoad.Count) i--;
            GameObject ki = Instantiate(tRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            return ki;
        }
    }
    public GameObject TRoad11
    {
        get
        {
            int i = (int)(Random.value * tRoad.Count);
            if (i == tRoad.Count) i--;
            GameObject ki = Instantiate(tRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(0, 180, 0);
            return ki;
        }
    }
    public GameObject TRoad13
    {
        get
        {
            int i = (int)(Random.value * tRoad.Count);
            if (i == tRoad.Count) i--;
            GameObject ki = Instantiate(tRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(0, 90, 0);
            return ki;
        }
    }
    public GameObject TRoad14
    {
        get
        {
            int i = (int)(Random.value * tRoad.Count);
            if (i == tRoad.Count) i--;
            GameObject ki = Instantiate(tRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(0, 270, 0);
            return ki;
        }
    }
    //---------------------------------------------------------
    public List<GameObject> lRoad = new List<GameObject>();
    public GameObject LRoad05
    {
        get
        {
            int i = (int)(Random.value * lRoad.Count);
            if (i == lRoad.Count) i--;
            GameObject ki = Instantiate(lRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            return ki;
        }
    }
    public GameObject LRoad09
    {
        get
        {
            int i = (int)(Random.value * lRoad.Count);
            if (i == lRoad.Count) i--;
            GameObject ki = Instantiate(lRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(0, 90, 0);
            return ki;
        }
    }
    public GameObject LRoad10
    {
        get
        {
            int i = (int)(Random.value * lRoad.Count);
            if (i == lRoad.Count) i--;
            GameObject ki = Instantiate(lRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(0, 180, 0);
            return ki;
        }
    }
    public GameObject LRoad06
    {
        get
        {
            int i = (int)(Random.value * lRoad.Count);
            if (i == lRoad.Count) i--;
            GameObject ki = Instantiate(lRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(0, 270, 0);
            return ki;
        }
    }
    //-----------------------------------------------
    public List<GameObject> stopRoad = new List<GameObject>();
    
    public GameObject SRoad01
    {
        get
        {
            int i = (int)(Random.value * stopRoad.Count);
            if (i == stopRoad.Count) i--;
            GameObject ki = Instantiate(stopRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(0, 180, 0);
            return ki;
        }
    }
    public GameObject SRoad02
    {
        get
        {
            int i = (int)(Random.value * stopRoad.Count);
            if (i == stopRoad.Count) i--;
            GameObject ki = Instantiate(stopRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(0, 0, 0);
            return ki;
        }
    }
    public GameObject SRoad08
    {
        get
        {
            int i = (int)(Random.value * stopRoad.Count);
            if (i == stopRoad.Count) i--;
            GameObject ki = Instantiate(stopRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(0, 270, 0);
            return ki;
        }
    }
    public GameObject SRoad04
    {
        get
        {
            int i = (int)(Random.value * stopRoad.Count);
            if (i == stopRoad.Count) i--;
            GameObject ki = Instantiate(stopRoad[i]);
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(0, 90, 0);
            return ki;
        }
    }
}
