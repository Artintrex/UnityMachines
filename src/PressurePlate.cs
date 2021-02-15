using UnityEngine;

namespace MachineParts.Scripts
{
    public class PressurePlate : Machine
    {
        public GameObject plate;
        public float activationDistance = 0.5f;
        public float progress;
        public float maxProgress;
        public bool getScore;

        public Signal outputSignal = Signal.PowerOn;

        private float _initialHeight;

        private void Start()
        {
            _initialHeight = plate.transform.localPosition.y;
        }

        // Update is called once per frame
        private void Update()
        {
            if (plate.transform.localPosition.y < _initialHeight)
            {
                progress = (_initialHeight - plate.transform.localPosition.y) / activationDistance;
            }
            else progress = 0;

            UpdateSignal(outputSignal, progress > 1 ? 1 : 0);

            if (!getScore) return;
            if (progress > maxProgress)
            {
                maxProgress = progress;
            }
        }
    }
}