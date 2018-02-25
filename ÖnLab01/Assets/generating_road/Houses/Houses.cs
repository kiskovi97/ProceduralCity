using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Houses : MonoBehaviour {

    public List<GameObject> alapHosues = new List<GameObject>();

    public GameObject AlapHosues01
    {
        get
        {
            int i = (int)(Random.value * alapHosues.Count);
            if (i == alapHosues.Count) i--;
            GameObject ki = alapHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 90, 0);
            return Instantiate(ki);
        }
    }
    public GameObject AlapHosues02
    {
        get
        {
            int i = (int)(Random.value * alapHosues.Count);
            if (i == alapHosues.Count) i--;
            GameObject ki = alapHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 270, 0);
            return Instantiate(ki);
        }
    }
    public GameObject AlapHosues04
    {
        get
        {
            int i = (int)(Random.value * alapHosues.Count);
            if (i == alapHosues.Count) i--;
            GameObject ki = alapHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 0, 0);
            return Instantiate(ki);
        }
    }
    public GameObject AlapHosues08
    {
        get
        {
            int i = (int)(Random.value * alapHosues.Count);
            if (i == alapHosues.Count) i--;
            GameObject ki = alapHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 180, 0);
            return Instantiate(ki);
        }
    }

    public List<GameObject> lalapHosues = new List<GameObject>();

    public GameObject LAlapHosues05
    {
        get
        {
            int i = (int)(Random.value * lalapHosues.Count);
            if (i == lalapHosues.Count) i--;
            GameObject ki = lalapHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 0, 0);
            return Instantiate(ki);
        }
    }
    public GameObject LAlapHosues09
    {
        get
        {
            int i = (int)(Random.value * lalapHosues.Count);
            if (i == lalapHosues.Count) i--;
            GameObject ki = lalapHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 90, 0);
            return Instantiate(ki);
        }
    }
    public GameObject LAlapHosues10
    {
        get
        {
            int i = (int)(Random.value * lalapHosues.Count);
            if (i == lalapHosues.Count) i--;
            GameObject ki = lalapHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 180, 0);
            return Instantiate(ki);
        }
    }
    public GameObject LAlapHosues06
    {
        get
        {
            int i = (int)(Random.value * lalapHosues.Count);
            if (i == lalapHosues.Count) i--;
            GameObject ki = lalapHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 270, 0);
            return Instantiate(ki);
        }
    }

    public List<GameObject> tetoHosues = new List<GameObject>();

    public GameObject TetoHosues05
    {
        get
        {
            int i = (int)(Random.value * tetoHosues.Count);
            if (i == tetoHosues.Count) i--;
            GameObject ki = tetoHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 0, 0);
            return Instantiate(ki);
        }
    }

    public List<GameObject> semmiHosues = new List<GameObject>();

    public GameObject SemmiHouse
    {
        get
        {
            int i = (int)(Random.value * semmiHosues.Count);
            if (i == semmiHosues.Count) i--;
            GameObject ki = semmiHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 0, 0);
            return Instantiate(ki);
        }
    }

    public List<GameObject> xHosues = new List<GameObject>();
    public GameObject XHouse
    {
        get
        {
            int i = (int)(Random.value * xHosues.Count);
            if (i == xHosues.Count) i--;
            GameObject ki = xHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 0, 0);
            return Instantiate(ki);
        }
    }
    public GameObject YHouse
    {
        get
        {
            int i = (int)(Random.value * xHosues.Count);
            if (i == xHosues.Count) i--;
            GameObject ki = xHosues[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 90, 0);
            return Instantiate(ki);
        }
    }

    public List<GameObject> thouse = new List<GameObject>();
    public GameObject THouse07
    {
        get
        {
            int i = (int)(Random.value * thouse.Count);
            if (i == thouse.Count) i--;
            GameObject ki = thouse[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);

            ki.transform.Rotate(-90, 0, 0);
            return Instantiate(ki);
        }
    }
    public GameObject THouse11
    {
        get
        {
            int i = (int)(Random.value * thouse.Count);
            if (i == thouse.Count) i--;
            GameObject ki = thouse[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 180, 0);
            return Instantiate(ki);
        }
    }
    public GameObject THouse13
    {
        get
        {
            int i = (int)(Random.value * thouse.Count);
            if (i == thouse.Count) i--;
            GameObject ki = thouse[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 90, 0);
            return Instantiate(ki);
        }
    }
    public GameObject THouse14
    {
        get
        {
            int i = (int)(Random.value * thouse.Count);
            if (i == thouse.Count) i--;
            GameObject ki = thouse[i];
            ki.transform.rotation = new Quaternion(0, 0, 0, 0);
            ki.transform.Rotate(-90, 270, 0);
            return Instantiate(ki);
        }
    }


}
