using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNode {

    public Vector3 position;

    List<RoadNode> szomszedok;
    public List<RoadNode> Szomszedok
    {
        get
        {
            return szomszedok;
        }
    }

    bool sideroad = false;
    // Generating Variables
    float straightFreq = 0.9f;
    float rotationRandom = 0.2f;
    int maxelagazas = 4;
    float randomTurn = 0.2f;
    // Konstruktor
    public RoadNode(float freq, int max, float rotate)
    {
        szomszedok = new List<RoadNode>();
        position = new Vector3(0, 0, 0);
        straightFreq = freq;
        maxelagazas = max;
        rotationRandom = rotate;
    }

    // Grafban Kor kereseset segito fuggveny
    public RoadNode Kovetkezo(RoadNode elozo, bool jobbra)
    {
        if (szomszedok.Count < 1) return null;
        if (szomszedok.Count == 1) return szomszedok[0];
        if (!szomszedok.Contains(elozo)) {
            Debug.Log("Rosz elozo lett megadva");
            return null;
        }
        RoadNode ki = szomszedok[0];
        Vector3 ki_irany = (ki.position - position).normalized;
        Vector3 elozo_irany = (elozo.position - position).normalized;
        float angleNow;
        if (jobbra)
            angleNow = 360;
        else
            angleNow = -360;
        foreach (RoadNode road in szomszedok)
        {
            if (road == elozo) continue;
            Vector3 kovetkezo_irany = (road.position - position).normalized;
            float angleNew = Vector3.SignedAngle(elozo_irany, kovetkezo_irany,Vector3.up);
            if (jobbra)
            {
                if (angleNew < 0) angleNew += 360;
            }
            else
                 if (angleNew > 0) angleNew -= 360;

            
            if (angleNow > angleNew && jobbra)
            {
                ki = road;
                elozo_irany = elozo.position - position;
                angleNow = angleNew;
            }
            if (angleNow < angleNew && !jobbra)
            {
                ki = road;
                elozo_irany = elozo.position - position;
                angleNow = angleNew;
            }
        }
        if (ki == elozo) return null;
        return ki;
    }

    // Visualization function
    public void DrawLines(Color c)
    {
        foreach (RoadNode road in szomszedok)
        {
                Debug.DrawLine(position, road.position, c, 100, false);
        }
    }

    // Smooth function
    public void Smooth(float intensity)
    {
        if (szomszedok.Count < 2) return;
        Vector3 center = new Vector3(0, 0, 0);
        foreach (RoadNode szomszed in szomszedok) center += szomszed.position;
        center /= szomszedok.Count;
        Vector3 irany = center - position;
        position += irany * intensity;
    }

    // Deciosion where to go from here
    List<Vector3> tovabb_irany = new List<Vector3>();
    void MakeIranyok()
    {
        Vector3 elozo_irany = new Vector3(0, 0, 1);
        if (szomszedok.Count>0) elozo_irany = szomszedok[0].position - position;

        int elagazasok = (int)(Random.value * (maxelagazas-2) + 2);
        if (elagazasok < 2) elagazasok = 2;
        if (sideroad) elagazasok = 3;
        for (int i = 1; i < elagazasok + 1; i++)
        {
            Vector3 uj = new Vector3();
            float Rotation = 3.14f * (2 * (-i / (elagazasok + 1.0f)));
            Rotation += -randomTurn + Random.value * randomTurn*2;
            uj.Set(
            elozo_irany.x * Mathf.Cos(Rotation) - elozo_irany.z * Mathf.Sin(Rotation), elozo_irany.y * -1,
            elozo_irany.x * Mathf.Sin(Rotation) + elozo_irany.z * Mathf.Cos(Rotation));
            tovabb_irany.Add(uj.normalized);
        }


    }

     
    // First Side Roads Generating
    List<Vector3> side_irany = new List<Vector3>();
    void MakeSideIrany()
    {
        // Only straight roads can make sideroads
        if (szomszedok.Count != 2) return;
        Vector3 irany = szomszedok[0].position - szomszedok[1].position;
        Vector3 meroleges1 = new Vector3(irany.z, irany.y, irany.x * -1);
        Vector3 meroleges2 = new Vector3(irany.z * -1, irany.y, irany.x);
        
        side_irany.Add(meroleges1.normalized);
        side_irany.Add(meroleges2.normalized);
    }
    public List<RoadNode> GenerateSideRoads(float distance, float straightFreqS, float RotationRandomS)
    {
        List<RoadNode> ki = new List<RoadNode>();
        MakeSideIrany();
        foreach(Vector3 irany in side_irany)
        {
            RoadNode ad = new RoadNode(straightFreqS, maxelagazas, RotationRandomS);
            ad.position = position + irany * distance;
            ad.SetElozo(this);
            ad.sideroad = true;
            ki.Add(ad);
            szomszedok.Add(ad);
        }
        return ki;
    }
    
    // SImple Road Generation
    public List<RoadNode> GenerateRoads(float distance)
    {
        List<RoadNode> ki = new List<RoadNode>();
        if (Random.value < straightFreq)
        {
            GenerateStraight(ki, distance);
        }
        else
        {
            GenerateCrossing(ki, distance);
        }
        return ki;
    }
    void GenerateCrossing(List<RoadNode> ki, float distance)
    {
        MakeIranyok();
        foreach (Vector3 irany in tovabb_irany)
        {
            RoadNode ad = new RoadNode(straightFreq, maxelagazas, rotationRandom);
            ad.position = position + irany * distance;
            ad.SetElozo(this);
            ad.sideroad = sideroad;
            ki.Add(ad);
            szomszedok.Add(ad);
        }
    }
    void GenerateStraight(List<RoadNode> ki, float distance)
    {
        RoadNode ad = new RoadNode(straightFreq, maxelagazas, rotationRandom);
        Vector3 irany = new Vector3(0, 0, 1.5f);
        if (szomszedok.Count>0) irany = position - szomszedok[0].position;

        Vector3 random_irany = new Vector3();
        float Rotation = Random.value * rotationRandom * 2 - rotationRandom;
        random_irany.Set(
        irany.x * Mathf.Cos(Rotation) - irany.z * Mathf.Sin(Rotation), irany.y * -1,
        irany.x * Mathf.Sin(Rotation) + irany.z * Mathf.Cos(Rotation));

        ad.position = position + random_irany.normalized * distance;
        ad.SetElozo(this);
        ad.sideroad = sideroad;
        ki.Add(ad);
        szomszedok.Add(ad);
    }

    // Road Generating Helper functions
    public void SetElozo(RoadNode setElozo)
    {
        if (szomszedok.Count == 0)
            szomszedok.Add(setElozo);
        else
            szomszedok[0] = setElozo;
    }

    // Merge helper functions
    public void addSzomszed(RoadNode be)
    {
        szomszedok.Add(be);
    }
    public void removeSzomszed(RoadNode ki)
    {
        szomszedok.Remove(ki);
    }
    public RoadNode getElozo()
    {
        return szomszedok[0];
    }
    public void Csere(RoadNode uj, RoadNode regi)
    {
        int index = szomszedok.IndexOf(regi);
        if (index > szomszedok.Count || index < 0)
        {
            Debug.Log("CsereHiba");
            return;
        }
        szomszedok[index] = uj;
    }

}
