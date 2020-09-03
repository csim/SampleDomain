namespace SampleApp.Orders.Client.Records
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using SampleApp.Shared.Abstractions.Records;

    public class ProductRecord : RecordBase, ICompactableRecord
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public override string ToString()
        {
            return $"{Number} - {Name}";
        }
    }

    [Owned]
    public class CompactProductRecord
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
