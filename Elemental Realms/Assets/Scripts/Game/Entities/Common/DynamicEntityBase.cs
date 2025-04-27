using UnityEngine;

namespace Game.Entities.Common
{
    [RequireComponent(typeof(Animator))]
    public class DynamicEntityBase : EntityBase
    {
        [Header("Dynamic Base Properties")]
        // Speed
        [SerializeField] protected float _baseSpeed = 1;
        public float BaseSpeed => _baseSpeed;
        private float _speedMultiplier = 1;

        public Vector2 MovementDirection { get; internal set; } = Vector2.zero;
        public Vector2 LookDirection { get; internal set; } = Vector2.zero;

        public Animator EntityAnimator { private set; get; }

        public bool IsMoving => EntityRigidbody.linearVelocity.magnitude > .1f;

        protected override void Awake()
        {
            base.Awake();

            EntityAnimator = GetComponent<Animator>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (MovementDirection.magnitude > 0.1f) Move();
        }

        public virtual void Move()
        {
            if (MovementDirection.magnitude > 0.1f)
            {
                EntityRigidbody.AddForce(MovementDirection.normalized * BaseSpeed * Time.fixedDeltaTime * 2500 * EntityRigidbody.mass * GetFinalSpeedMultiplier());

                if (Mathf.Abs(LookDirection.normalized.x) > .1f)
                {
                    _renderer.transform.localScale = new Vector3(LookDirection.x > 0 ? 1 : -1, 1, 1);
                }
            }
        }

        public virtual float GetFinalSpeedMultiplier()
        {
            return _speedMultiplier;
        }

        public virtual void SetSpeedMultiplier(float speedMultiplier)
        {
            _speedMultiplier = speedMultiplier;
        }

        protected override void Update()
        {
            base.Update();

            EntityAnimator.SetFloat("SpeedMultiplier", GetFinalSpeedMultiplier());

            Debug.DrawLine(transform.position, transform.position + (Vector3)MovementDirection, Color.blue);
            Debug.DrawLine(transform.position, transform.position + (Vector3)LookDirection, Color.red);
        }
    }
}