using Enemies.BossZombie;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetSummonTime", story: "Set summonTime with [AttackCompo]", category: "Action", id: "8e79ec89706e7498dc60a52ccba58863")]
public partial class SetSummonTimeAction : Action
{
    [SerializeReference] public BlackboardVariable<BossZombieAttackCompo> AttackCompo;

    protected override Status OnStart()
    {
        AttackCompo.Value.SetSummonTime();
        return Status.Success;
    }
}

