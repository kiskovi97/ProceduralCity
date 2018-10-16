using UnityEngine;

namespace Assets.Scripts.AdvancedCity
{
    public class Vehicle : MonoBehaviour
    {
        public Animator anim;
        public MovementPoint nextPoint;
        public MovementPoint elozoPoint;
        protected float actualspeed = 1.5f;
        public void Start()
        {
            if (anim == null)
            anim = GetComponent<Animator>();
        }
        public virtual void setPoint(MovementPoint next)
        {
            nextPoint = next.GetNextPoint();
            if (nextPoint == null)
                nextPoint = next;
            if (nextPoint != next)
                elozoPoint = next;
            transform.rotation = Quaternion.LookRotation(nextPoint.direction);
        }

        public virtual void Update()
        {
            Step();
        }
        public virtual void Step()
        {
            if (nextPoint == null) return;
            float length = (nextPoint.center - transform.position).magnitude;
            if (length < 0.1f)
            {
                MovementPoint tmp = nextPoint;
                nextPoint = nextPoint.GetNextPoint();
                if (tmp != nextPoint && nextPoint != null)
                {
                    elozoPoint = tmp;
                }
                if (anim != null)
                {
                    anim.SetFloat("Blend", 0);
                }
            }
            else
                Move();
        }
        public virtual void Move()
        {
            if (anim != null)
            {
                anim.SetFloat("Blend", 1);
            }
            Vector3 toward = (nextPoint.center - transform.position);
            transform.rotation = Quaternion.LookRotation(toward);
            transform.position += toward.normalized * actualspeed * 0.01f;
        }

    }
}
