using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    [System.Serializable]
    public class CarPath : System.Object
    {
        public MovementPoint[] others;

        public MovementPoint[] input;

        public MovementPoint[] output;

        public MovementPoint leftCross;

        public MovementPoint rightCross;

        public MovementPoint tramInput;

        public MovementPoint tramOutput;

        public MeshRenderer[] lamps;

        public CarPath() { }

        public CarPath(MovementPoint[] others, 
            MovementPoint[] input, 
            MovementPoint[] output, 
            MovementPoint leftCross, 
            MovementPoint rightCross, 
            MovementPoint tramInput, 
            MovementPoint tramOutput, 
            MeshRenderer[] lamps)
        {
            this.others = others;
            this.input = input;
            this.output = output;
            this.leftCross = leftCross;
            this.rightCross = rightCross;
            this.tramInput = tramInput;
            this.tramOutput = tramOutput;
            this.lamps = lamps;
        }

        public void SetMovementPoints(MovementPointContainer container)
        {
            leftCross = container.GetMovementPoint(leftCross.ID);
            rightCross = container.GetMovementPoint(rightCross.ID);
            if (tramInput != null) tramInput = container.GetMovementPoint(tramInput.ID);
            if (tramOutput != null) tramOutput = container.GetMovementPoint(tramOutput.ID);
            for (int i = 0; i < output.Length; i++)
                output[i]= container.GetMovementPoint(output[i].ID);
            for (int i = 0; i < input.Length; i++)
                input[i] = container.GetMovementPoint(input[i].ID);
            for (int i = 0; i < others.Length; i++)
                others[i] = container.GetMovementPoint(others[i].ID);
            //DrawHelpLines(false);
        }

        internal IEnumerable<MovementPoint> GetPoints()
        {
            List<MovementPoint> output = new List<MovementPoint>();
            output.AddRange(others);
            output.AddRange(input);
            output.AddRange(output);
            output.Add(leftCross);
            output.Add(rightCross);
            if (tramInput != null)
                output.Add(tramInput);
            if (tramOutput != null)
                output.Add(tramOutput);
            return output;
        }

        public void DrawHelpLines(bool depthtest)
        {
           // leftCross.Draw(depthtest);
           // rightCross.Draw(depthtest);
            if (tramInput != null) tramInput.Draw(depthtest);
            if (tramOutput != null) tramOutput.Draw(depthtest);
            for (int i = 0; i < output.Length; i++)
                output[i].Draw(depthtest);
            for (int i = 0; i < input.Length; i++)
                input[i].Draw(depthtest);
            for (int i = 0; i < others.Length; i++)
                others[i].Draw(depthtest);
        }

        public CarPath SwitchCopy()
        {
            return new CarPath(others, output, input, rightCross, leftCross, tramOutput, tramInput, lamps);
        }
    }
}
