using UnityEngine;

public class BuildingContainer : MonoBehaviour {

    [SerializeField]
    private GameObject[] buildings;
	public GameObject Building{
        get {
            int i = (int)(Random.value * (buildings.Length));
            return Instantiate(buildings[i]);
        }
    }
    public GameObject BuildingBySize(float size)
    {
        if (size > 1) size = 0;
        if (size < 0) size = 0;
        int i = (int)(size * (buildings.Length));
        return Instantiate(buildings[i]);
    }
    [SerializeField]
    private GameObject[] greenPlace;
    public GameObject Place
    {
        get
        {
            int i = (int)(Random.value * (greenPlace.Length));
            return Instantiate(greenPlace[i]);
        }
    }
}
