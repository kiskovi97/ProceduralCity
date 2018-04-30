using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAssetStore : MonoBehaviour {

    public GameObject[] windows;
    public GameObject window
    {
        get
        {
            int i = (int) (Random.value * (windows.Length ));
            return windows[i];
        }
    }
    public GameObject[] walls;
    public GameObject wall
    {
        get
        {
            int i = (int)(Random.value * (walls.Length ));
            return walls[i];
        }
    }
}
