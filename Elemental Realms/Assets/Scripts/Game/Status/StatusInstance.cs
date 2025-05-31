using System;
using UnityEngine;

namespace Game.Status
{
    [Serializable]
    public class StatusInstance
    {
        public StatusBaseSO Status;

        [HideInInspector] public StatusData StatusData;

        public void Tick(float deltaTime, StatusManager target) => Status.Tick(deltaTime, target, this);
        public void Inflict(StatusManager target) => Status.Inflict(target, this);
        public void Reapply(StatusManager target) => Status.Reapply(target, this);
        public void Finish(StatusManager target) => Status.Finish(target, this);
    }
}