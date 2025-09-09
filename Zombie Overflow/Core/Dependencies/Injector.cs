using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core.Dependencies
{    [DefaultExecutionOrder(-10)]
    public class Injector : MonoBehaviour
    {
        private const BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private readonly Dictionary<Type, object> _registry = new Dictionary<Type, object>();

        private void Awake()
        {
            var providers = FindMonoBehaviours().OfType<IDependencyProvider>();
            foreach (var provider in providers)
            {
                
                RegisterProvider(provider);
            }

            var injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
        }

        private void Inject(MonoBehaviour injectable)
        {
            var type = injectable.GetType();
            var injectableFields = type.GetFields(_bindingFlags)
                .Where(f => Attribute.IsDefined(f, typeof(InjectAttribute)));
            foreach (var f in injectableFields)
            {
                var fieldType = f.FieldType;
                var injectInstance = Resolve(fieldType);
                Debug.Assert(injectInstance != null, $"Inject instance not found for {fieldType.FullName}");
                f.SetValue(injectable, injectInstance);
            }

            var injectableMethods = type.GetMethods(_bindingFlags)
                .Where(m => Attribute.IsDefined(m, typeof(InjectAttribute)));
            foreach (var m in injectableMethods)
            {
                var requiredParameters = m.GetParameters().Select(p => p.ParameterType).ToArray();
                var parameterInstances = requiredParameters.Select(Resolve).ToArray();
                m.Invoke(injectable, parameterInstances);
            }
        }

        private object Resolve(Type fieldType)
        {
            _registry.TryGetValue(fieldType, out object instance);
            return instance;
        }

        private bool IsInjectable(MonoBehaviour mono)
        {
            var members = mono.GetType().GetMembers(_bindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        private void RegisterProvider(IDependencyProvider provider)
        {
            if (Attribute.IsDefined(provider.GetType(), typeof(PropertyAttribute)))
            {
                UnityEngine.Debug.Log($"Registering provider {provider.GetType()}, instance {provider}");
                _registry.Add(provider.GetType(), provider);
                return;
            }

            var methods = provider.GetType().GetMethods(_bindingFlags);

            foreach (var method in methods)
            {
                if(!Attribute.IsDefined(method, typeof(ProvideAttribute) )) continue;

                Type returnType = method.ReturnType;
                object providedInstance = method.Invoke(provider, null);
                Debug.Assert(providedInstance != null, $"Provided instance is null : {method.Name}");
                
                _registry.Add(returnType, providedInstance);

            }
        }

        private static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        }
    }
}