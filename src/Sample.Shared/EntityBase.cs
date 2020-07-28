namespace Sample.Shared
{
    using System.Collections.Generic;

    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class EntityBase
    {
        public int Id { get; set; }

        public List<EventBase> Events = new List<EventBase>();
    }
}
