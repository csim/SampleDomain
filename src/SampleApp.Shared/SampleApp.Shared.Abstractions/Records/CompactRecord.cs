namespace SampleApp.Shared.Abstractions.Records
{
    using System;

    public interface ICompactRecord
    {
        Guid? Id { get; set; }
    }

    public class CompactRecord : ValueObject, ICompactRecord
    {
        public Guid? Id { get; set; }
    }
}
