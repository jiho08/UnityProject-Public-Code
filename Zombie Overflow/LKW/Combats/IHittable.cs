using Entities;
using UnityEngine;
namespace Combat
{
    public interface IHittable
    {
        public void TakeHit(Vector3 direction);
    }
}