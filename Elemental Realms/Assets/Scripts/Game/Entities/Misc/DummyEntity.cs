using Game.Data;
using Game.Entities.Common;
using Game.Interactions;
using UnityEngine;

namespace Game.Entities.Misc
{
    [RequireComponent(typeof(Animator))]
    public class DummyEntity : Entity, IInteractionContextConsumer
    {
        [SerializeField] private ParticleSystem _hitParticle;

        protected override void Awake()
        {
            base.Awake();
        }

        public void ConsumeContext(InteractionContext ctx)
        {
            var particleEffect = Instantiate(_hitParticle, transform.position + new Vector3(0, 1), Quaternion.identity);

            float degrees = Mathf.Atan2(ctx.HitDirection.y, ctx.HitDirection.x) * Mathf.Rad2Deg;
            particleEffect.gameObject.transform.localEulerAngles = new Vector3(0, 0, degrees);

            Destroy(particleEffect.gameObject, 1);

            GetComponent<Animator>().SetTrigger("Hit");
        }
    }
}