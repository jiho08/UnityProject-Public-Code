using Enemies.SniperZombie;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TargetTracking", story: "Tracking Target with [AttackCompo]", category: "Action", id: "e0d29cbbe5399b26685620015b506bf0")]
public partial class TargetTrackingAction : Action
{
    [SerializeReference] public BlackboardVariable<SniperZombieAttackCompo> AttackCompo;

    protected override Status OnStart()
    {
        AttackCompo.Value.StartTracking();
        return Status.Success;
    }

   
}

