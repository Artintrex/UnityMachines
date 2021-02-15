using System.Collections;
using UnityEngine;

namespace MachineParts.Scripts
{
    public class PivotRotation : Machine
    {
        private Quaternion StartRotation;
        public Vector3 TargetRotation;

        bool isRotating = false;
        bool isRotated = false;

        public float Speed;
        public bool SignalOverride = true;

        void Start()
        {
            StartRotation = transform.rotation;
        }
        void Update()
        {
            if (IsPowered)
            {
                if (!isRotating && !isRotated)
                {
                    StartCoroutine(Rotate(Quaternion.Euler(TargetRotation)));
                }
            }
            else
            {
                if (!isRotating && isRotated)
                {
                    StartCoroutine(Rotate(StartRotation));
                }
            }
        }

        IEnumerator Rotate(Quaternion angle)
        {
            isRotating = true;
            while (transform.rotation != angle)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * Speed);

                yield return null;
            }
            isRotated = !isRotated;
            isRotating = false;
        }

        public override void ConnectionUpdate()
        {
            if (!SignalOverride)
            {
                Speed = ReadIncomingSignal(Signal.Speed);
            }
        }
    }
}
