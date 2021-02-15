using UnityEngine;

namespace MachineParts.Scripts
{
    public class Wheel : Machine
    {
        public enum mode
        {
            free,
            left,
            right
        }
        public ForceMode PowerMode = ForceMode.Force;
        [Tooltip("Constraint rotation direction")]
        public mode Mode = 0;
        [Tooltip("Locks this machine after progress is complete")]
        public bool ProgressLock = true;
        [Tooltip("How many turns it takes for progress completion")]
        [Range(1, 100)]
        public int ProgressTurns = 1;

        public float MaxSpeed = 10.0f;
        public float Speed = 5.0f;
        public float Power = 100.0f;
        public bool SignalOverride = true;


        Rigidbody rb;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        bool reset = false;

        Quaternion oldRot = Quaternion.identity;

        private float _progress;
        void Update()
        {
            _progress += Mathf.Abs(oldRot.z - transform.rotation.z) / 2;
        

            if (_progress > ProgressTurns)
            {
                if (ProgressLock)
                {
                    MaxSpeed = 0;
                }
                UpdateSignal(Signal.PowerOn, 1);
            }

            UpdateSignal(Signal.Progress, _progress / ProgressTurns);

            if (IsPowered)
            {
                rb.AddRelativeTorque(new Vector3(0, 0, Power), PowerMode);
            }
            rb.maxAngularVelocity = MaxSpeed;
            Speed = rb.angularVelocity.z;

            if (Mode == mode.right)
            {
                if (Speed > 0) reset = true;
            }
            else if (Mode == mode.left)
            {
                if (Speed < 0) reset = true;
            }

            //Reset
            if (reset)
            {
                rb.isKinematic = true;

                if (transform.rotation.z > 0.01f)
                {
                    transform.Rotate(0, 0, -1);
                }
                else if (transform.rotation.z < -0.01f)
                {
                    transform.Rotate(0, 0, 1);
                }
                else
                {
                    rb.isKinematic = false;
                    reset = false;
                }
            }
            oldRot = transform.rotation;
        }

        public override void ConnectionUpdate()
        {
            if (!SignalOverride)
            {
                Speed = ReadIncomingSignal(Signal.Speed);
                Power = ReadIncomingSignal(Signal.Power);
            }
        }
    }
}
