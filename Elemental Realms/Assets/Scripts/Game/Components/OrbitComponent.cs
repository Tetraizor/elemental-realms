using Game.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Components
{
    public class OrbitComponent : MonoBehaviour
    {
        [SerializeField] private float _maxAngle = 80;

        public Transform TargetTransform
        {
            get => _targetTransform;
            set
            {
                _targetTransform = value;
            }
        }
        [SerializeField] private Transform _targetTransform;

        public Vector2 Direction
        {
            get => _direction;
            set
            {
                _direction = value;

                if (CanDirectionChange && _targetTransform != null)
                {
                    float angle = Mathf.Atan2(_direction.y, Mathf.Abs(_direction.x)) * Mathf.Rad2Deg;
                    angle = Mathf.Clamp(angle, _maxAngle * -1, _maxAngle);
                    _targetTransform.localEulerAngles = new Vector3(0, 0, angle);
                }
            }
        }
        private Vector2 _direction = Vector2.zero;

        public bool CanDirectionChange = true;
    }
}