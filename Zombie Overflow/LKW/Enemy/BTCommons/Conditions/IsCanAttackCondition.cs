using Enemies.PoliceZombie;
using System;
using Enemies;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsCanAttack", story: "Check can attack with [AttackCompo]", category: "Conditions", id: "2edd4eb2a26da9d36a948d0e0cd0025a")]
public partial class IsCanAttackCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyAttackCompo> AttackCompo;

    public override bool IsTrue()
    {
        return Time.time >= AttackCompo.Value.lastAttackTime + AttackCompo.Value.attackCooldown;
    }

    public override void OnStart()
    {
    }
}
