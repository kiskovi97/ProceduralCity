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
        Vector3[] newControlPoints = (Vector3[])kontrolpoints.Clone();
        int indexStart = (int)(Random.value * newControlPoints.Length);
        int index2 = indexStart + 1;
        int index3 = indexStart + 2;
        int index4 = indexStart + 3;
        if (index4 >= newControlPoints.Length)
        {
            index4 = index4 - newControlPoints.Length;
            if (index3 >= newControlPoints.Length)
            {
                index3 = index3 - newControlPoints.Length;
                if (index2 >= newControlPoints.Length)
                {
                    index2 = index2 - newControlPoints.Length;
                }
            }
        }
        if (Vector3.SignedAngle(newControlPoints[index2] - newControlPoints[indexStart], newControlPoints[index2] - newControlPoints[index3], new Vector3(0, 1, 0)) < 0)
        {
            newControlPoints[index2] = (newControlPoints[index2] * 3 + newControlPoints[indexStart]) / 4;
        }
        if (Vector3.SignedAngle(newControlPoints[index3] - newControlPoints[index2], newControlPoints[index3] - newControlPoints[index4], new Vector3(0, 1, 0)) < 0)
        {
            newControlPoints[index3] = (newControlPoints[index3] * 3 + newControlPoints[index4]) / 4;
        }
        return newControlPoints;
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