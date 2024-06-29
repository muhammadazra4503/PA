using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
using AnimationState = Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts.AnimationState;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.ExampleScripts
{
    public class CharacterControls : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private float attackCooldown = 1f;
        private float lastAttackTime;

        public Character Character;
        public CharacterController Controller;
        public BoxCollider BoxCollider;  // Reference to BoxCollider
        public float RunSpeed = 1f;
        public float JumpSpeed = 3f;
        public float CrawlSpeed = 0.25f;
        public float WallJumpSpeed = 5f;
        public float Gravity = -0.2f;
        public ParticleSystem MoveDust;
        public ParticleSystem JumpDust;

        private Vector3 _motion = Vector3.zero;
        private int _inputX, _inputY;
        private float _activityTime;
        private bool _isOnWall = false;
        private bool _isWallJumping = false;
        private bool _isCrawling = false;
        private bool _isStickingToWall = false;

        public LayerMask WallLayer;
        public ProjectilePool projectilePool;  // Reference to the Projectile Pool

        private int _wallStickCount = 0;
        private const int _maxWallSticks = 2;

        private void Start()
        {
            Character.SetState(AnimationState.Idle);
            Respawn();
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.A) && Time.time > lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                Character.Animator.SetTrigger("Attack");
                ShootProjectile();
            }

            if (Input.GetKeyDown(KeyCode.J)) Character.Animator.SetTrigger("Jab");
            else if (Input.GetKeyDown(KeyCode.P)) Character.Animator.SetTrigger("Push");
            else if (Input.GetKeyDown(KeyCode.H)) Character.Animator.SetTrigger("Hit");
            else if (Input.GetKeyDown(KeyCode.I)) { Character.SetState(AnimationState.Idle); _activityTime = 0; }
            else if (Input.GetKeyDown(KeyCode.R)) { Character.SetState(AnimationState.Ready); _activityTime = Time.time; }
            else if (Input.GetKeyDown(KeyCode.B)) Character.SetState(AnimationState.Blocking);
            else if (Input.GetKeyUp(KeyCode.B)) Character.SetState(AnimationState.Ready);
            else if (Input.GetKeyDown(KeyCode.D)) Character.SetState(AnimationState.Dead);
            else if (Input.GetKeyDown(KeyCode.S)) Character.Animator.SetTrigger("Slash");
            else if (Input.GetKeyDown(KeyCode.O)) Character.Animator.SetTrigger("Shot");
            else if (Input.GetKeyDown(KeyCode.F)) Character.Animator.SetTrigger("Fire1H");
            else if (Input.GetKeyDown(KeyCode.E)) Character.Animator.SetTrigger("Fire2H");
            else if (Input.GetKeyDown(KeyCode.C)) Character.SetState(AnimationState.Climbing);
            else if (Input.GetKeyUp(KeyCode.C)) Character.SetState(AnimationState.Ready);
            else if (Input.GetKeyUp(KeyCode.L)) Character.Blink();

            if (Controller.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    GetDown();
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    GetUp();
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _inputX = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _inputX = 1;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _inputY = 1;

                if (Controller.isGrounded)
                {
                    JumpDust.Play(true);
                }
                else if (_isOnWall)
                {
                    if (_inputX != 0)  // Only allow jumping to the side when on the wall
                    {
                        _isWallJumping = true;
                        _motion = new Vector3(WallJumpSpeed * _inputX, JumpSpeed);
                        _isOnWall = false;
                        Character.SetState(AnimationState.Jumping);
                    }
                }
            }

            if (Input.GetKey(KeyCode.Z) && _isOnWall && _wallStickCount < _maxWallSticks)
            {
                StartStickingToWall();
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                StopStickingToWall();
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (Time.frameCount <= 1)
            {
                Controller.Move(new Vector3(0, Gravity) * Time.fixedDeltaTime);
                return;
            }

            var state = Character.GetState();

            if (state == AnimationState.Dead)
            {
                if (_inputX == 0) return;

                Character.SetState(AnimationState.Running);
            }

            if (_inputX != 0)
            {
                Turn(_inputX);
            }

            if (Controller.isGrounded)
            {
                _isOnWall = false;
                _isWallJumping = false;
                _wallStickCount = 0;  // Reset the wall stick count when grounded

                if (state == AnimationState.Jumping)
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        GetDown();
                    }
                    else
                    {
                        Character.Animator.SetTrigger("Landed");
                        Character.SetState(AnimationState.Ready);
                        JumpDust.Play(true);
                    }
                }

                _motion = _isCrawling
                    ? new Vector3(CrawlSpeed * _inputX, 0)
                    : new Vector3(RunSpeed * _inputX, JumpSpeed * _inputY);

                if (_inputX != 0 || _inputY != 0)
                {
                    if (_inputY > 0)
                    {
                        Character.SetState(AnimationState.Jumping);
                    }
                    else
                    {
                        switch (state)
                        {
                            case AnimationState.Idle:
                            case AnimationState.Ready:
                                Character.SetState(AnimationState.Running);
                                break;
                        }
                    }
                }
                else
                {
                    switch (state)
                    {
                        case AnimationState.Crawling:
                        case AnimationState.Climbing:
                        case AnimationState.Blocking:
                            break;
                        default:
                            var targetState = Time.time - _activityTime > 5 ? AnimationState.Idle : AnimationState.Ready;

                            if (state != targetState)
                            {
                                Character.SetState(targetState);
                            }

                            break;
                    }
                }
            }
            else
            {
                if (!_isWallJumping)
                {
                    CheckForWall();
                }

                _motion = new Vector3(RunSpeed * _inputX, _motion.y);
                Character.SetState(AnimationState.Jumping);
            }

            _motion.y += Gravity;

            Controller.Move(_motion * Time.fixedDeltaTime);

            Character.Animator.SetBool("Grounded", Controller.isGrounded);
            Character.Animator.SetBool("Moving", Controller.isGrounded && _inputX != 0);
            Character.Animator.SetBool("Falling", !Controller.isGrounded && Controller.velocity.y < 0);

            if (_inputX != 0 || _inputY != 0 || Character.Animator.GetBool("Action"))
            {
                _activityTime = Time.time;
            }

            _inputX = _inputY = 0;

            if (Controller.isGrounded && !Mathf.Approximately(Controller.velocity.x, 0))
            {
                var velocity = MoveDust.velocityOverLifetime;

                velocity.xMultiplier = 0.2f * -Mathf.Sign(Controller.velocity.x);

                if (!MoveDust.isPlaying)
                {
                    MoveDust.Play();
                }
            }
            else
            {
                MoveDust.Stop();
            }
        }

        private void Turn(int direction)
        {
            var scale = Character.transform.localScale;
            scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);
            Character.transform.localScale = scale;
        }

        private void GetDown()
        {
            Character.Animator.SetTrigger("GetDown");
        }

        private void GetUp()
        {
            Character.Animator.SetTrigger("GetUp");
        }

        private void CheckForWall()
        {
            RaycastHit hit;
            if (Physics.Raycast(Character.transform.position, Character.transform.right * _inputX, out hit, 0.1f, WallLayer))
            {
                _isOnWall = true;
                _motion = Vector3.zero;
                Character.SetState(AnimationState.Idle);
            }
            else
            {
                _isOnWall = false;
            }
        }

        private void StartStickingToWall()
        {
            _isStickingToWall = true;
            _wallStickCount++;  // Increment the wall stick count
            _motion = Vector3.zero;  // Stop the character's movement
            Character.SetState(AnimationState.Climbing);  // Assuming climbing is the sticking state
        }

        private void StopStickingToWall()
        {
            _isStickingToWall = false;
            Character.SetState(AnimationState.Ready);
        }

        private void ShootProjectile()
        {
            GameObject projectile = projectilePool.GetProjectile();
            projectile.transform.position = projectileSpawnPoint.position;
            float direction = Character.transform.localScale.x > 0 ? 1 : -1;
            projectile.GetComponent<Projectile>().SetDirection(direction);
        }

        public void Respawn()
        {
            transform.position = spawnPoint.position;
            Character.SetState(AnimationState.Ready);
            Character.Animator.SetTrigger("Idle");
            var health = GetComponent<Health>();
            if (health != null)
            {
                health.ResetHealth();
            }
        }
    }
}
