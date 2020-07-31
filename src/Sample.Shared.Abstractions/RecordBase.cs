namespace Sample.Shared.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public abstract class RecordBase : ValueObject
    {
        [IgnoreMemberValue]
        public List<EventBase> Events { get; set; } = new List<EventBase>();

        [Key]
        public Guid? Id { get; set; }

        public string PartitionKey { get; set; }
    }
}
