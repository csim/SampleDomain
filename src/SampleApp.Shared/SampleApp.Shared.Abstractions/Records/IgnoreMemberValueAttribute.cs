namespace SampleApp.Shared.Abstractions.Records
{
    using System;

    // source: https://github.com/jhewlett/ValueObject
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreMemberValueAttribute : Attribute
    {
    }
}
