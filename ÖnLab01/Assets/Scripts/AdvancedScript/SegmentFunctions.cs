﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentFunctions  {
    class Point
    {
        public Point(float a, float b)
        {
            x = a;
            y = b;
        }
        public float x;
        public float y;
    };

    static float max(float x, float y)
    {
        return (x > y) ? x : y;
    }
    static float min(float x, float y)
    {
        return (x < y) ? x : y;
    }

    // Given three colinear points p, q, r, the function checks if
    // point q lies on line segment 'pr'
    static bool onSegment(Point p, Point q, Point r)
    {
        if (q.x <= max(p.x, r.x) && q.x >= min(p.x, r.x) &&
            q.y <= max(p.y, r.y) && q.y >= min(p.y, r.y))
            return true;

        return false;
    }

    // To find orientation of ordered triplet (p, q, r).
    // The function returns following values
    // 0 --> p, q and r are colinear
    // 1 --> Clockwise
    // 2 --> Counterclockwise
    static int orientation(Point p, Point q, Point r)
    {
        // for details of below formula.
        float val = (q.y - p.y) * (r.x - q.x) -
                  (q.x - p.x) * (r.y - q.y);

        if (val == 0) return 0;  // colinear

        return (val > 0) ? 1 : -1; // clock or counterclock wise
    }

    // The main function that returns true if line segment 'p1q1'
    // and 'p2q2' intersect.
    static bool doIntersect(Point p1, Point q1, Point p2, Point q2)
    {
        // Find the four orientations needed for general and
        // special cases
        int o1 = orientation(p1, q1, p2);
        int o2 = orientation(p1, q1, q2);
        int o3 = orientation(p2, q2, p1);
        int o4 = orientation(p2, q2, q1);

        // General case
        if (o1 != o2 && o3 != o4)
            return true;
        
        return false; // Doesn't fall in any of the above cases
    }

    public static bool doIntersect(Vector3 pv1, Vector3 qv1, Vector3 pv2, Vector3 qv2)
    {
        Point p1 = new Point(pv1.x, pv1.z);
        Point q1 = new Point(qv1.x, qv1.z);
        Point p2 = new Point(pv2.x, pv2.z);
        Point q2 = new Point(qv2.x, qv2.z);
        return doIntersect(p1, q1, p2, q2);
        
    }
}
