using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace _01.Script.LKW.Enemy.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChangeAnimation", story: "[animator] change [current] to [next]", category: "Action", id: "4573ea014299cf3336d396fd7c8a410a")]
    public partial class ChangeAnimationAction : Action
    {
        [SerializeReference] public BlackboardVariable<Animator> Animator;
        [SerializeReference] public BlackboardVariable<string> Current;
        [SerializeReference] public BlackboardVariable<string> Next;

        protected override Status OnStart()
        {
            Animator.Value.SetBool(Current.Value, false);
            Current.Value = Next.Value;
            Animator.Value.SetBool(Current.Value, true);
        
            return Status.Success;
        }

    }
}

