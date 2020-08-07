namespace SampleApp.Shared.Abstractions
{
    using System;

    public interface ICompactableRecord
    {
        public Guid? Id { get; set; }

        public virtual CompactRecord ToCompact()
        {
            return new CompactRecord { Id = Id };
        }

        public virtual T ToCompact<T>() where T : CompactRecord, new()
        {
            return new T { Id = Id };
        }
    }
}
