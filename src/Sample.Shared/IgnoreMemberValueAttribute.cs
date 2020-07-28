namespace Sample.Shared
{
    using System;

    // source: https://github.com/jhewlett/ValueObject
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreMemberValueAttribute : Attribute
    {
    }
}
