using System;
using Core.Dependencies;
using Players;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "EntityFinder", menuName = "SO/Entity/Finder", order = 0)]
    public class EntityFinderSO : ScriptableObject
    {
        public Entity target {get; private set;}

        public void SetTarget(Entity entity)
        {
            target = entity;
        }
    }
}