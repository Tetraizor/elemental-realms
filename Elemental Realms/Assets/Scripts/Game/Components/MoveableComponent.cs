using System.Collections.Generic;
using System.Linq;
using Game.Entities.Common;
using Game.Enum;
using Game.Modifiers;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Entity))]
    public class MoveableComponent : MonoBehaviour
    {
        [SerializeField] protected float _baseSpeed = 1;
        public float BaseSpeed => _baseSpeed;
        private float _baseSpeedMultiplier = 1;

        private List<ISpeedModifier> _speedModifiers = new();

        public Vector2 MovementDirection { get; internal set; } = Vector2.zero;
        public Vector2 LookDirection { get; internal set; } = Vector2.zero;

        private Animator _moveableAnimator;
        private GameObject _moveableRenderer;
        private Rigidbody2D _moveableRigidbody;

        public bool CanMove = true;

        public MoveableDirectionMode DirectionMode = MoveableDirectionMode.SetByLookVector;

        public bool IsMoving => _moveableRigidbody?.linearVelocity.magnitude > .1f;

        private void Awake()
        {
            _moveableRigidbody = GetComponent<Rigidbody2D>();
            _moveableAnimator = GetComponent<Animator>();
            _moveableRenderer = transform.Find("Renderer").gameObject;
        }

        private void Start()
        {
            _speedModifiers = GetComponentsInChildren<ISpeedModifier>(true).ToList();
        }

        public void RegisterSpeedModifier(ISpeedModifier modifier)
        {
            if (!_speedModifiers.Contains(modifier)) _speedModifiers.Add(modifier);
        }

        public void DeregisterSpeedModifier(ISpeedModifier modifier)
        {
            if (_speedModifiers.Contains(modifier)) _speedModifiers.Remove(modifier);
        }

        public virtual void Move()
        {
            if (CanMove)
            {
                if (MovementDirection.magnitude > 0.1f)
                {
                    _moveableRigidbody.AddForce(MovementDirection.normalized * BaseSpeed * Time.fixedDeltaTime * 2500 * _moveableRigidbody.mass * GetFinalSpeedMultiplier());
                }
            }
        }

        private void FixedUpdate()
        {
            if (MovementDirection.magnitude > 0.1f) Move();

            var referenceVector = DirectionMode == MoveableDirectionMode.SetByMovementVector ? MovementDirection : LookDirection;

            if (Mathf.Abs(referenceVector.normalized.x) > .1f)
            {
                _moveableRenderer.transform.localScale = new Vector3(referenceVector.x > 0 ? 1 : -1, 1, 1);
            }
        }

        public void SetBaseSpeedMultiplier(float speedMultiplier)
        {
            _baseSpeedMultiplier = speedMultiplier;
        }

        public float GetFinalSpeedMultiplier()
        {
            float externalSpeedMultiplier = 1;

            _speedModifiers.ForEach((modifier) => externalSpeedMultiplier *= modifier.GetSpeedModifier());

            return _baseSpeedMultiplier * externalSpeedMultiplier;
        }

        private void Update()
        {
            _moveableAnimator.SetFloat("SpeedMultiplier", GetFinalSpeedMultiplier());

            Debug.DrawLine(transform.position, transform.position + (Vector3)MovementDirection, Color.blue);
            Debug.DrawLine(transform.position, transform.position + (Vector3)LookDirection, Color.red);
        }
    }
}