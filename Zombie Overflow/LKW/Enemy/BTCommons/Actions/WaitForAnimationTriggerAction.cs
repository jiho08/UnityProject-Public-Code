using Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForAnimationTrigger", story: "wait for [trigger] end", category: "Action", id: "89a64421e33d386ee9547eda00300b2f")]
public partial class WaitForAnimationTriggerAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityAnimationTrigger> Trigger;

    private bool _endTriggerCall;

    protected override Status OnStart()
    {
        Trigger.Value.OnAnimationEndTrigger += HandleAnimationEnd;
        return Status.Running;
    }


    protected override Status OnUpdate()
    {
        return _endTriggerCall ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
        _endTriggerCall = false;
        Trigger.Value.OnAnimationEndTrigger -= HandleAnimationEnd;

    }
    private void HandleAnimationEnd()
    {
        _endTriggerCall = true;
    }
}

