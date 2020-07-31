namespace Sample.Shared.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class RecordBase : ValueObject
    {
        [IgnoreMemberValue]
        [NotMapped]
        public List<EventBase> Events { get; } = new List<EventBase>();

        [Key]
        public Guid? Id { get; set; }

        public string PartitionKey { get; set; }

    }
}
