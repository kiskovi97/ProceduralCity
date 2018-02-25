using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour {
    public float crossPlus = 2;
    public float lineMinus = 0.2f;
    public int size = 1000;
    public bool[,] tomb;
    public int db = 0;
    public GameObject rootObjektum;
    public Roads01 roads;
    public Houses houses;

    public float merce = 2;

	// Use this for initialization
	void Start () {
        tomb = new bool[size, size];
        if (tomb == null) Debug.Log("Nem sikerult");
        for (int i=0; i<size; i++)
        {
            for (int j = 0; j<size; j++)
            {
                tomb[i,j] = false;
            }
        }
        Invoke("GenerateObjects",1);
        
	}

    void GenerateObjects()
    {
        if (tomb == null) Debug.Log("Nincs tomb");
        int tavolsag = size / 3;
        for (int i = 1; i < 2; i++)
        {
            for (int j = 1; j < 2; j++)
            {
                GameObject go = Instantiate(rootObjektum);
                go.transform.position = new Vector3(tavolsag * merce * i, 0, tavolsag * merce * j);
                go.GetComponent<Generate>().map = this.gameObject;
                tomb[tavolsag * i, tavolsag * j] = true;
            }
        }
    }
    public void Vegeztem()
    {
        for (int i = 1; i < size - 1; i++)
        {
            for (int j = 1; j < size - 1; j++)
            {
                if (tomb[i, j] && tomb[i + 1, j] && tomb[i, j + 1] && tomb[i + 1, j + 1])
                {

                    tomb[i + 1, j + 1] = false;
                }
            }
        }



        for (int i = 1; i < size-1; i++)
        {
            for (int j = 1; j < size-1; j++)
            {
                if (!tomb[i, j])
                {
                     
                    GameObject house = null;
                    int type = (tomb[i, j + 1] ? 1 : 0) + (tomb[i, j - 1] ? 2 : 0) + (tomb[i - 1, j] ? 4 : 0) + (tomb[i + 1, j] ? 8 : 0);
                    switch (type)
                    {
                        case 0:
                            house = houses.SemmiHouse;
                            break;
                        case 1:
                            house = houses.AlapHosues01;
                            break;
                        case 2:
                            house = houses.AlapHosues02;
                            break;
                        case 3:
                            house = houses.YHouse;
                            break;

                        case 4:
                            house = houses.AlapHosues04;
                            break;
                        case 5:
                            house = houses.LAlapHosues05;
                            break;
                        case 6:
                            house = houses.LAlapHosues06;
                            break;
                        case 7:
                            house = houses.THouse07;
                            break;
                        case 8:
                            house = houses.AlapHosues08;
                            break;
                        case 9:
                            house = houses.LAlapHosues09;
                            break;
                        case 10:
                            house = houses.LAlapHosues10;
                            break;
                        case 11:
                            house = houses.THouse11;
                            break;
                        case 12:
                            house = houses.XHouse;
                            break;
                        case 13:
                            house = houses.THouse13;
                            break;
                        case 14:
                            house = houses.THouse14;
                            break;
                        case 15:
                            house = houses.SemmiHouse;
                            break;
                    }
                    if (house!=null)
                    {
                        house.transform.position = new Vector3(merce * i, 0.0f, merce * j);
                        house.transform.localScale *= merce / 2.0f;
                        HouseGrow h = house.GetComponent<HouseGrow>();
                        if (h != null)
                        {
                            h.merce = merce;
                        }
                    }
                    
                        
                        
                } else
                {
                    GameObject road = null;
                    int type = (tomb[i, j + 1] ? 1 : 0) + (tomb[i, j - 1] ? 2 : 0) + (tomb[i - 1, j] ? 4 : 0) + (tomb[i + 1, j ] ? 8 : 0);
                    switch (type)
                    {
                        case 1:
                            road = roads.SRoad01;
                            break;
                        case 2:
                            road = roads.SRoad02;
                            break;
                        case 3:
                            road = roads.XRoad;
                            break;
                        case 4:
                            road = roads.SRoad04;
                            break;

                        case 5:
                            road = roads.LRoad05;
                            break;
                        case 6:
                            road = roads.LRoad06;
                            break;
                        case 7:
                            road = roads.TRoad07;
                            break;
                        case 8:
                            road = roads.SRoad08;
                            break;
                        case 9:
                            road = roads.LRoad09;
                            break;
                        case 10:
                            road = roads.LRoad10;
                            break;
                        case 11:
                            road = roads.TRoad11;
                            break;
                        case 12:
                            road = roads.YRoad;
                            break;
                        case 13:
                            road = roads.TRoad13;
                            break;
                        case 14:
                            road = roads.TRoad14;
                            break;
                        case 15:
                            road = roads.CrossRoad;
                            break;
                        
                        default:
                            road = null;
                            break;
                    }


                    if (road != null)
                    {
                        road.transform.position = new Vector3(merce * i, 0, merce * j);
                        road.transform.localScale *= merce / 2.0f;
                    }
                    
                }
            }
        }
    }

}
