using System;
using Animations;
using Core.Define;
using Players;
using UnityEngine;
using UnityEngine.Serialization;
using StructDefine = Core.StructDefine;

namespace Entities
{
    public class SplitEntityRenderer : EntityRenderer, IPlayerComponent
    {
        [SerializeField] private Animator topAnimator;
        [SerializeField] private Animator bottomAnimator;
        [SerializeField] private StructDefine.CharacterRenderData[] characterRenderDatas;
        
        [SerializeField] private AnimParamSO moveDirXParam;
        [SerializeField] private AnimParamSO moveDirZParam;
        [SerializeField] private AnimParamSO attackParam;
        [SerializeField] private AnimParamSO dodgeParam;
        
        public void SetUpPlayer(CharacterSO character)
        {
            var playerType = character.playerType;
            foreach (var data in characterRenderDatas)
            {
                if (data.playerType == playerType)
                {
                    data.characterRender.SetActive(true);
                    data.gunRender.SetActive(true);
                }
                else
                {
                    data.characterRender.SetActive(false);
                    data.gunRender.SetActive(false);
                }
            }
        }
        
        public void SetMoveDir(Vector2 dir)
        {
            SetParam(moveDirXParam.hashValue, dir.x, EnumDefine.StateType.Bottom);
            SetParam(moveDirZParam.hashValue, dir.y, EnumDefine.StateType.Bottom);
        }
        
        public void Attack() => SetParam(attackParam.hashValue, EnumDefine.StateType.Top);
        public void Dodge() => SetParam(dodgeParam.hashValue, EnumDefine.StateType.Bottom);

        #region SetParams
        public void SetParam(int hash, float value, EnumDefine.StateType stateType)
        {
            switch (stateType)
            {
                case EnumDefine.StateType.Top:
                    topAnimator.SetFloat(hash, value);
                    break;
                case EnumDefine.StateType.Bottom:
                    bottomAnimator.SetFloat(hash, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null);
            }
        }

        public void SetParam(int hash, bool value, EnumDefine.StateType stateType)
        {
            switch (stateType)
            {
                case EnumDefine.StateType.Top:
                    topAnimator.SetBool(hash, value);
                    break;
                case EnumDefine.StateType.Bottom:
                    bottomAnimator.SetBool(hash, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null);
            }
        }

        public void SetParam(int hash, int value, EnumDefine.StateType stateType)
        {
            switch (stateType)
            {
                case EnumDefine.StateType.Top:
                    topAnimator.SetInteger(hash, value);
                    break;
                case EnumDefine.StateType.Bottom:
                    bottomAnimator.SetInteger(hash, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null);
            }
        }

        public void SetParam(int hash, EnumDefine.StateType stateType)
        {
            switch (stateType)
            {
                case EnumDefine.StateType.Top:
                    topAnimator.SetTrigger(hash);
                    break;
                case EnumDefine.StateType.Bottom:
                    bottomAnimator.SetTrigger(hash);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null);
            }
        }
        #endregion

    }
}