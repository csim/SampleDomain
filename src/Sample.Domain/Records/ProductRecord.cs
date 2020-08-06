namespace Sample.Ordering.Domain.Records
{
    using System;
    using Sample.Abstractions;

    public class ProductRecord : RecordBase
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public override string ToString()
        {
            return $"{Number} - {Name}";
        }
    }

    public class ProductCompact
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
