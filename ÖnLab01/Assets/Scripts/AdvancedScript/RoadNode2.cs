using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNode2 {
    RoadNode2 elozo = null;
    List<RoadNode2> szomszedok;
    public Vector3 position;
    
    // Use this for initialization
   
    public RoadNode2() {
        szomszedok = new List<RoadNode2>();
        position = new Vector3(0, 0, 0);
    }
    public void SetPosition(Vector3 pos)
    {
        
        position = pos;
        Debug.DrawLine(position, elozo.position, Color.yellow, 100, false);
    }

    List<Vector3> tovabb_irany = new List<Vector3>();
    void MakeIranyok()
    {
        Vector3 elozo_irany = new Vector3(0, 0, 1);
        if (elozo != null) elozo_irany =  elozo.position - position;

        int elagazasok = (int)(Random.value * 2 + 2);
        Debug.Log(elagazasok);
        for (int i = 1; i < elagazasok + 1; i++)
        {
            Vector3 uj = new Vector3();
            float Rotation = 3.14f * (2 * (-i / (elagazasok + 1.0f)));
            Rotation += -0.2f + Random.value * 0.4f;
            uj.Set(
            elozo_irany.x * Mathf.Cos(Rotation) - elozo_irany.z * Mathf.Sin(Rotation), elozo_irany.y * -1,
            elozo_irany.x * Mathf.Sin(Rotation) + elozo_irany.z * Mathf.Cos(Rotation));
            tovabb_irany.Add(uj.normalized);
        }


    }

    public void SetElozo(RoadNode2 setElozo)
    {
        elozo = setElozo;
        if (szomszedok.Count == 0)
            szomszedok.Add(elozo);
        else
            szomszedok[0] = setElozo;
        Debug.DrawLine(position, elozo.position, Color.red, 100, false);
    }
    public List<RoadNode2> GenerateRoads()
    {
        List<RoadNode2> ki = new List<RoadNode2>();
        if (Random.value <0.9f)
        {
            RoadNode2 ad = new RoadNode2();
            Vector3 irany = new Vector3(0, 0, 1);
            if (elozo != null) irany = position - elozo.position;
            ad.position = position + irany + new Vector3(Random.value * 0.2f - 0.1f, 0, 0);
            ad.SetElozo(this);
            ki.Add(ad);
        }
        else
        {
            MakeIranyok();
            foreach (Vector3 irany in tovabb_irany)
            {
                RoadNode2 ad = new RoadNode2();
                ad.position = position + irany*2;
                ad.SetElozo(this);
                ki.Add(ad);
            }
        }
        
        return ki;
    }
    
	
}
