namespace SampleApp.Orders.Client.Records
{
    using System;
    using SampleApp.Shared.Abstractions;

    public class ProductRecord : RecordBase, ICompactableRecord
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public override string ToString()
        {
            return $"{Number} - {Name}";
        }
    }

    public class CompactProductRecord
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
