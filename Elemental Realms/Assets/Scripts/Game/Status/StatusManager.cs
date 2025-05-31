using System.Collections.Generic;
using System.Linq;
using Game.Entities.Common;
using UnityEngine;

namespace Game.Status
{
    public class StatusManager
    {
        public List<StatusInstance> Statuses { get; private set; } = new();
        private List<StatusInstance> _toRemove = new();

        public Entity Owner { get; private set; }

        public StatusManager(Entity owner) => Owner = owner;

        public void AddStatus(StatusInstance statusInstance)
        {
            if (HasStatus(statusInstance.Status))
            {
                Statuses.Find(status => status.Status == statusInstance.Status)?.Reapply(this);
                return;
            }

            if ((statusInstance.Status.Type & ~Owner.ResistantEffects) != statusInstance.Status.Type) return;

            Statuses.Add(statusInstance);
            statusInstance.Inflict(this);
        }

        public bool HasStatus(StatusBaseSO statusSO) => Statuses.Any(si => si.Status == statusSO);

        public void TickStatus(float deltaTime)
        {
            Statuses.ForEach(status => status.Tick(deltaTime, this));

            if (_toRemove.Count > 0)
            {
                foreach (var rem in _toRemove)
                {
                    if (Statuses.Remove(rem))
                        rem.Finish(this);
                }

                _toRemove.Clear();
            }
        }

        public void RemoveStatus(StatusInstance statusInstance)
        {
            if (!Statuses.Contains(statusInstance)) return;

            if (!_toRemove.Contains(statusInstance))
                _toRemove.Add(statusInstance);
        }
    }
}