namespace Sample.Shared.Abstractions
{
    using System;
    using System.Collections.Generic;

    public abstract class RecordBase
    {
        public List<EventBase> Events { get; } = new List<EventBase>();

        public Guid? Id { get; set; }
    }
}
