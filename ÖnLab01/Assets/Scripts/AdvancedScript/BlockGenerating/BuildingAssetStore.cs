using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAssetStore : MonoBehaviour {

    public GameObject[] windows;
    public GameObject window
    {
        get
        {
            int i = (int) (Random.value * (windows.Length - 1));
            return windows[i];
        }
    }
}
