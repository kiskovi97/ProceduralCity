
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    class Road
    {
        Crossing egyik;
        Vector3[] line_egyik;
        Vector3[] line_masik;
        Crossing masik;
        public Road()
        {
            egyik = null;
            line_egyik = null; 
            line_masik = null; 
            masik = null; 
        }
        public void setSzomszedok(Crossing a, Crossing b)
        {
            egyik = a;
            masik = b;
        }
        public void addLine(Crossing be, Vector3 a, Vector3 b)
        {
            if (be.Equals(egyik))
            {
                line_egyik = new Vector3[2];
                line_egyik[0] = a;
                line_egyik[1] = b;
            } 
            if (be.Equals(masik))
            {
                line_masik = new Vector3[2];
                line_masik[0] = a;
                line_masik[1] = b;
            } 
        }
        public void Draw()
        {
            //if (line_egyik!=null)
            //    Debug.DrawLine(line_egyik[0], line_egyik[1], Color.blue, 1000, false);
            //if (line_masik != null)
            //    Debug.DrawLine(line_masik[0], line_masik[1], Color.blue, 1000, false);
            if (line_egyik != null && line_masik != null)
            {
                Debug.DrawLine(line_egyik[0], line_masik[1], Color.blue, 1000, false);
                Debug.DrawLine(line_egyik[1], line_masik[0], Color.blue, 1000, false);

                Vector3[] baloldal = { (line_masik[0] + line_masik[1] * 3) / 4 , (line_egyik[0] * 3 + line_egyik[1]) / 4 };
                Vector3[] jobboldal = {  (line_egyik[0] + line_egyik[1] * 3) / 4 , (line_masik[0] * 3 + line_masik[1]) / 4};
                Debug.DrawLine(baloldal[0], (baloldal[1] + baloldal[0])/2, Color.black, 1000, false);
                Debug.DrawLine(jobboldal[0], (jobboldal[1] + jobboldal[0])/2, Color.black, 1000, false);
            }
        }
        public bool isSame(Crossing a, Crossing b)
        {
            return ((egyik == a && masik == b) || (egyik == b && masik == a));
        }
        public Crossing NextCros(Crossing a)
        {
            if (egyik.Equals(a))
            {
                return masik;
            }
            if (masik.Equals(a))
            {
                return egyik;
            }
            return null;
        }
    }
}
