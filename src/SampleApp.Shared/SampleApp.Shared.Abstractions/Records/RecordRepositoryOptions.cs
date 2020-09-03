namespace SampleApp.Shared.Abstractions.Records
{
    public interface IRecordRepositoryOptions
    {
        string Connection { get; set; }

        RecordRespositoryMode Mode { get; set; }
    }

    public class RecordRepositoryOptions : IRecordRepositoryOptions
    {
        public string Connection { get; set; }

        public RecordRespositoryMode Mode { get; set; }
    }

    public enum RecordRespositoryMode
    {
        Cosmos,
        SqlServer
    }
}
