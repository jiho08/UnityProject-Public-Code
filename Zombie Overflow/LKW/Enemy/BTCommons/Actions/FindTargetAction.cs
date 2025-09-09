using System;
using Enemies;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace _01.Script.LKW.Enemy.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "FindTarget", story: "[self] set [target] from finder", category: "Action", id: "e61acc0128eacd89a190672b72a0d60f")]
    public partial class FindTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<BTEnemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            Target.Value = Self.Value.EntityFinder.target.transform;
            Debug.Assert(Target.Value != null, $"Target is null {Self.Value.gameObject.name}");
            return Status.Success;
        }
    }
}

