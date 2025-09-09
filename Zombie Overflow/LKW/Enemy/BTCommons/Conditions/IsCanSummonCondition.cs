using Enemies.BossZombie;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsCanSummon", story: "Check can Summon with [AttackCompo]", category: "Conditions", id: "c9f067f2ae34a5a4676ee9c0135b89af")]
public partial class IsCanSummonCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BossZombieAttackCompo> AttackCompo;

    public override bool IsTrue()
    {
        return Time.time >= AttackCompo.Value.LastSummonTime + AttackCompo.Value.SummonCooldown;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
