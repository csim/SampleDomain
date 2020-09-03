namespace SampleApp.Shared.Abstractions.Blobs
{
    public interface IBlobRepositoryOptions
    {
        string Connection { get; set; }

        BlobRespositoryMode Mode { get; set; }
    }

    public class BlobRepositoryOptions : IBlobRepositoryOptions
    {
        public string Connection { get; set; }

        public BlobRespositoryMode Mode { get; set; }
    }

    public enum BlobRespositoryMode
    {
        AzureStorage,
        FileSystem
    }
}
