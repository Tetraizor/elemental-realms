using Game.Enum;
using UnityEngine;

namespace Game.Status
{
    [CreateAssetMenu(fileName = "OnFireEffect", menuName = "Effects/Status/On Fire", order = 0)]
    public class OnFireStatusSO : StatusBaseSO
    {
        [SerializeField] private GameObject _fireParticles;

        public const float LIFETIME = 5;
        public const float FIRE_DAMAGE = 5;
        public const float DAMAGE_INTERVAL = 1;

        public override void Inflict(StatusManager target, StatusInstance statusInstance)
        {
            var statusData = new OnFireStatusData();
            statusInstance.StatusData = statusData;

            var fire = Instantiate(_fireParticles, target.Owner.gameObject.transform).GetComponent<ParticleSystem>();

            fire.transform.localPosition = Vector3.zero;
            fire.transform.localEulerAngles = Vector3.zero;

            statusData.FireParticles = fire;
        }

        public override void Tick(float deltaTime, StatusManager target, StatusInstance statusInstance)
        {
            var statusData = statusInstance.StatusData as OnFireStatusData;

            if (!statusData.IsActive) return;

            statusData.Timer += deltaTime;
            statusData.Lifetime += deltaTime;

            if (statusData.Timer >= DAMAGE_INTERVAL)
            {
                statusData.Timer = 0;
                target.Owner.Health.TakeDamage(FIRE_DAMAGE, DamageType.Fire);
            }

            if (statusData.Lifetime >= LIFETIME)
            {
                target.RemoveStatus(statusInstance);
            }
        }

        public override void Finish(StatusManager target, StatusInstance statusInstance)
        {
            var statusData = statusInstance.StatusData as OnFireStatusData;

            statusData.IsActive = false;

            statusData.FireParticles.transform.parent = null;

            statusData.FireParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

            Destroy(statusData.FireParticles.gameObject, 3);
        }

        public override void Reapply(StatusManager target, StatusInstance statusInstance)
        {
            var statusData = statusInstance.StatusData as OnFireStatusData;
            statusData.Lifetime = 0;
        }
    }

    public class OnFireStatusData : StatusData
    {
        public ParticleSystem FireParticles;
        public float Timer = 0;
        public float Lifetime = 0;

        public bool IsActive = true;
    }
}