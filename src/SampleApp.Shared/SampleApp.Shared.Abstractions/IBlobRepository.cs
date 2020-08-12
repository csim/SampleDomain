namespace SampleApp.Shared.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public interface IBlobRepository
    {
        void Delete(string filePath);

        Task DeleteAsync(string filePath);

        bool Exists(string filePath);

        Task<bool> ExistsAsync(string filePath);

        StorageBlobInfo Info(string filePath);

        Task<StorageBlobInfo> InfoAsync(string filePath);

        byte[] Read(string filePath);

        Stream ReadAsStream(string filePath);

        Task<Stream> ReadAsStreamAsync(string filePath);

        string ReadAsString(string filePath, Encoding encoding = null);

        Task<string> ReadAsStringAsync(string filePath, Encoding encoding = null);

        Task<byte[]> ReadAsync(string filePath);

        void Write(string filePath, string content, bool overwrite = false);

        void Write(string filePath, string content, Encoding encoding, bool overwrite = false);

        void Write(string filePath, Stream stream, bool overwrite = false);

        void Write(string filePath, byte[] content, bool overwrite = false);

        Task WriteAsync(string filePath, string content, bool overwrite = false);

        Task WriteAsync(string filePath, string content, Encoding encoding, bool overwrite = false);

        Task WriteAsync(string filePath, Stream stream, bool overwrite = false);

        Task WriteAsync(string filePath, byte[] content, bool overwrite = false);
    }

    [ExcludeFromCodeCoverage]
    public class StorageBlobInfo
    {
        public string ContentType { get; set; }

        public string Filename { get; set; }

        public string Path { get; set; }

        /// <summary>
        ///     Size in Bytes
        /// </summary>
        public long? Size { get; set; }

        public double SizeKb => (Size ?? 0) / 1024d;

        public double SizeMb => SizeKb / 1024d;

        public Uri Uri { get; set; }
    }
}
