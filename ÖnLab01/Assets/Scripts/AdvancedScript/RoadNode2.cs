using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNode2 {
    RoadNode2 elozo = null;
    List<RoadNode2> szomszedok;
    public Vector3 position;
    public float straightFreq = 0.9f;
    public int maxelagazas = 4;
    public float RotationRandom = 0.2f;

    // Use this for initialization

    public RoadNode2(float freq, int max, float rotate) {
        szomszedok = new List<RoadNode2>();
        position = new Vector3(0, 0, 0);
        straightFreq = freq;
        maxelagazas = max;
        RotationRandom = rotate;
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

        int elagazasok = (int)(Random.value * (maxelagazas-2) + 2);
        if (elagazasok < 2) elagazasok = 2;
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
        if (Random.value < straightFreq)
        {
            GenerateStraight(ki);
        }
        else
        {
            GenerateCrossing(ki);
        }
        return ki;
    }

    void GenerateCrossing(List<RoadNode2> ki)
    {
        Debug.Log("Elagazas");
        MakeIranyok();
        Debug.Log(tovabb_irany.Count);
        foreach (Vector3 irany in tovabb_irany)
        {
            RoadNode2 ad = new RoadNode2(straightFreq, maxelagazas, RotationRandom);
            ad.position = position + irany * 2;
            ad.SetElozo(this);
            ki.Add(ad);
        }
    }

    void GenerateStraight(List<RoadNode2> ki)
    {
        RoadNode2 ad = new RoadNode2(straightFreq, maxelagazas, RotationRandom);
        Vector3 irany = new Vector3(0, 0, 1.5f);
        if (elozo != null) irany = position - elozo.position;

        Vector3 random_irany = new Vector3();
        float Rotation = Random.value * RotationRandom *2 - RotationRandom;
        random_irany.Set(
        irany.x * Mathf.Cos(Rotation) - irany.z * Mathf.Sin(Rotation), irany.y * -1,
        irany.x * Mathf.Sin(Rotation) + irany.z * Mathf.Cos(Rotation));

        ad.position = position + random_irany;
        ad.SetElozo(this);
        ki.Add(ad);
    }
    
	
}
