using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Door : MeshElementImpl
{
    public Door(Vector3 LD, Vector3 LU, Vector3 RD, Vector3 RU)
    {
        Vector3 inner = Vector3.Cross((LD - LU), (LD - RD))/2;
        Vector3 LCenter = (2 * LD + LU) / 3;
        Vector3 RCenter = (2 * RD + RU) / 3;
        shapes.Add(new ReactRectangle(LCenter, LCenter + inner, LU, LU + inner, (int)MaterialEnum.SIMPLEWALL));
        shapes.Add(new ReactRectangle(LD, LD + inner, LCenter, LCenter + inner, (int)MaterialEnum.GROUNDWALL));
        
        shapes.Add(new ReactRectangle(RU, RU + inner, RCenter, RCenter + inner, (int)MaterialEnum.SIMPLEWALL));
        shapes.Add(new ReactRectangle(RCenter, RCenter + inner, RD, RD + inner, (int)MaterialEnum.GROUNDWALL));

        shapes.Add(new ReactRectangle(LU, LU + inner, RU, RU + inner, (int)MaterialEnum.SIMPLEWALL));

        shapes.Add(new Rectangle(LD + inner, RD + inner, LU + inner, RU + inner, (int)MaterialEnum.DOOR));

    }
}
