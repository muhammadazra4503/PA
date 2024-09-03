using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
using AnimationState = Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts.AnimationState;
using UnityEngine.UI;
using TMPro;

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
        public BoxCollider BoxCollider;
        public float RunSpeed = 1f;
        public float JumpSpeed = 3f;
        public float WallJumpSpeed = 5f;
        public float Gravity = -0.2f;
        public ParticleSystem MoveDust;
        public ParticleSystem JumpDust;

        private Vector3 _motion = Vector3.zero;
        private int _inputX, _inputY;
        private float _activityTime;
        private bool _isOnWall = false;
        private bool _isWallJumping = false;

        [SerializeField] private int maxRespawns = 3;
        private int remainingRespawns;
        [SerializeField] private TMP_Text respawnText;

        [SerializeField] private Image cooldownImage;
        [SerializeField] private TMP_Text cooldownText;

        public LayerMask WallLayer;
        public ProjectilePool projectilePool;

        private int _wallStickCount = 0;
        private const int _maxWallSticks = 2;

        private UIManager uiManager;

        // Reference to the virtual joystick
        [SerializeField] private VariableJoystick joystick;

        // Reference to the attack button
        [SerializeField] private Button attackButton;

        private void Start()
        {
            Character.SetState(AnimationState.Idle);
            remainingRespawns = maxRespawns;
            UpdateRespawnText();
            InitialRespawn();

            uiManager = FindObjectOfType<UIManager>();

            lastAttackTime = -attackCooldown;

            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = 0;
            }

            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(false); 
            }

            // Attach the attack function to the button
            if (attackButton != null)
            {
                attackButton.onClick.AddListener(Attack);
            }
        }

        private void Update()
        {
            HandleInput();
            UpdateCooldownIndicator();
        }

        private void HandleInput()
        {
            // Joystick input for movement
            _inputX = Mathf.RoundToInt(joystick.Horizontal);
            _inputY = Mathf.RoundToInt(joystick.Vertical);

            if (_inputY > 0 && Controller.isGrounded)
            {
                JumpDust.Play(true);
            }
            else if (_inputY > 0 && _isOnWall)
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
                _wallStickCount = 0;  

                if (state == AnimationState.Jumping)
                {
                    Character.Animator.SetTrigger("Landed");
                    Character.SetState(AnimationState.Ready);
                    JumpDust.Play(true);
                }

                _motion = new Vector3(RunSpeed * _inputX, JumpSpeed * _inputY);

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

        private void UpdateCooldownIndicator()
        {
            if (cooldownImage != null && cooldownText != null)
            {
                float cooldownRemaining = Mathf.Max(0, lastAttackTime + attackCooldown - Time.time);
                float cooldownFraction = cooldownRemaining / attackCooldown;

                cooldownImage.fillAmount = cooldownFraction;

                if (cooldownRemaining > 0)
                {
                    cooldownText.gameObject.SetActive(true); 
                    cooldownText.text = Mathf.Ceil(cooldownRemaining).ToString();
                }
                else
                {
                    cooldownText.gameObject.SetActive(false); 
                }
            }
        }

        // Method to trigger the attack when the button is pressed
        private void Attack()
        {
            if (Time.time > lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                Character.Animator.SetTrigger("Attack");
                ShootProjectile();
            }
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
            if (remainingRespawns > 0)
            {
                remainingRespawns--;
                UpdateRespawnText();
                if (remainingRespawns == 0)
                {
                    // No more respawns left
                    Debug.Log("No more respawns left.");
                    if (uiManager != null)
                    {
                        uiManager.GameOver();
                    }
                }
                else
                {
                    // Respawn the character
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

        private void InitialRespawn()
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

        private void UpdateRespawnText()
        {
            if (respawnText != null)
            {
                respawnText.text = "" + remainingRespawns;
            }
        }
    }
}
