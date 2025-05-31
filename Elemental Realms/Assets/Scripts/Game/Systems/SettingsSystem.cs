using System.Collections;
using DG.Tweening;
using Tetraizor.Bootstrap.Base;
using Tetraizor.MonoSingleton;
using UnityEngine;

namespace Game.Systems
{
    public class SettingsSystem : MonoSingleton<SettingsSystem>, IPersistentSystem
    {
        public string GetName() => "SettingsSystem";

        public IEnumerator LoadSystem()
        {
            yield return null;

            DOTween.onWillLog += (logType, obj) => false;
        }
    }
}