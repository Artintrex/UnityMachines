using UnityEngine;

namespace MachineParts.Scripts
{
    public class Platform : Machine
    {
        [Tooltip("How much this door moves")]
        public Vector3 OpenDistance;

        public bool SelfPowered = false;
        public float Progress;
        public bool autoCalculate;

        public float Speed
        {
            get => speed;

            set
            {
                speed = value * 50 / OpenDistance.magnitude;
            
                if (autoCalculate)
                {
                    CalculateTime();
                }
            } 
        }
    
        [SerializeField]
        private float speed;

        private Vector3 _lockedPosition;
        private Rigidbody2D _rb;

        void Start()
        {
            _lockedPosition = transform.position;
            _rb = GetComponent<Rigidbody2D>();
        
            speed *= 50 / OpenDistance.magnitude;
        
            if (autoCalculate)
            {
                CalculateTime();
            }
        }
    
        void Update()
        {
            if (IsPowered)
            {
                if (Progress < 1)
                {
                    _rb.MovePosition(_lockedPosition + (OpenDistance * Progress));
                    if (SelfPowered)
                    {
                        Progress += Time.deltaTime * speed;
                    }

                }
                else if (Progress >= 1)
                {
                    UpdateSignal(Signal.PowerOn, 1);
                }
            }
            else
            {
                if (Progress > 0)
                {
                    _rb.MovePosition(_lockedPosition + (OpenDistance * Progress));
                    if (SelfPowered)
                    {
                        Progress -= Time.deltaTime * speed;
                    }

                }
                else if (Progress <= 0)
                {
                    UpdateSignal(Signal.PowerOn, 0);
                }
            }
        }

        public override void ConnectionUpdate()
        {

        }
    
        private void CalculateTime()
        {
            RepeaterController repeater = transform.root.GetComponentInChildren<RepeaterController>();
            if (repeater)
            {
                repeater.RepeatingDelay = 1 / speed;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.DrawSphere(_lockedPosition + OpenDistance, 5);
            }
            else
            {
                Gizmos.DrawSphere(transform.position + OpenDistance, 5);
            }
        }
    }
}