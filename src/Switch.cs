using UnityEngine;

namespace MachineParts.Scripts
{
    public class Switch : Machine
    {
        public GameObject[] objects;
    
        public override void ConnectionUpdate()
        {
            foreach (GameObject go in objects)
            {
                if (IsPowered)
                {
                    go.SetActive(true);
                }
                else
                {
                    go.SetActive(false);
                }
            }
        }
    }
}
