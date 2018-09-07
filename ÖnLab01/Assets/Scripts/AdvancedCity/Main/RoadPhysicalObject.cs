using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RoadPhysicalObject : MonoBehaviour
{

    public GameObject sideRoadObject;
    List<Vector2> ControlPoints = new List<Vector2>();
    Mesh mesh;
    List<Vector3> meshVertexes = new List<Vector3>();
    List<List<int>> subTriangles;
    List<Vector2> myUV = new List<Vector2>();
    List<Material> materials;


    // Szelso es Kozepso pontok
    Vector3 S1, S2, K1, K2;
    public void CreateRoadMesh(Vector3 s1, Vector3 s2, Vector3 k1, Vector3 k2, int mat)
    {
        Vector2[] tomb =
        {
            new Vector2(0,0),
            new Vector2(1.0f,0)
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
        GenerateMesh(mat);
    }
    public void CreateCrossingMesh(List<Vector3> pointList, int mat)
    {
        subTriangles = new List<List<int>>();
        mesh = GetComponent<MeshFilter>().mesh;
        materials = new List<Material>();
        materials.AddRange(GetComponent<MeshRenderer>().materials);
        for (int i = 0; i < materials.Count; i++)
        {
            subTriangles.Add(new List<int>());
        }
        CrossingGenerator building = new CrossingGenerator(pointList, mat);
        foreach (Triangle triangle in building.getTriangles())
        {
            AddTriangle(triangle);
        }

    }

    bool AddTriangle(Vector3 A, Vector3 B, Vector3 C, int mat)
    {
        if (A == B || B == C || C == A) return false;
        float area = MyMath.Area(A, B, C);
        if (area < 0.0001f) return false;
        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(A);

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(B);

        subTriangles[mat].Add(meshVertexes.Count);
        meshVertexes.Add(C);
        return true;

    }

    private void AddTriangle(Triangle triangle)
    {
        if (subTriangles.Count <= triangle.material)
        {
            Debug.Log("Need material : " + triangle.material);
            return;
        }
        Matrix4x4 matrix = gameObject.transform.worldToLocalMatrix;
        Vector3 to = transform.position;
        if (triangle.uvs.Length < 3) return;
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.A - to));
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.B - to));
        subTriangles[triangle.material].Add(meshVertexes.Count);
        meshVertexes.Add(matrix * (triangle.C - to));
        myUV.Add(triangle.uvs[0]);
        myUV.Add(triangle.uvs[1]);
        myUV.Add(triangle.uvs[2]);
    }
    void GenerateMesh(int mat)
    {
        bool elore = true;
        if (K1.x < K2.x) elore = false;
        else if (K1.x == K2.x && K1.z < K2.z) elore = false;
        Vector3 irany1 = S1 - K1;
        Vector3 irany2 = S2 - K2;
        Vector3 fel = new Vector3(0, 1, 0);
        for (int i = 0; i < ControlPoints.Count - 1; i++)
        {
            Vector3 tmpK1 = K1 + irany1 * ControlPoints[i].x + fel * ControlPoints[i].y;
            Vector3 tmpK2 = K2 + irany2 * ControlPoints[i].x + fel * ControlPoints[i].y;
            Vector3 tmpS1 = K1 + irany1 * ControlPoints[i + 1].x + fel * ControlPoints[i + 1].y;
            Vector3 tmpS2 = K2 + irany2 * ControlPoints[i + 1].x + fel * ControlPoints[i + 1].y;
            float hoszK = (K1 - K2).magnitude * 0.5f;
            float hoszS = (S1 - S2).magnitude * 0.5f;

            bool egyik = AddTriangle(tmpS2, tmpS1, tmpK1, mat);
            bool masik = AddTriangle(tmpK1, tmpK2, tmpS2, mat);

            if (!elore)
            {
                if (egyik)
                {
                    myUV.Add(new Vector2(ControlPoints[i + 1].x, hoszS));
                    myUV.Add(new Vector2(ControlPoints[i + 1].x, 0));
                    myUV.Add(new Vector2(ControlPoints[i].x, 0));
                }

                if (masik)
                {
                    myUV.Add(new Vector2(ControlPoints[i].x, 0));
                    myUV.Add(new Vector2(ControlPoints[i].x, hoszK));
                    myUV.Add(new Vector2(ControlPoints[i + 1].x, hoszS));
                }
            }
            else
            {
                if (egyik)
                {
                    myUV.Add(new Vector2(ControlPoints[i + 1].x, 0));
                    myUV.Add(new Vector2(ControlPoints[i + 1].x, hoszS));
                    myUV.Add(new Vector2(ControlPoints[i].x, hoszK));
                }
                if (masik)
                {
                    myUV.Add(new Vector2(ControlPoints[i].x, hoszK));
                    myUV.Add(new Vector2(ControlPoints[i].x, 0));
                    myUV.Add(new Vector2(ControlPoints[i + 1].x, 0));
                }
            }
        }
    }
    public void CreateMesh()
    {
        if (meshVertexes == null) return;
        mesh.Clear();
        if (meshVertexes.Count < 3)
        {
            Destroy(this.gameObject, 0.1f);
            return;
        }
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
