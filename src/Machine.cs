using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MachineParts.Scripts
{
    public abstract class Machine : MonoBehaviour
    {
        public enum Signal
        {

            PowerOn,
            Speed,
            Power,
            Progress,
            A,
            B,
            C,
            Value
        }
    
        public GameObject[] _outputObjects;

        [Tooltip("Turns the machine on")]
        public bool IsPowered;

        public List<Connection> Connections = new List<Connection>();

        void Awake()
        {
            ResetConnection();
        }

        //private void OnValidate()
        //{
        //    DisconnectAll();
        //    ResetConnection();
        //}

        private void ResetConnection()
        {
            foreach (GameObject go in _outputObjects)
            {
                if (go)
                {
                    go.GetComponent<Machine>().MakeConnection(this);
                }
            }
        }

        private void OnDestroy()
        {
            DisconnectAll();
        }

        public void DisconnectAll()
        {
            foreach (Connection i in Connections.ToList())
            {
                i.Disconnect();
            }
        }

        public void Disconnect(Machine other)
        {
            foreach (Connection i in Connections)
            {
                if ((i.InputMachine == other && i.OutputMachine == this) || (i.OutputMachine == other && i.InputMachine == this))
                {
                    i.Disconnect();
                    break;
                }
            }
        }

        public void MakeConnection(Machine input)
        {
            new Connection(input, this);
        }

        public void UpdateSignal(Signal signal, float value)
        {
            foreach (Connection i in Connections)
            {
                if (i.InputMachine == this)
                {
                    i.SignalData[signal] = value;
                    i.Ping();
                }
            }
        }

        public float ReadIncomingSignal(Signal signal)
        {
            float total = 0;
        
            foreach (Connection i in Connections)
            {
                if (i.OutputMachine == this)
                {
                    i.SignalData.TryGetValue(signal, out var value);
                    total += value;
                }
            }

            return total;
        }

        public float ReadOutboundSignal(Signal signal)
        {
            float total = 0;

            foreach (Connection i in Connections)
            {
                if (i.InputMachine == this)
                {
                    i.SignalData.TryGetValue(signal, out var value);
                    total += value;
                }
            }

            return total;
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (Connection i in Connections)
            {
                Gizmos.DrawLine(i.InputMachine.transform.position, i.OutputMachine.transform.position);
            }

            if (Application.isEditor && !Application.isPlaying)
            {
                foreach (GameObject i in _outputObjects)
                {
                    if (!i) continue;
                    Gizmos.DrawLine(transform.position, i.transform.position);
                }
            }
        }

        public virtual void ConnectionUpdate()
        {

        }

        private void ActivationCall()
        {
            if (ReadIncomingSignal(Signal.PowerOn) > 0)
            {
                IsPowered = true;
            }
            else
            {
                IsPowered = false;
            }
            ConnectionUpdate();
        }

        //Holds information about active connection between two machine objects
        public class Connection
        {
            //Signal Sender
            public Machine InputMachine;
            //Signal Receiver
            public Machine OutputMachine;

            public Dictionary<Signal, float> SignalData;

            public Connection(Machine input, Machine output)
            {
                InputMachine = input;
                OutputMachine = output;

                input.Connections.Add(this);
                output.Connections.Add(this);

                SignalData = new Dictionary<Signal, float>();
            }

            public void Disconnect()
            {
                InputMachine.Connections.Remove(this);
                if (InputMachine != OutputMachine)
                {
                    OutputMachine.Connections.Remove(this);
                }
            }

            public void Ping()
            {
                OutputMachine.ActivationCall();
            }
        }
    }
}