namespace Sample.Shared.Infrastructure
{
    public class SampleSharedInfrastructureOptions
    {
        public string RecordDatabaseConnection { get; set; } = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        public RecordDatabaseType RecordDatabaseType { get; set; } = RecordDatabaseType.Cosmos;
    }

    public enum RecordDatabaseType
    {
        SqlLite,
        Cosmos
    }
}
