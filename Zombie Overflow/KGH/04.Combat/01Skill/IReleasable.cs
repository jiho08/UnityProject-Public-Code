using System;

namespace Combat.Skills
{
    public interface IReleasable
    {
        public bool IsHolding { get; }
        public void ReleaseSkill();
        public void CancelSkill();
        
    }
}