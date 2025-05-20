using DG.Tweening;
using Game.Enum;
using Game.Modifiers;
using UnityEngine;

namespace Game.Status
{
    [CreateAssetMenu(fileName = "FrozenStatusEffect", menuName = "Effects/Status/Frozen", order = 1)]
    public class FrozenStatusSO : StatusBaseSO, ISpeedModifier
    {
        [SerializeField] private GameObject _frozenParticles;
        [SerializeField] private GameObject _frozenShell;

        public const float LIFETIME = 4;
        public const float DAMAGE_INTERVAL = 1;
        public const float FROZEN_DAMAGE = 5;

        public override void Inflict(StatusManager target, StatusInstance statusInstance)
        {
            var statusData = new FrozenStatusData();
            statusInstance.StatusData = statusData;

            var frozenParticle = Instantiate(_frozenParticles, target.Owner.gameObject.transform).GetComponent<ParticleSystem>();
            var frozenShell = Instantiate(_frozenShell, target.Owner.gameObject.transform);
            frozenShell.transform.localPosition = new Vector2(0, -0.2f);
            frozenShell.transform.localScale = Vector3.zero;
            frozenShell.transform.DOScale(Vector3.one, .2f);

            frozenParticle.transform.localPosition = Vector3.zero;
            frozenParticle.transform.localEulerAngles = Vector3.zero;

            statusData.FrozenParticles = frozenParticle;
            statusData.FrozenShell = frozenShell;
        }

        public override void Tick(float deltaTime, StatusManager target, StatusInstance statusInstance)
        {
            var statusData = statusInstance.StatusData as FrozenStatusData;

            if (!statusData.IsActive) return;

            statusData.Lifetime += deltaTime;
            statusData.Time += deltaTime;

            if (statusData.Lifetime >= LIFETIME)
            {
                target.RemoveStatus(statusInstance);
            }

            if (statusData.Time > DAMAGE_INTERVAL)
            {
                statusData.Time = 0;
                target.Owner.Health.TakeDamage(FROZEN_DAMAGE, DamageType.Cold);
            }
        }

        public override void Finish(StatusManager target, StatusInstance statusInstance)
        {
            var statusData = statusInstance.StatusData as FrozenStatusData;

            statusData.IsActive = false;

            statusData.FrozenParticles.transform.parent = null;
            statusData.FrozenShell.transform.parent = null;
            statusData.FrozenShell.transform.DOScale(Vector3.zero, .2f);

            statusData.FrozenParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

            Destroy(statusData.FrozenParticles.gameObject, 3);
            Destroy(statusData.FrozenShell.gameObject, 1);
        }

        public override void Reapply(StatusManager target, StatusInstance statusInstance)
        {
            var statusData = statusInstance.StatusData as FrozenStatusData;
            statusData.Lifetime = 0;
        }

        public float GetSpeedModifier() => 0;
    }

    public class FrozenStatusData : StatusData
    {
        public ParticleSystem FrozenParticles;
        public GameObject FrozenShell;
        public float Lifetime = 0;
        public float Time = 0;

        public bool IsActive = true;
    }
}