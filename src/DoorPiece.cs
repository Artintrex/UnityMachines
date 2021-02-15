using UnityEngine;

namespace MachineParts.Scripts
{
    public class DoorPiece : MonoBehaviour
    {
        [Range(3, 20)]
        public float destroyTime = 3.0f;
        public float randomSpread = 0.2f;

        private Rigidbody2D _rb;
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private float _realDestroyTime;
        public void Explode()
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.gravityScale = 50;
            _rb.AddForce(_force, ForceMode2D.Impulse);
            Destroy(gameObject, _realDestroyTime);
        }

        private Vector2 _force;
        public void PreCalculation(Vector2 force)
        {
            //Get random point on unit circle
            var randomRadian = Mathf.PI * randomSpread;
            var randomAngle = Random.Range(-randomRadian, randomRadian);
            var ca = Mathf.Cos(randomAngle);
            var sa = Mathf.Sin(randomAngle);
        
            _force = new Vector2(force.x * ca - force.y * sa, force.x * sa + force.y * ca);

            _realDestroyTime = Random.Range(destroyTime - 2, destroyTime + 2);
        }
    }
}
