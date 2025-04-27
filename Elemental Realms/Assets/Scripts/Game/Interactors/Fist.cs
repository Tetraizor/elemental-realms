using Game.Controllers;
using Game.Data;
using Game.Entities.Common;
using Game.Entities.Player;
using UnityEngine;

namespace Game.Interactors
{
    public class Fist : InteractorBase
    {
        public bool IsBeingUsed = false;
        private float _cooldown = .6f;
        private float _waitTime = .6f;

        private Animator _fistAnimator;

        private GameObject _renderer;
        private GameObject _orbit;

        public override void StartInteraction(InteractionData data)
        {
            IsBeingUsed = true;
        }

        public override void EndInteraction(InteractionData data)
        {
            if (IsBeingUsed)
            {
                IsBeingUsed = false;
            }
        }

        public override void CancelInteraction(InteractionData data)
        {
            if (IsBeingUsed)
            {
                IsBeingUsed = false;
                _waitTime = _cooldown;
            }
        }

        public override void PullUp(PlayerEntity player)
        {
            base.PullUp(player);

            _waitTime = _cooldown;
        }

        public override void PutDown()
        {
            Destroy(gameObject);
        }

        protected void Start()
        {
            _fistAnimator = GetComponent<Animator>();
            _renderer = transform.Find("Renderer").gameObject;
            _orbit = _renderer.transform.Find("Orbit").gameObject;
        }

        private void Use()
        {
            _waitTime = _cooldown;

            float lookDegrees = Mathf.Atan2(Player.LookDirection.y, Mathf.Abs(Player.LookDirection.x)) * Mathf.Rad2Deg;
            lookDegrees = Mathf.Clamp(lookDegrees, -60, 60);

            _orbit.transform.localEulerAngles = new Vector3(0, 0, lookDegrees);

            _fistAnimator.SetTrigger(Random.Range(0, 1f) > .5f ? "LeftPunch" : "RightPunch");
        }

        private void Update()
        {
            if (IsBeingUsed && _waitTime <= 0)
            {
                Use();
            }

            _waitTime -= Time.deltaTime;
            _waitTime = Mathf.Max(_waitTime, 0);

            if (_waitTime <= 0)
            {
                _orbit.transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            SpeedMultiplier = _waitTime > 0 ? .4f : 1f;
            _renderer.transform.localScale = new Vector3(Player.LookDirection.x > 0 ? 1 : -1, 1, 1);

            _fistAnimator.SetBool("IsMoving", Player.IsMoving);
            _fistAnimator.SetFloat("MoveSpeedMultiplier", Player.GetFinalSpeedMultiplier());
        }

        public override void OnHitEnter(EntityBase entity)
        {
            entity.TakeDamage(new DamageData
            {
                Strength = 10,
                HitDirection = (entity.transform.position - transform.position).normalized
            });

            FindFirstObjectByType<CameraController>().TriggerCameraShake(.1f, .1f);
        }

        public override void OnHitStay(EntityBase entity)
        {
        }

        public override void OnHitExit(EntityBase entity)
        {
        }
    }
}