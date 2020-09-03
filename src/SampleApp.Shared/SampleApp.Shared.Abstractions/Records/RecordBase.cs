namespace SampleApp.Shared.Abstractions.Records
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using NServiceBus;

    public abstract class RecordBase : ValueObject
    {
        public DateTime? CreatedOn { get; set; }

        [Key]
        public Guid? Id { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string PartitionKey { get; set; }

        public virtual IMessage[] AddedMessages()
        {
            return null;
        }

        public virtual IMessage[] DeletedMessages()
        {
            return null;
        }

        public virtual IMessage[] UpdatedMessages()
        {
            return null;
        }
    }
}
