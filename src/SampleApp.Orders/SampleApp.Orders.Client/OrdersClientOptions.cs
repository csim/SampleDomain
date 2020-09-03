namespace SampleApp.Orders.Client
{
    using SampleApp.Shared.Abstractions.Blobs;
    using SampleApp.Shared.Abstractions.Records;

    public class OrdersClientOptions
    {
        public BlobRepositoryOptions Blobs { get; set; } = new BlobRepositoryOptions
        {
            Mode = BlobRespositoryMode.FileSystem, Connection = @"c:\blob"
        };

        public RecordRepositoryOptions Records { get; set; } = new RecordRepositoryOptions
        {
            Connection =
                "AccountEndpoint=https://localhost:8081;DatabaseName=SampleOrders;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;"
        };
    }
}
