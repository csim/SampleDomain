namespace SampleApp.Shared.Abstractions
{
    public interface IRecordRepositoryOptions
    {
        string Connection { get; set; }
    }

    public class RecordRepositoryOptions : IRecordRepositoryOptions
    {
        public string Connection { get; set; }
    }
}
