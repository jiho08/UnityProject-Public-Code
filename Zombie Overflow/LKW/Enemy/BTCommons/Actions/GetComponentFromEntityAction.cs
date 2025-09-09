using System;
using System.Collections.Generic;
using Enemies;
using Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Action = Unity.Behavior.Action;

namespace _01.Script.LKW.Enemy.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "GetComponentFromEntity", story: "Get component from [Self]", category: "Action", id: "db2a1a9d130441bd450e3ff106040910")]
    public partial class GetComponentFromEntityAction : Action
    {
        [SerializeReference] public BlackboardVariable<BTEnemy> Self;

        protected override Status OnStart()
        {
            
            BTEnemy enemy = Self.Value;
            
            SetVariable(enemy, "Animator",enemy.GetComponentInChildren<Animator>());
            
            List<BlackboardVariable> varList = enemy.btAgent.BlackboardReference.Blackboard.Variables;
            foreach (var variable in varList)
            {
                if(typeof(IEntityComponent).IsAssignableFrom(variable.Type) == false) continue; 
                
                SetVariable(enemy, variable.Name, enemy.GetCompo(variable.Type));
            }
            
            return Status.Success;
        }

        private void SetVariable(BTEnemy enemy, string variableName, IEntityComponent component)
        {
            Debug.Assert(component != null, $"Check {variableName} component not exist on {enemy.gameObject.name}");
            if (enemy.btAgent.GetVariable(variableName, out BlackboardVariable variable))
            {
                variable.ObjectValue = component;
            }
        }
        private void SetVariable<T>(BTEnemy enemy, string variableName, T component)
        {
            Debug.Assert(component != null, $"Check {variableName} component exist on {enemy.gameObject.name}");
            BlackboardVariable<T> variable = enemy.GetBlackboardVariable<T>(variableName);
            variable.Value = component;
        }
    }
}

