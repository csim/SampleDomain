﻿namespace SampleApp.Orders.Domain
{
    using SampleApp.Shared.Abstractions;

    public class OrdersDomainOptions
    {
        private RecordRepositoryOptions Repository { get; set; } = new RecordRepositoryOptions
        {
            Mode = RecordRepositoryMode.Cosmos,
            Connection = "AccountEndpoint=https://localhost:8081;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;DataName=Sample;"
        };
    }
}
