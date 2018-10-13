using UnityEngine;

public class HelpLine
{
    public Vector3[] mainLine;
    public Vector3[] sideline;
    public Vector3 roadEdgeCross;
    public Vector3 sideCross;
    public HelpLine(Vector3[] mainLine, Vector3[] sideLine, Vector3 roadEdgeCross, Vector3 sideCross)
    {
        this.mainLine = mainLine;
        this.sideline = sideLine;
        this.roadEdgeCross = roadEdgeCross;
        this.sideCross = sideCross;
    }
}
