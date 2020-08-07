namespace SampleApp.Shared.Infrastructure
{
    public class InfrastructureOptions
    {
        public string AzureStorageAccountConnection { get; set; } = "UseDevelopmentStorage=true";

        public BlobRespositoryMode BlobRespositoryMode { get; set; } = BlobRespositoryMode.FileSystem;

        public CosmosConnectionInfo CosmosConnection { get; set; } = new CosmosConnectionInfo();

        public string FileSystemBlobBasePath { get; set; } = @"c:\blob";

        public RecordRepositoryMode RecordRepositoryMode { get; set; } = RecordRepositoryMode.InMemory;

        public string SqlLiteConnection { get; set; } =
            "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    }

    public class CosmosConnectionInfo
    {
        public string AccountEndpoint { get; set; } = "https://localhost:8081";

        public string AccountKey { get; set; } = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        public string DatabaseName { get; set; } = "Sample";
    }

    public enum RecordRepositoryMode
    {
        SqlLite,
        Cosmos,
        InMemory
    }

    public enum BlobRespositoryMode
    {
        AzureStorage,
        FileSystem
    }
}
