using UnityEngine;
namespace Assets.Scripts.AdvancedCity
{
    public class ObjectConverter
    {
        GameObject obj;
        public ObjectConverter(GameObject obj)
        {
            this.obj = obj;
        }
        public void Rectangle(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            obj.transform.position = (a + b + c + d) / 4;
            obj.transform.localScale = new Vector3((a - b).magnitude, 0.5f, (a - c).magnitude / 2);
            obj.transform.rotation = Quaternion.LookRotation(a - c);
        }
        public void Line(Vector3 a, Vector3 b, float scale, float height1, float height2)
        {
            a += new Vector3(0, height1, 0);
            b += new Vector3(0, height2, 0);
            LineRenderer renderer = obj.GetComponent<LineRenderer>();
            if (renderer !=null)
            {
                obj.transform.rotation = new Quaternion();
                MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                if (meshRenderer != null) meshRenderer.enabled = false;
                renderer.SetPositions(new Vector3[] { a, b });
            } else
            {
                obj.transform.position = (a + b) / 2;
                obj.transform.rotation = Quaternion.LookRotation(a - b);
                obj.transform.localScale = new Vector3(scale, scale, (a - b).magnitude * 50);
            }
        }
        public void Forward(Vector3 position, Vector3 forward, float height)
        {
            obj.transform.position = position + new Vector3(0, height, 0);
            obj.transform.rotation = Quaternion.LookRotation(forward, new Vector3(0, 1, 0));
        }
    }
}
