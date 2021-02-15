using System.Collections;
using UnityEngine;

namespace MachineParts.Scripts
{
    public class Gear : Machine
    {
        public GameObject pivot;
        public float speed;
        public float playerContactDuration = 0.5f;
        public float contactLossDuration = 1.0f;
    
        private bool _isHighLight;
        private bool _isActivated;
        private Renderer _renderer;
        private Rigidbody2D _playerRb;

        private static readonly int Vector148543C7B = Shader.PropertyToID("Vector1_48543C7B");
        private static readonly int Vector117Be9A58 = Shader.PropertyToID("Vector1_17BE9A58");
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _playerRb = GameManager.instance.playerController.GetComponent<Rigidbody2D>();
        }

        private float _currentBrightness;
        private Coroutine _cr;
        private bool _crRunning;
        
        private void Update()
        {
            int count = 0;
            foreach (var groundedObject in GameManager.instance.playerStatus.groundedObjects)
            {
                if (groundedObject == gameObject) count++;
            }

            if (count >= 2)
            {
                if (!_isHighLight)
                {
                    _cr = StartCoroutine(SetBrightness(_currentBrightness, 0.15f));
                }
                _isHighLight = true;
                _timer = playerContactDuration;
            }

            if (_isHighLight)
            {
                if(Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Controller_B"))
                {
                    if (_isActivated)
                    {
                        Release();
                    }
                    else
                    {
                        Catch();
                    }
                }

                if (_isActivated)
                {
                    if (Mathf.Abs(GameManager.instance.playerController.horizontalInput) >
                        GameManager.instance.playerController.InputDeadzone)
                    {
                        pivot.transform.Rotate(Vector3.forward, GameManager.instance.playerController.horizontalInput * Time.deltaTime * speed);
                    }
                }
                
                TimerUpdate();
            }
        }

        private float _timer;

        private void TimerUpdate()
        {
            _timer -= Time.deltaTime;

            if (_timer > 0) return;
            if (_isHighLight && !_isActivated)
            {
                _cr = StartCoroutine(SetBrightness(_currentBrightness, 0));

                _isHighLight = false;
            }

            if (_timer < -contactLossDuration && _isActivated)
            {
                Release();
            }
        }

        private IEnumerator SetBrightness(float from, float to)
        {
            if(_crRunning)StopCoroutine(_cr);
            _crRunning = true;
            float timer = 0;
            while (timer < 1)
            {
                _currentBrightness = Mathf.Lerp(from, to, timer);
                _renderer.material.SetFloat(Vector148543C7B, _currentBrightness);
                timer += Time.deltaTime;
            
                yield return null;
            }
            _crRunning = false;
        }

        private void Catch()
        {
            _playerRb.drag = 99999;
            _isActivated = true;
            GameManager.instance.playerStatus.Limited = true;
            GameManager.instance.hud.FixedRightStickText("まわす");
            _cr = StartCoroutine(SetBrightness(_currentBrightness, 0.4f));
            _renderer.material.SetFloat(Vector117Be9A58, 0.04f);
        }

        private void Release()
        {
            _playerRb.drag = 2;
            _isActivated = false;
            GameManager.instance.playerStatus.Limited = false;
                        
            GameManager.instance.hud.FixedRightStickText("うごく");
            _cr = StartCoroutine(SetBrightness(_currentBrightness, 0.15f));
            _renderer.material.SetFloat(Vector117Be9A58, 0);
        }
    }
}
