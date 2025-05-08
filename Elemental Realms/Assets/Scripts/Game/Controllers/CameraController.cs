using System.Collections;
using UnityEngine;

namespace Game.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _baseOffset;
        private Vector2 _shakeOffset;

        private void FixedUpdate()
        {
            Vector2 lookOffset = (new Vector2(UnityEngine.Input.mousePosition.x / Screen.width, UnityEngine.Input.mousePosition.y / Screen.height) * 2) - Vector2.one;

            transform.position = _target.position + _baseOffset + (Vector3)_shakeOffset + (Vector3)lookOffset * 2;
        }

        public void TriggerCameraShake(float magnitude, float duration)
        {
            StartCoroutine(ShakeCamera(magnitude, duration));
        }

        private IEnumerator ShakeCamera(float magnitude, float duration)
        {
            float shakeTimeLeft = duration;

            while (shakeTimeLeft > 0)
            {
                shakeTimeLeft -= Time.deltaTime;
                yield return null;

                float magnitudeMultiplier = 1 - (shakeTimeLeft / duration);

                _shakeOffset = new Vector2(
                    Random.Range(magnitude * magnitudeMultiplier * -1, magnitude * magnitudeMultiplier),
                    Random.Range(magnitude * magnitudeMultiplier * -1, magnitude * magnitudeMultiplier)
                );
            }
        }
    }
}