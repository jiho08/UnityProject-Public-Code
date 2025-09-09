using Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/BossStateChangeEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "BossStateChangeEvent", message: "Boss state change to [newState]", category: "Events", id: "2e03740bb688e9972bf3af8e3af6bf8f")]
public partial class BossStateChangeEvent : EventChannelBase
{
    public delegate void BossStateChangeEventEventHandler(BossState newState);
    public event BossStateChangeEventEventHandler Event; 

    public void SendEventMessage(BossState newState)
    {
        Event?.Invoke(newState);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<BossState> newStateBlackboardVariable = messageData[0] as BlackboardVariable<BossState>;
        var newState = newStateBlackboardVariable != null ? newStateBlackboardVariable.Value : default(BossState);

        Event?.Invoke(newState);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        BossStateChangeEventEventHandler del = (newState) =>
        {
            BlackboardVariable<BossState> var0 = vars[0] as BlackboardVariable<BossState>;
            if(var0 != null)
                var0.Value = newState;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as BossStateChangeEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as BossStateChangeEventEventHandler;
    }
}

