using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace FSM
{
    public class EntityStateMachine
    {
        public EntityState CurrentState { get; private set; }
        private Dictionary<string, EntityState> _states;

        public EntityStateMachine(Entity entity, StateSO[] stateList)
        {
            _states = new Dictionary<string, EntityState>();
            foreach (var state in stateList)
            {
                var type = Type.GetType(state.className);
                Debug.Assert(type != null, $"Finding type is null D: : {state.className}");
                var entityState = Activator.CreateInstance(type, entity, state.animParam) as EntityState;
                _states.Add(state.stateName, entityState);
            }
        }

        public void ChangeState(string newStateName, bool forced = false)
        {
            var newState = _states.GetValueOrDefault(newStateName);
            Debug.Assert(newState != null, $"State is null D: : {newStateName}");
            
            if (!forced && CurrentState == newState)
            {
                return;
            }
            
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }
        
        public void UpdateStateMachine()
        {
            CurrentState?.Update();
        }

        public void DestroyStateMachine()
        {
            foreach (var state in _states.Values)
            {
                state.Destroy();
            }

            _states.Clear();
            CurrentState = null;
        }
    }
}