using System;

namespace Core.Dependencies
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
        
    }
}