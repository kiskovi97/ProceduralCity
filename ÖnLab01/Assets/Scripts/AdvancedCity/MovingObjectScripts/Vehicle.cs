using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    public class Vehicle : MonoBehaviour
    {
        public Animator anim;
        public MovementPosition point;
        public new Rigidbody rigidbody;
        public float actualSpeed = 1.5f;
        public virtual void Start()
        {
            if (anim == null)
            anim = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            MovementPointContainer container = FindObjectOfType< MovementPointContainer>();
            if (container != null)
            {
                point.nextPoint = container.GetMovementPoint(point.nextPoint.ID);
                point.prevPoint = container.GetMovementPoint(point.prevPoint.ID);
                point.pos = point.prevPoint.center;
                transform.position = point.pos;
            }
        }
        public virtual void SetPoint(MovementPoint next)
        {
            point.prevPoint = next;
            point.nextPoint = next.GetNextPoint();
            if (point.nextPoint == null)
                point.nextPoint = next;
            if (point.nextPoint != next)
                point.prevPoint = next;
            point.pos = next.center;
            transform.rotation = Quaternion.LookRotation(next.direction);
            transform.position = next.center;
        }

        public virtual void FixedUpdate()
        {
            Step();
        }

        public virtual void Step()
        {
            point.Step(actualSpeed * Time.deltaTime);
            Move();
        }

        public virtual void Move()
        {
            if (anim != null)
            {
                anim.SetFloat("Blend", 1);
            }
            transform.rotation = Quaternion.LookRotation(point.Forward);
            SetPosition(point.pos);
        }

        public void SetPosition(Vector3 position)
        {
            if (rigidbody != null)
                rigidbody.MovePosition(position);
            else
                transform.position = position;
        }
    }
}
