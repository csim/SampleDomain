namespace SampleApp.Orders.Client
{
    using SampleApp.Shared.Abstractions;

    public class OrdersClientOptions
    {
        public RecordRepositoryOptions RecordRepository { get; set; } = new RecordRepositoryOptions
        {
            Connection = "AccountEndpoint=https://localhost:8081;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;DatabaseName=Sample;"
        };
    }
}
