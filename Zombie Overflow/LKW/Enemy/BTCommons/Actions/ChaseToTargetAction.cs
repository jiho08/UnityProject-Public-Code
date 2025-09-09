using Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseToTarget", story: "chase to [target] with [mover]", category: "Action", id: "f1110adc3dec520f187a4ca5386a4cac")]
public partial class ChaseToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<EnemyMovement> Mover;

    protected override Status OnStart()
    {
        Mover.Value.SetDestination(Target.Value.position);
        return Status.Success;
    }

}

