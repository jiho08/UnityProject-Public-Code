using Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "KnockBack", story: "[Self] knockBack to [KnockBackDir] with [Mover]", category: "Action", id: "3387d88433f8d1abb0f212319f4871f2")]
public partial class KnockBackAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Self;
    [SerializeReference] public BlackboardVariable<EnemyMovement> Mover;
    [SerializeReference] public BlackboardVariable<Vector3> KnockBackDir;

    protected override Status OnStart()
    {
        Mover.Value.KnockBack(KnockBackDir.Value);
        return Status.Success;
    }

   
}

