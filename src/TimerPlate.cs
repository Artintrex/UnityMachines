using System.Collections;
using UnityEngine;

namespace MachineParts.Scripts
{
    public class TimerPlate : Machine
    {
        public float timerCount = 5.0f;
        public bool powerOnSignal = true;
        public Transform gauge;
        
        public Signal outputSignal = Signal.PowerOn;
    
        private Coroutine _routine;
        private bool _isActivated;

        private float _angleStep;
        private void Start()
        {
            _angleStep = 360 / timerCount;
        }

        private void Activate()
        {
            if (!_isActivated)
            {
                _routine = StartCoroutine(Activation());
            }
            else
            {
                StopCoroutine(_routine);
                _routine = StartCoroutine(Activation());
            }
        
        }

        private IEnumerator Activation()
        {
            _isActivated = true;
            if (powerOnSignal)
            {
                UpdateSignal(outputSignal, 1);
            }
            else
            {
                UpdateSignal(outputSignal, 0);
            }


            float timer = timerCount;

            while (timer > 0)
            {
                timer -= Time.deltaTime;
            
                gauge.rotation = Quaternion.Euler(Vector3.back * (timer * _angleStep));
            
                yield return null;
            }

            if (powerOnSignal)
            {
                UpdateSignal(outputSignal, 0);
            }
            else
            {
                UpdateSignal(outputSignal, 1);
            }
            
            _isActivated = false;
        }

        public override void ConnectionUpdate()
        {
            base.ConnectionUpdate();

            if (IsPowered)
            {
                Activate();
            }
        }
    }
}
