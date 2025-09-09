using System;
using System.Linq;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Combat.Skills.ShowDown
{
    public class ShowdownPP : MonoBehaviour
    {
        [SerializeField] private VolumeProfile globalVolumeProfile;
        [SerializeField] private VolumeProfile showdownVolumeProfile;
        [SerializeField] private float transitionDuration = 0.5f;
        private VolumeProfile _initialVolume;

        private void Awake()
        {
            foreach (var fromCompo in showdownVolumeProfile.components)
            {
                var toCompo = globalVolumeProfile.components.Find(x => x.GetType() == fromCompo.GetType());
                if (toCompo == null)
                {
                    globalVolumeProfile.Add(fromCompo.GetType(), true);
                }
            }

            _initialVolume = ScriptableObject.CreateInstance<VolumeProfile>();
            foreach (var compo in globalVolumeProfile.components)
            {
                var compoInShowdown = _initialVolume.Add(compo.GetType());
                foreach (var over in compo.parameters.Where(p => p.overrideState))
                {
                    var def = compoInShowdown.parameters.FirstOrDefault(x => x.GetType() == over.GetType());
                    if (def != null) def.overrideState = true;
                }

                compo.Override(compoInShowdown, 1);
            }
        }

        public void ChangeVolume(bool isShowdown)
        {
            InterpVolumeProfile(isShowdown ? showdownVolumeProfile : _initialVolume);
        }

        private void InterpVolumeProfile(VolumeProfile profile) => DOVirtual.Float(0f, 1f, transitionDuration, t =>
            SetGlobalVolumeProfile(profile, t)).SetEase(Ease.OutCubic);

        private void SetGlobalVolumeProfile(VolumeProfile profile, float interp)
        {
            foreach (var compo in profile.components)
            {
                var compoInGlobal = globalVolumeProfile.components.Find(x => x.GetType() == compo.GetType());
                if (compoInGlobal != null)
                {
                    compo.Override(compoInGlobal, interp);
                }
            }
        }
    }
}