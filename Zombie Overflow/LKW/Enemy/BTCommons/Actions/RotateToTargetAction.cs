using Entities;
using System;
using Enemies;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateToTarget", story: "[Mover] rotate to [Target]", category: "Action", id: "24deab5fba79cc284418409f37ce6f97")]
public partial class RotateToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyMovement> Mover;
    [SerializeReference] public BlackboardVariable<Transform> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    protected override Status OnUpdate()
    {
        if (LookTargetSmoothly())
        {
            return Status.Success;
        }
            
        return Status.Running;
    }

    private bool LookTargetSmoothly()
    {
        Quaternion targetRotation = Mover.Value.LookAtTarget(Target.Value.position,true);
        const float angleThreshold = 5f;
        return Quaternion.Angle(targetRotation,Self.Value.rotation ) < angleThreshold;
    }
}

