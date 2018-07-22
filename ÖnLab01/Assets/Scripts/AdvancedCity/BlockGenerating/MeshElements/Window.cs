﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Window : MeshElementImpl
{
    static int MATERIAL = 5;
    public Window(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        int plusz = (int)(Random.value * 3);
        shapes.Add(new Rectangle(LD, RD, LU, RU, MATERIAL + plusz));
    }

}
