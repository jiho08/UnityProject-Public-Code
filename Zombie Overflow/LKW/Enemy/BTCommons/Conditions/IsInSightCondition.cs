using Enemies;
using System;
using Enemies.PoliceZombie;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsInSight", story: "[Self] check [Target]", category: "Conditions", id: "463d70758b7fe27ca4b1dba65534c0a7")]
public partial class IsInSightCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    public override bool IsTrue()
    {
        float distance = Vector3.Distance(Self.Value.transform.position, Target.Value.position);

        return distance < Self.Value.AttackRange;
    }
}
