using UnityEngine;

namespace MachineParts.Scripts
{
    public class Gate : Machine
    {
        Vector3 LockedPosition;
        [Tooltip("How much this door moves")]
        public Vector3 OpenDistance;
        public enum mode
        {
            Trigger,
            Progressive
        }
        public mode OpenMode = mode.Progressive;
        public bool SelfPowered;

        void Start()
        {
            LockedPosition = transform.localPosition;
        }

        public float Progress, Speed;
        void Update()
        {
            if (IsPowered)
            {
                switch (OpenMode)
                {
                    case mode.Trigger:
                    {
                        transform.localPosition = LockedPosition + OpenDistance;
                        UpdateSignal(Signal.PowerOn, 1);
                        break;
                    }
                    case mode.Progressive:
                    {
                        if (Progress < 1)
                        {
                            transform.localPosition = LockedPosition + (OpenDistance * Progress);
                            if (SelfPowered)
                            {
                                Progress += Time.deltaTime * Speed;
                            }

                        }
                        else if (Progress >= 1)
                        {
                            UpdateSignal(Signal.PowerOn, 1);
                            transform.localPosition = LockedPosition + OpenDistance;
                        }
                        break;
                    }
                }
            }
            else
            {
                switch (OpenMode)
                {
                    case mode.Trigger:
                    {
                        transform.localPosition = LockedPosition;
                        UpdateSignal(Signal.PowerOn, 0);
                        break;
                    }
                    case mode.Progressive:
                    {
                        if (Progress > 0)
                        {
                            transform.localPosition = LockedPosition + (OpenDistance * Progress);
                            if (SelfPowered)
                            {
                                Progress -= Time.deltaTime * Speed;
                            }

                        }
                        else if (Progress <= 0)
                        {
                            UpdateSignal(Signal.PowerOn, 0);
                            transform.localPosition = LockedPosition;
                        }
                        break;
                    }
                }
            }
        }
        public override void ConnectionUpdate()
        {

        }
    
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.DrawSphere(transform.TransformPoint(LockedPosition + OpenDistance - transform.localPosition), 5);
            }
            else
            {
                Gizmos.DrawSphere(transform.position + OpenDistance, 5);
            }
        }
    }
}
