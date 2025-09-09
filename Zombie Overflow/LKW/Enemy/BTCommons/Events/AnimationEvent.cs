using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/AnimationEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "AnimationEvent", message: "change animation to [next]", category: "Events", id: "f5123adee92a113436248b8a60693f32")]
public partial class AnimationEvent : EventChannelBase
{
    public delegate void AnimationEventEventHandler(string next);
    public event AnimationEventEventHandler Event; 

    public void SendEventMessage(string next)
    {
        Event?.Invoke(next);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<string> nextBlackboardVariable = messageData[0] as BlackboardVariable<string>;
        var next = nextBlackboardVariable != null ? nextBlackboardVariable.Value : default(string);

        Event?.Invoke(next);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        AnimationEventEventHandler del = (next) =>
        {
            BlackboardVariable<string> var0 = vars[0] as BlackboardVariable<string>;
            if(var0 != null)
                var0.Value = next;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as AnimationEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as AnimationEventEventHandler;
    }
}

