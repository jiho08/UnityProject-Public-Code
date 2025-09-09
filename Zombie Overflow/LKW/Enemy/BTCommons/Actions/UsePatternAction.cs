using Enemies;
using Enemies.BossZombie;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UsePattern", story: "Use [Pattern] with [AttackCompo]", category: "Action", id: "5a1f0abd5d4faecb3ce2db3394521c9a")]
public partial class UsePatternAction : Action
{
    [SerializeReference] public BlackboardVariable<PatternType> Pattern;
    [SerializeReference] public BlackboardVariable<BossZombieAttackCompo> AttackCompo;

    protected override Status OnStart()
    {
        AttackCompo.Value.UsePattern(Pattern.Value);
        return Status.Success;
    }
}

