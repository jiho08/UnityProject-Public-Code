using Animations;
using Combat;
using Entities;
using FSM;
using UnityEngine;

namespace Players.States
{
    public class PlayerTopAttackState : PlayerTopState
    {
        public PlayerTopAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if (!attackCompo.AttemptAttack())
            {
                player.ChangeState("IDLE", stateType);
                return;
            }
            splitRenderer.Attack();
        }

        public override void Update()
        {
            base.Update();
            if (isTriggerCall)
            {
                player.ChangeState("IDLE", stateType);
            }
        }
    }
}