using System.Collections.Generic;
using UnityEngine;

namespace MachineParts.Scripts
{
    public class BreakableDoor : Machine
    {
        public float destroyTime = 10.0f;
        public Vector2 explosionVector;
    
        private List<DoorPiece> _pieces;

        private AudioSource _audio;
        public AudioClip DestroySound;

        private void Start()
        {

            _audio = GetComponent<AudioSource>();

            _pieces = new List<DoorPiece>();
        
            foreach (Transform child in transform)
            {
                var piece = child.GetComponent<DoorPiece>();

                if (!piece) continue;
            
                _pieces.Add(piece);
                piece.PreCalculation(explosionVector);
            }
        }

        private bool _exploded;
        private void Explode()
        {

            _audio.PlayOneShot(DestroySound);

            foreach(var piece in _pieces)
            {
                piece.Explode();
            }
            Destroy(gameObject, destroyTime);
            Destroy(GetComponent<BoxCollider2D>());
            _exploded = true;
        }
    
        public override void ConnectionUpdate()
        {
            if (IsPowered && !_exploded) Explode();
        }

        private void Update()
        {
            if (IsPowered && !_exploded) Explode();
        }
    }
}