using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.AdvancedCity
{
    public class CrossingManager : MonoBehaviour
    {
        public List<Crossing> crossings = new List<Crossing>();
        public void SetCrossings(List<Crossing> crossings)
        {
            this.crossings = crossings;
        }

        // Use this for initialization
        void Start()
        {
            MovementPointContainer container = FindObjectOfType<MovementPointContainer>();
            foreach (Crossing cros in crossings) cros.SetMovementPoints(container);
            Debug.Log("MovementPointContainer is Set");
        }

        private int tick = 0;
        private bool standBy = true;
        // Update is called once per frame
        void Update()
        {
            tick++;
            if (standBy && tick > 100)
            {
                foreach (Crossing cros in crossings) cros.Switch(standBy);
                tick = 0;
                standBy = false;
                Debug.Log("Valtott");
            }
            if (tick > 200)
            {
                foreach (Crossing cros in crossings) cros.Switch(standBy);
                tick = 0;
                standBy = true;
                Debug.Log("Valtott");
            }
        }
    }
}
