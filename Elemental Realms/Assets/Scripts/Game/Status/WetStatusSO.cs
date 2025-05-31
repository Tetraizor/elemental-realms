using Game.Enum;
using UnityEngine;

namespace Game.Status
{
    [CreateAssetMenu(fileName = "WetStatusEffect", menuName = "Effects/Status/Wet", order = 1)]
    public class WetStatusSO : StatusBaseSO
    {
        [SerializeField] private GameObject _wetParticles;

        public const float LIFETIME = 10;

        public override void Inflict(StatusManager target, StatusInstance statusInstance)
        {
            var statusData = new WetStatusData();
            statusInstance.StatusData = statusData;

            var wetParticle = Instantiate(_wetParticles, target.Owner.gameObject.transform).GetComponent<ParticleSystem>();

            wetParticle.transform.localPosition = Vector3.zero;
            wetParticle.transform.localEulerAngles = Vector3.zero;

            statusData.WetParticles = wetParticle;
        }

        public override void Tick(float deltaTime, StatusManager target, StatusInstance statusInstance)
        {
            var statusData = statusInstance.StatusData as WetStatusData;

            if (!statusData.IsActive) return;

            statusData.Lifetime += deltaTime;

            if (statusData.Lifetime >= LIFETIME)
            {
                target.RemoveStatus(statusInstance);
            }
        }

        public override void Finish(StatusManager target, StatusInstance statusInstance)
        {
            var statusData = statusInstance.StatusData as WetStatusData;

            statusData.IsActive = false;

            statusData.WetParticles.transform.parent = null;

            statusData.WetParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

            Destroy(statusData.WetParticles.gameObject, 3);
        }

        public override void Reapply(StatusManager target, StatusInstance statusInstance)
        {
            var statusData = statusInstance.StatusData as WetStatusData;
            statusData.Lifetime = 0;
        }
    }

    public class WetStatusData : StatusData
    {
        public ParticleSystem WetParticles;
        public float Lifetime = 0;

        public bool IsActive = true;
    }
}