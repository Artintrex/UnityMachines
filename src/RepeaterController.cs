using System;
using UnityEngine;

namespace MachineParts.Scripts
{
    public class RepeaterController : Machine
    {
        [Header("RepeaterSettings")]
        public float RepeatingDelay = 1.0f;
        public float startDelay = 0;

        private void Start()
        {
            _timer = startDelay;
        }

        private float _timer;
        private bool _switch = false;
        private void Update()
        {
            if (IsPowered)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _switch = !_switch;
                    UpdateSignal(Signal.PowerOn, Convert.ToSingle(_switch));
                    _timer = RepeatingDelay;
                }
            }
        }
    }
}