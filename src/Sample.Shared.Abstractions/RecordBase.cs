namespace Sample.Shared.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public abstract class RecordBase : ValueObject
    {
        public DateTime? CreatedOn { get; set; }

        [IgnoreMemberValue]
        public List<IRecordEvent> Events { get; } = new List<IRecordEvent>();

        [Key]
        public Guid? Id { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string PartitionKey { get; set; }
    }
}
