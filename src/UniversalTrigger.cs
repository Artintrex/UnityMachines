using System;
using UnityEngine;

namespace MachineParts.Scripts
{
    public class UniversalTrigger : Machine
    {
        public string[] TriggerTags;
        public Status.StatusMode TriggerStatus;
        public KeyCode ActivationKey = KeyCode.None;

        private void Update()
        {
            if (ActivationKey != KeyCode.None)
            {
                if (count > 0)
                {
                    if (Input.GetKeyDown(ActivationKey))
                    {
                        UpdateSignal(Signal.PowerOn, Convert.ToSingle(keyswitch));
                        keyswitch = !keyswitch;
                    }
                }
            }
        }
        bool keyswitch = true;
        int count = 0;
        private void OnTriggerEnter2D(Collider2D other)
        {
            foreach (string s in TriggerTags)
            {
                if (other.CompareTag(s))
                {
                    count++;
                    if (ActivationKey == KeyCode.None)
                    {
                        UpdateSignal(Signal.PowerOn, Convert.ToSingle(true));
                    }

                    Status status = other.GetComponent<Status>();
                    if (status != null)
                    {
                        status.AddStatus(TriggerStatus);
                    }
                    break;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            foreach (string s in TriggerTags)
            {
                if (other.CompareTag(s))
                {
                    count--;
                    if (ActivationKey == KeyCode.None)
                    {
                        UpdateSignal(Signal.PowerOn, Convert.ToSingle(false));
                    }
                    Status status = other.GetComponent<Status>();
                    if (status != null)
                    {
                        status.RemoveStatus(TriggerStatus);
                    }
                    break;
                }
            }
        }
        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Color tmp = Color.green;
            tmp.a /= 10;
            Gizmos.color = tmp;
            
            
            Gizmos.matrix = transform.localToWorldMatrix;
            
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }

        public override void ConnectionUpdate()
        {
            //if (IsPowered) enabled = true;
            //else enabled = false;
        }
    }
}
