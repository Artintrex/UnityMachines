using System.Collections;
using UnityEngine;
//using UnityEditor.ShaderGraph.Internal;

namespace MachineParts.Scripts
{
    public class Rocker : Machine
    {
        [Header("Target Agent Settings")]
        public string[] TriggerTags;
        [Range(0, 2500)]
        public float Force;
        [Header("Rocker Settings")]
        //public int Health;
        public float DecayTimer;
        [Header("Rocker Animation Settings")]
        public GameObject Sprite;
        public float ExpansionSize;
        public int OscillationCount;
        public float OscillationTime;
        private AudioSource _audio;

        public AudioClip Sound;

        private Vector3 _startScale;
        private Vector3 _targetScale;

        private void Start()
        {
            _startScale = Sprite.transform.localScale;
            _targetScale = _startScale * ExpansionSize;
            _audio = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {

           
                _audio.PlayOneShot(Sound);
          


            if (IsPowered)
            {
                foreach (string s in TriggerTags)
                {
                    if (other.gameObject.CompareTag(s))
                    {
                        Rigidbody2D trb = other.gameObject.GetComponentInParent<Rigidbody2D>();
                        trb.AddForce(-other.contacts[0].normal.normalized * Force, ForceMode2D.Impulse);
                        //trb.velocity = (other.transform.position - transform.position).normalized * Force;
                        Animate();
                        //Health--;
                        UpdateSignal(Signal.PowerOn, 1);
                   
                    }
                }
            }
        }

        //private bool _isBeingDestroyed = false;
        //private IEnumerator DestroyEvent()
        //{
        //    _isBeingDestroyed = true;
        //    gameObject.AddComponent<Rigidbody2D>().gravityScale = 50;
        //    Destroy(GetComponent<Collider2D>());
        //    yield return new WaitForSeconds(DecayTimer);
        //    Destroy(gameObject);
        //}

        //private Coroutine _animationRoutine;
        private bool _animationRunning;
        private void Animate()
        {
            if (!_animationRunning)
            {
                //StopCoroutine(_animationRoutine);
                StartCoroutine(ExpansionAnimation());
            }
        
        }
        private IEnumerator ExpansionAnimation()
        {
            Vector3 velocity = Vector3.zero;
            bool flag = true;
            int count = OscillationCount;
            _animationRunning = true;
            while (count > 0)
            {
                if (flag)
                {
                    Sprite.transform.localScale = Vector3.SmoothDamp(Sprite.transform.localScale, _targetScale, ref velocity, OscillationTime);

                    if ((Sprite.transform.localScale - _targetScale).sqrMagnitude < .001f) flag = false;
                }
                else
                {
                    Sprite.transform.localScale = Vector3.SmoothDamp(Sprite.transform.localScale, _startScale, ref velocity, OscillationTime);

                    if ((Sprite.transform.localScale - _startScale).sqrMagnitude < .001f)
                    {
                        count--;
                        flag = true;
                    }
                }
                yield return null;
            }
            _animationRunning = false;
            UpdateSignal(Signal.PowerOn, 0);
        }
    }
}