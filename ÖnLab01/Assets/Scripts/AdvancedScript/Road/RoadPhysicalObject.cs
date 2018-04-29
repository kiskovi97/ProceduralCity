using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RoadPhysicalObject : MonoBehaviour {

    public GameObject sideRoadObject;
    List<Vector2> ControlPoints = new List<Vector2>();
    Mesh mesh;
    List<Vector3> meshVertexes = new List<Vector3>();
    List<List<int>> subTriangles;
    List<Vector2> myUV = new List<Vector2>();
    List<Material> materials;

    // Szelso es Kozepso pontok
    Vector3 S1, S2, K1, K2;

    public void GenerateBlockMesh(Vector3 s1, Vector3 s2, Vector3 k1, Vector3 k2)
    {
        Vector2[] tomb =
        {
            new Vector2(0,0),
            new Vector2(0.8f,0),
            new Vector2(0.8f,0.05f),
            new Vector2(1.0f,0.05f)
        };
        ControlPoints.AddRange(tomb);
        subTriangles = new List<List<int>>();
        mesh = GetComponent<MeshFilter>().mesh;
        materials = new List<Material>();
        materials.AddRange(GetComponent<MeshRenderer>().materials);
        for (int i = 0; i < materials.Count; i++)
        {
            subTriangles.Add(new List<int>());
        }
        S1 = s1;
        S2 = s2;
        K1 = k1;
        K2 = k2;

        
    
        GenerateMesh();
    }
    void AddTriangle(Vector3 A, Vector3 B, Vector3 C, int mat)
    {

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(A);

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(B);

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(C);

    }
    void GenerateMesh()
    {
        bool elore = true;
        if (K1.x < K2.x) elore = false;
        else if (K1.x == K2.x && K1.z < K2.z) elore = false;
        Vector3 irany1 = S1 - K1;
        Vector3 irany2 = S2 - K2;
        Vector3 fel = new Vector3(0, 1, 0);
        for (int i=0; i<ControlPoints.Count-1; i++)
        {

            

            Vector3 tmpK1 = K1 + irany1 * ControlPoints[i].x + fel * ControlPoints[i].y;
            Vector3 tmpK2 = K2 + irany2 * ControlPoints[i].x + fel * ControlPoints[i].y;
            Vector3 tmpS1 = K1 + irany1 * ControlPoints[i + 1].x + fel * ControlPoints[i + 1].y;
            Vector3 tmpS2 = K2 + irany2 * ControlPoints[i + 1].x + fel * ControlPoints[i + 1].y;

            if (i == 0)
            {
                GameObject obj = Instantiate(sideRoadObject, (tmpS1 + tmpS2) / 2, new Quaternion(0, 0, 0, 0));
                obj.transform.localScale-=new Vector3(0.5f, 0.5f, 0.5f);
                Vector3 irany = ((tmpK1 - tmpS1) + (tmpK2 - tmpS2)) / 2;
                obj.transform.rotation = Quaternion.LookRotation(irany);
            }
            

            float hoszK = (K1-K2).magnitude*2;
            float hoszS = (S1-S2).magnitude*2;
            
            AddTriangle(tmpS2, tmpS1,  tmpK1,  0);
            AddTriangle(tmpK1, tmpK2, tmpS2,  0);

            if (!elore)
            {
                myUV.Add(new Vector2(0.5f - ControlPoints[i + 1].x * 0.5f, hoszS));
                myUV.Add(new Vector2(0.5f - ControlPoints[i + 1].x * 0.5f, 0));
                myUV.Add(new Vector2(0.5f - ControlPoints[i].x * 0.5f, 0));

                myUV.Add(new Vector2(0.5f - ControlPoints[i].x * 0.5f, 0));
                myUV.Add(new Vector2(0.5f - ControlPoints[i].x * 0.5f, hoszK));
                myUV.Add(new Vector2(0.5f - ControlPoints[i + 1].x * 0.5f, hoszS));
            }
            else
            {
                myUV.Add(new Vector2(0.5f - ControlPoints[i + 1].x * 0.5f, 0));
                myUV.Add(new Vector2(0.5f - ControlPoints[i + 1].x * 0.5f, hoszS));
                myUV.Add(new Vector2(0.5f - ControlPoints[i].x * 0.5f, hoszK));

                myUV.Add(new Vector2(0.5f - ControlPoints[i].x * 0.5f, hoszK));
                myUV.Add(new Vector2(0.5f - ControlPoints[i].x * 0.5f, 0));
                myUV.Add(new Vector2(0.5f - ControlPoints[i + 1].x * 0.5f, 0));
            }
           
        }

        
    }
    public void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = meshVertexes.ToArray();
        mesh.subMeshCount = subTriangles.Count;
        for (int i = 0; i < subTriangles.Count; i++)
        {
            mesh.SetTriangles(subTriangles[i].ToArray(), i);
        }
        mesh.SetUVs(0, myUV);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
