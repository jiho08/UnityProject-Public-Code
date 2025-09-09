using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        protected Dictionary<Type,IEntityComponent> _components = new Dictionary<Type,IEntityComponent>();

        public UnityEvent OnHit;

        public int DeadBodyLayer { get; private set; }

        protected virtual void Awake()
        {
            DeadBodyLayer = LayerMask.NameToLayer("DeadBody");
            
            AddComponents();
            InitializeComponents();
            AfterInitialize();
        }
    
        protected virtual void InitializeComponents()
        {
            _components.Values.ToList().ForEach(compo => compo.Initialize(this));
        }
        
        protected virtual void AfterInitialize()
        {
            _components.Values.OfType<IAfterInit>().ToList().ForEach(compo => compo.AfterInit());
            OnHit?.AddListener(HandleHit);
        }

        protected virtual void OnDestroy()
        {
            OnHit.RemoveListener(HandleHit);
        }
        
        protected virtual void AddComponents()
        {
            GetComponentsInChildren<IEntityComponent>().ToList()
                .ForEach(compo => _components.Add(compo.GetType(),compo));
        }
            
            
        public T GetCompo<T>(bool isDerived = false) where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out var compo))
            {
                return (T)compo;
            }

            if (!isDerived) return default(T);

            var findType = _components.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if (findType == null) return default(T);
            return (T)_components[findType];
        }
        
        public IEntityComponent GetCompo(Type type) 
            => _components.GetValueOrDefault(type);

        protected abstract void HandleHit();

    }
}
