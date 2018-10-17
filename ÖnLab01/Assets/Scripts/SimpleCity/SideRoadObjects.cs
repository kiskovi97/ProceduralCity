using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideRoadObjects : MonoBehaviour
{

    public List<GameObject> padok = new List<GameObject>();

    public GameObject PadX
    {
        get
        {
            if (padok.Count == 0) return new GameObject();
            int i = (int)(Random.value * padok.Count);
            if (i == padok.Count) i--;
            GameObject ki = Instantiate(padok[i]);
            return ki;
        }
    }

    public List<GameObject> lampa = new List<GameObject>();

    public GameObject LampaX
    {
        get
        {
            if (lampa.Count == 0) return new GameObject();
            int i = (int)(Random.value * lampa.Count);
            if (i == lampa.Count) i--;
            GameObject ki = Instantiate(lampa[i]);
            return ki;
        }
    }
}
