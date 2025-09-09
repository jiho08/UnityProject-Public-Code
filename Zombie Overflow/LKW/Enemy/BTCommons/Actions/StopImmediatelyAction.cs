using Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopImmediately", story: "stop Immediately with [mover]", category: "Action", id: "d380187b2c9f2ff559a8f8f792bdc9be")]
public partial class StopImmediatelyAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyMovement> Mover;

    protected override Status OnStart()
    {
        Mover.Value.StopImmediately();
        return Status.Success;
    }
}

