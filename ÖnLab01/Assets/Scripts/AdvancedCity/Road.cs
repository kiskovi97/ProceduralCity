
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
        MovementPoint[] egyik_be;
        MovementPoint[] egyik_ki;
        int sav = 0;
        Crossing masik;
        Vector3[] line_masik;
        MovementPoint[] masik_be;
        MovementPoint[] masik_ki;
        public int Savok()
        {
            return sav;
        }
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
            if (a.main && b.main)
            {
                sav = 3;
            }
            else sav = 1;
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
        public void addMovePoint(Crossing be, MovementPoint[] befele, MovementPoint[] kifele)
        {
            if (be.Equals(egyik))
            {
                egyik_be = befele;
                egyik_ki = kifele;
            }
            if (be.Equals(masik))
            {
                masik_be = befele;
                masik_ki = kifele;
            }
            if (egyik_be != null && masik_ki != null)
            {
                for (int i=sav; i>0; i--)
                {
                    int j = egyik_be.Length - i;
                    if (j < 0) j = 0;
                    int x = masik_ki.Length - i;
                    if (x < 0) j = 0;
                    egyik_be[j].ConnectPoint(masik_ki[x]);

                    j = egyik_ki.Length - i;
                    if (j < 0) j = 0;
                    x = masik_be.Length - i;
                    if (x < 0) j = 0;
                    masik_be[x].ConnectPoint(egyik_ki[j]);
                }
            }
        }
        public void Draw(bool depthtest)
        {
            int a = egyik_be.Length > masik_ki.Length ? masik_ki.Length : egyik_ki.Length;
            for (int i=0; i< a -1; i++)
            {
                Debug.DrawLine((egyik_be[i].center + egyik_be[i+1].center)/2, (masik_ki[i].center + masik_ki[i+1].center)/2, Color.white, 1000, depthtest);
                Debug.DrawLine((egyik_ki[i].center + egyik_ki[i + 1].center) / 2, (masik_be[i].center + masik_be[i + 1].center) / 2, Color.white, 1000, depthtest);
            }
            if (line_egyik != null && line_masik != null)
            {
                Debug.DrawLine(line_egyik[0], line_masik[1], Color.blue, 1000, depthtest);
                Debug.DrawLine(line_egyik[1], line_masik[0], Color.blue, 1000, depthtest);
                Debug.DrawLine((line_egyik[1] + line_egyik[0])/2, (line_masik[0] + line_masik[1])/2, Color.white, 1000, depthtest);
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
