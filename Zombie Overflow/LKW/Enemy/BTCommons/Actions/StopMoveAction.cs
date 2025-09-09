using Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopMove", story: "[Mover] stop set to [newValue]", category: "Action", id: "e2f95226b591e1abc45376feb41ac0ed")]
public partial class StopMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyMovement> Mover;
    [SerializeReference] public BlackboardVariable<bool> NewValue;

    protected override Status OnStart()
    {
        Mover.Value.SetStop(NewValue.Value);
        return Status.Success;
    }

}

