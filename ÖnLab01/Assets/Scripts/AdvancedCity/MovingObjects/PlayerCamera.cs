using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AdvancedCity
{
    public class PlayerCamera : MonoBehaviour
    {
        public Transform target;
        float maxLeft = 0.2f;

        public void Start()
        {
            
        }
        public virtual void Update()
        {
             /*var x = Input.GetAxis("MouseX") * Time.deltaTime * 0.00003f;
             var y = Input.GetAxis("MouseY") * Time.deltaTime * 0.0001f;
             transform.position += target.rotation*new Vector3(x * -1, y * -1, 0);
            if (transform.position.y <0.1f) transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
            if (transform.localPosition.y > 1.0f) transform.localPosition = new Vector3(transform.localPosition.x, 1.0f, transform.localPosition.z);
            if (transform.localPosition.y < -1.0f) transform.localPosition = new Vector3(transform.localPosition.x, -1.0f, transform.localPosition.z);
            if (transform.localPosition.x < -maxLeft) transform.localPosition = new Vector3(-maxLeft, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x > maxLeft) transform.localPosition = new Vector3(maxLeft, transform.localPosition.y, transform.localPosition.z);
            Vector3 other = target.position;
             Vector3 dir = other - transform.position;
             transform.rotation = Quaternion.LookRotation(dir);*/

        }

    }
}
