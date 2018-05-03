using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {
    
    protected bool Xplus = true;
    protected bool Xminus = true;
    protected bool Zplus = true;
    protected bool Zminus = true;
    public float timeing = 0.02f;
    public float simplefactor = 1.0f;
    public GameObject simpleRoad;
    public float crossfactor = 0.5f;
    public GameObject crossRoad;
    public float curvefactor = 0.2f;
    public GameObject curveRoad;
    public GameObject map;
    private float merce;
    private MapScript m;
    private Generate rekurzio = null;
    private int childs=0;
    private bool novekedett;
    //inicializalasra hasznalhato
    public void SetBools(bool _Xplus, bool _Xminus, bool _Zplus, bool _Zminus)
    {
        Xplus = _Xplus;
        Xminus = _Xminus;
        Zplus = _Zplus;
        Zminus = _Zminus;
    }
	
    // Letrejottekor fut le
	void Start () {
        m = map.GetComponent<MapScript>();
        merce = m.merce;
        Invoke("fullGenerate", timeing);
	}
    // Ut novekedese egy egyseggel, vagy rekurzivan visszafejteni, ha zsakutca van
    void fullGenerate()
    {
        int i = (int) (transform.position.x / merce);
        int j = (int) (transform.position.z / merce);
        if (i + 2 >= m.size && Xplus)  {
            Xplus = false;  Zplus = true;  Zminus = true;
        }
        if (i - 2 < 0 && Xminus)  {
            Xminus = false;  Zplus = true; Zminus = true;
        }
        if (j + 2 >= m.size && Zplus) {
            Zplus = false; Xplus = true;  Xminus = true;
        }
        if (j - 2 < 0 && Zminus) {
            Zminus = false;  Xplus = true;  Xminus = true;
        }

        if (i + 1 >= m.size || j - 1 < 0 || i - 1 < 0 || j + 1 >= m.size) {
            {
                RekurzioFgv();
                return;
            }
        }
        
        novekedett = false;
        m.db++;
        if (Xplus) if (!m.tomb[i + 1, j]) { novekedett = true; GenerateXplus(); m.tomb[i + 1, j] = true; if (novekedett) childs++; }
        novekedett = false;
        if (Zplus) if (!m.tomb[i, j + 1]) { novekedett = true; GenerateZplus(); m.tomb[i, j+1] = true; if (novekedett) childs++; }
        novekedett = false;
        if (Xminus) if (!m.tomb[i - 1, j]) { novekedett = true; GenerateXminus(); m.tomb[i-1, j] = true; if (novekedett) childs++; }
        novekedett = false;
        if (Zminus) if (!m.tomb[i, j - 1]) { novekedett = true; GenerateZminus(); m.tomb[i, j-1] = true; if (novekedett) childs++; }
        if (childs == 0)
        {
            RekurzioFgv();
            return;
        }
    }


    private GameObject go;
    void  GenerateXplus()
    {

        int i = (int)(transform.position.x / merce);
        int j = (int)(transform.position.z / merce);
        if (Random.value < simplefactor)
        {
            if (Random.value < curvefactor && !m.tomb[i+1,j+1])
            {
                go = Instantiate(curveRoad);
                go.GetComponent<Generate>().SetBools(false, true, true, false);
            }
            else
            if (Random.value < curvefactor && !m.tomb[i + 1, j - 1])
            {
                go = Instantiate(curveRoad);
                go.GetComponent<Generate>().SetBools(false, true, false, true);
            }
            else
            {
                if (m.tomb[i + 1, j + 1] || m.tomb[i + 1, j - 1])
                {
                    novekedett = false;
                    return;
                }
                MakeXRoad();
            }
            
        }
        else if (Random.value < crossfactor)
        {
            MakeCross();
        }
        else
        {
            novekedett = false;
            return;
        }
        go.GetComponent<Generate>().Xminus = false;
        go.transform.position = transform.position + new Vector3(merce, 0, 0);
        Reduce();
    }

    void GenerateZplus()
    {
        int i = (int)(transform.position.x / merce);
        int j = (int)(transform.position.z / merce);
        if (Random.value < simplefactor)
        {
            if (Random.value < curvefactor && !m.tomb[i - 1, j + 1])
            {
                go = Instantiate(curveRoad);
                go.GetComponent<Generate>().SetBools(false, true, false, true);
            }
            else
             if (Random.value < curvefactor && !m.tomb[i + 1, j + 1])
            {
                go = Instantiate(curveRoad);
                go.GetComponent<Generate>().SetBools(true, false, false, true);
            }
            else
            {
                if (m.tomb[i - 1, j + 1] || m.tomb[i + 1, j + 1])
                {
                    novekedett = false;
                    return;
                }
                MakeYRoad();
               
            }
        }
        else if (Random.value < crossfactor) MakeCross();
        else
        {
            
            novekedett = false;
            return;
        }

        go.GetComponent<Generate>().Zminus = false;
        go.transform.position = transform.position + new Vector3(0, 0, merce);
        Reduce();
    }

    void GenerateXminus()
    {
        int i = (int)(transform.position.x / merce);
        int j = (int)(transform.position.z / merce);
        if (Random.value < simplefactor) {
            if (Random.value < curvefactor && !m.tomb[i - 1, j - 1])
            {
                go = Instantiate(curveRoad);
                go.GetComponent<Generate>().SetBools(true, false, false, true);
            }
            else
            if (Random.value < curvefactor && !m.tomb[i - 1, j + 1])
            {
                go = Instantiate(curveRoad);
                go.GetComponent<Generate>().SetBools(true, false, true, false);
            }
            else
            {
                if (m.tomb[i - 1, j + 1] ||
                m.tomb[i - 1, j - 1])
                {
                   
                    novekedett = false;
                    return;
                }
                MakeXRoad();
               
            }
        }
        else if (Random.value < crossfactor) MakeCross();
        else
        {

            novekedett = false;
            return;
        }
        go.GetComponent<Generate>().Xplus = false;
        go.transform.position = transform.position + new Vector3(-1*merce, 0, 0);
        Reduce();
    }

    void GenerateZminus()
    {
        int i = (int)(transform.position.x / merce);
        int j = (int)(transform.position.z / merce);
        if (Random.value < simplefactor)
        {
            if (Random.value < curvefactor && !m.tomb[i - 1, j - 1])
            {
                go = Instantiate(curveRoad);
                go.GetComponent<Generate>().SetBools(false, true, true, false);
            }
            else
             if (Random.value < curvefactor && !m.tomb[i + 1, j - 1])
            {
                go = Instantiate(curveRoad);
                go.GetComponent<Generate>().SetBools(true, false, true, false);
            }
            else
            {
                if (m.tomb[i - 1, j - 1] ||
                 m.tomb[i + 1, j - 1])
                {
                    
                    novekedett = false;
                    return;
                }
                MakeYRoad();

            }

        }
        else if (Random.value < crossfactor) MakeCross();
        else
        {
            novekedett = false;
            return;
        }
        go.transform.position = transform.position + new Vector3(0, 0, -1*merce);
        go.GetComponent<Generate>().Zplus = false;
        Reduce();
    }

    void Reduce()
    {
        go.transform.localScale = transform.localScale;
        Generate g = go.GetComponent<Generate>();
        g.simplefactor = simplefactor - m.lineMinus;
        g.crossfactor = crossfactor;
        g.curvefactor = curvefactor;
        g.map = map;
        g.rekurzio = this.GetComponent<Generate>();
        
    }
    void MakeCross()
    {
        simplefactor += map.GetComponent<MapScript>().crossPlus;
        go = Instantiate(crossRoad);
        go.GetComponent<Generate>().SetBools(true, true, true, true);
    }
    void MakeXRoad()
    {
        go = Instantiate(simpleRoad);
        go.GetComponent<Generate>().SetBools(true, true, false, false);
        go.transform.rotation = new Quaternion(0, 0, 0, 0);
        go.transform.Rotate(0, 90, 0);

    }
    void MakeYRoad()
    {
        go = Instantiate(simpleRoad);
        go.GetComponent<Generate>().SetBools(false, false, true, true);
    }

    public void RekurzioFgv()
    {
        if (childs > 1) childs--;
        else
        {
            if (rekurzio != null)
                rekurzio.RekurzioFgv();
            else
                MapErtesitese();
            Vege();
        }
        
    }
    public void MapErtesitese()
    {
        m.Vegeztem();
    }
    void Vege()
    {
        Destroy(this.gameObject,1);
    }

}


