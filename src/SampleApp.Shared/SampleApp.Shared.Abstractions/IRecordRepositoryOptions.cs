namespace SampleApp.Shared.Abstractions
{
    public interface IRecordRepositoryOptions
    {
        string Connection { get; set; }

        RecordRepositoryMode Mode { get; set; }
    }

    public class RecordRepositoryOptions : IRecordRepositoryOptions
    {
        public string Connection { get; set; }

        public RecordRepositoryMode Mode { get; set; }
    }
}

public enum RecordRepositoryMode
{
    SqlLite,
    Cosmos,
    SqlServer,
    InMemory
}
