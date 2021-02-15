using UnityEngine;

namespace MachineParts.Scripts
{
    public class SpawnerController : Machine
    {
        [Header("Spawner Settings")]
        public GameObject Prefab;
        public Rect SpawnArea;
        [Range(1, 1024)]
        public int MaxObjects = 10;

        public float Speed = 5.0f;
        public bool SignalOverride = true;

        private GameObject[] _objects;

        void Start()
        {
            //PreAllocate Objects
            _objects = new GameObject[MaxObjects];
            for (int i = 0; i < MaxObjects; ++i)
            {
                _objects[i] = Instantiate(Prefab);
                _objects[i].SetActive(false);
            }
        }

        public override void ConnectionUpdate()
        {
            if (IsPowered)
            {
                SpawnObject((int)ReadIncomingSignal(Signal.Power));
            }
        }

        //Spawn objects
        void SpawnObject(int count)
        {
            if (!SignalOverride) Speed = ReadIncomingSignal(Signal.Speed);

            int counter = 0;
            for (int i = 0; i < MaxObjects; ++i)
            {
                if (!_objects[i].activeSelf)
                {
                    _objects[i].transform.position = new Vector2(Random.Range(SpawnArea.x, SpawnArea.width) + transform.position.x,
                        Random.Range(SpawnArea.y, SpawnArea.height) + transform.position.y);
                    _objects[i].SetActive(true);
                    if (Speed != 0)
                    {
                        _objects[i].GetComponent<Rigidbody>().AddForce(transform.up * Speed, ForceMode.Impulse);
                    }
                    counter++;

                    if (counter >= count) break;
                }
            }
            UpdateSignal(Signal.Progress, ReadOutboundSignal(Signal.Progress) + counter);
        }


    }
}