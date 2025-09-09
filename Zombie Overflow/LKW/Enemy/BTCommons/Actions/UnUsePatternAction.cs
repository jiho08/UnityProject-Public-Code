using Enemies;
using Enemies.BossZombie;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UnUsePattern", story: "UnUse Pattern with [AttackCompo]", category: "Action", id: "87e4332dffd46efd2c5a47fa184d0217")]
public partial class UnUsePatternAction : Action
{
    [SerializeReference] public BlackboardVariable<BossZombieAttackCompo> AttackCompo;

    protected override Status OnStart()
    {
        AttackCompo.Value.DisablePattern();
        return Status.Success;
    }

}

