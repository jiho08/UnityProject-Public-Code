using Enemies;
using Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateFromAnimator", story: "[Movement] rotate to [Target] with [Trigger]", category: "Action", id: "b3419cc75b7eeb28fdce63392dd5c3c4")]
public partial class RotateFromAnimatorAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyMovement> Movement;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<EntityAnimationTrigger> Trigger;

    private bool _isRotate;
    
    protected override Status OnStart()
    {
        Trigger.Value.OnManualRotationTrigger += HandleManualRotation;
        return Status.Running;
    }

    private void HandleManualRotation(bool isRotate) => _isRotate = isRotate;
   
    protected override Status OnUpdate()
    {
        if (_isRotate)
        {
            Movement.Value.LookAtTarget(Target.Value.position);
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        Trigger.Value.OnManualRotationTrigger -= HandleManualRotation;
    }
}

