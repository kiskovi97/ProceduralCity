using System.Collections.Generic;
using UnityEngine;

class BuildingGenerator : GeneratorImpl
{
    CollisionBoxGenerator box = null;
    BuildingGenerator next = null;
    public BuildingGenerator(Vector3[] controlpoints, float floorSize, int floorCount, int more = 3, bool onGround = true)
    {
        if (floorCount < 1) return;
        for (int i = 0; i < controlpoints.Length; i++)
        { 
            int prev = i - 1;
            if (prev < 0) prev = controlpoints.Length - 1;
            FrontWallGenerator frontWall = new FrontWallGenerator(controlpoints[prev], controlpoints[i], floorSize, floorCount, onGround);
            generators.Add(frontWall);
        }
        float height = floorCount * floorSize;
        if (onGround)
        {
            height += floorSize * 1.5f;
        }
        MakeRoof(controlpoints, new Vector3(0, height, 0), (more <= 0 || (int)(floorCount * 0.7f) < 1));
        box = new CollisionBoxGenerator(controlpoints, height);
        if (more > 0)
        {
            Vector3[] newControlPoints = ControlChange(controlpoints);
            for (int i = 0; i < newControlPoints.Length; i++)
            {
                newControlPoints[i].y += height;
            }
            next = new BuildingGenerator(newControlPoints, floorSize, (int)(floorCount * 0.7f), more - 1, false);
            generators.Add(next);
        }
    }

    public Triangle[] GetCollision()
    {
        List<Triangle> triangles = new List<Triangle>();
        if (box != null) triangles.AddRange(box.getTriangles());
        if (next != null) triangles.AddRange(next.GetCollision());
        return triangles.ToArray();
    }

    private Vector3[] ControlChange(Vector3[] kontrolpoints)
    {
        List<Vector3> newControlPoints = new List<Vector3>(kontrolpoints);
        int indexStart = (int)(Random.value * newControlPoints.Count);
        int index2 = indexStart + 1;
        int index3 = indexStart + 2;
        int index4 = indexStart + 3;
        if (index4 >= newControlPoints.Count)
        {
            index4 = index4 - newControlPoints.Count;
            if (index3 >= newControlPoints.Count)
            {
                index3 = index3 - newControlPoints.Count;
                if (index2 >= newControlPoints.Count)
                {
                    index2 = index2 - newControlPoints.Count;
                }
            }
        }
        if (Random.value > 0.5f)
        {
            if (Random.value > 0.2f)
            {
                for (int i = 0; i < newControlPoints.Count; i++)
                {
                    int before = i - 1;
                    int next = i + 1;
                    if (before < 0) before = newControlPoints.Count - 1;
                    if (next > newControlPoints.Count - 1) next = 0;
                    if (Vector3.SignedAngle(newControlPoints[i] - newControlPoints[before], newControlPoints[i] - newControlPoints[next], new Vector3(0, 1, 0)) < 0)
                    {
                        newControlPoints[i] = (newControlPoints[i] * 3 + newControlPoints[before]) / 4;
                        newControlPoints[i] = (newControlPoints[i] * 3 + newControlPoints[next]) / 4;
                    }
                }
            } else if (newControlPoints.Count > 4)
            {
                if (Vector3.SignedAngle(newControlPoints[index2] - newControlPoints[indexStart], newControlPoints[index2] - newControlPoints[index3], new Vector3(0, 1, 0)) < 0)
                {
                    newControlPoints.RemoveAt(index2);
                }
            } 
            
        } else
        {
            Vector3 original = (newControlPoints[index2] + newControlPoints[index3]) / 2;
            if (Vector3.SignedAngle(newControlPoints[index2] - newControlPoints[indexStart], newControlPoints[index2] - newControlPoints[index3], new Vector3(0, 1, 0)) < 0)
            {
                newControlPoints[index2] = (newControlPoints[index2] * 3 + newControlPoints[indexStart]) / 4;
            }
            if (Vector3.SignedAngle(newControlPoints[index3] - newControlPoints[index2], newControlPoints[index3] - newControlPoints[index4], new Vector3(0, 1, 0)) < 0)
            {
                newControlPoints[index3] = (newControlPoints[index3] * 3 + newControlPoints[index4]) / 4;
            }
            newControlPoints.Insert(index3, original);
        } 
        return newControlPoints.ToArray();
    }

    private void MakeRoof(Vector3[] roofPoints, Vector3 up, bool last)
    {
        Vector3[] controlPoints = (Vector3[])roofPoints.Clone();
        for (int i = 0; i < controlPoints.Length; i++)
        {
            controlPoints[i] += up;
        }
        meshElements.Add(new RoofMesh(controlPoints, last));
    }
}