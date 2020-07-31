using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Shared.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;

    public interface IBlobRepository
	{
		bool BlobExists(string containerName, string fileName);

		Task<bool> BlobExistsAsync(string containerName, string fileName);

		StorageBlobInfo BlobInfo(string containerName, string fileName, bool includeExtendedProperties = false);

		Task<StorageBlobInfo> BlobInfoAsync(string containerName, string fileName, bool includeExtendedProperties = false);

		StorageBlobContainerInfo ContainerInfo(string name);


		void DeleteBlob(string containerName, string fileName);

		Task DeleteBlobAsync(string containerName, string fileName);

		StorageBlobContainerInfo EnsureContainer(string containerName, bool setPublicAccessType = false);

		Task<StorageBlobContainerInfo> EnsureContainerAsync(string containerName, bool setPublicAccessType = false);

		//StorageBlobQueryResponse<StorageBlobInfo> ListBlobs(
		//	string containerName,
		//	string prefix = null,
		//	int take = 1000,
		//	string continuationToken = null,
		//	bool includeExtendedProperties = false);

		//StorageBlobQueryResponse<StorageBlobContainerInfo> ListContainers(
		//	string prefix = null,
		//	int take = 1000,
		//	string continuationToken = null);

		byte[] ReadBlob(string containerName, string fileName);

		Stream ReadBlobAsStream(string containerName, string fileName);

		Task<MemoryStream> ReadBlobAsStreamAsync(string containerName, string fileName);

		string ReadBlobAsString(string containerName, string fileName, Encoding encoding = null);

		Task<string> ReadBlobAsStringAsync(string containerName, string fileName, Encoding encoding = null);

		Task<byte[]> ReadBlobAsync(string containerName, string fileName);

		void WriteBlob(string containerName, string fileName, string content, bool overwrite = false);

		void WriteBlob(string containerName, string fileName, string content, Encoding encoding, bool overwrite = false);

		void WriteBlob(string containerName, string fileName, Stream stream, bool overwrite = false);

		void WriteBlob(string containerName, string fileName, byte[] content, bool overwrite = false);

		Task WriteBlobAsync(string containerName, string fileName, string content, bool overwrite = false);

		Task WriteBlobAsync(string containerName, string fileName, string content, Encoding encoding, bool overwrite = false);

		Task WriteBlobAsync(string containerName, string fileName, Stream stream, bool overwrite = false);

		Task WriteBlobAsync(string containerName, string fileName, byte[] content, bool overwrite = false);
	}

    public class StorageBlobQueryResponse<T>
    {
        public string ContinuationToken { get; set; }

        public IEnumerable<T> Items { get; set; }
    }

    public class StorageBlobCopyInfo
    {
        public string SourceAccessToken { get; set; }

        public string SourceFilename { get; set; }

        public Uri SourceUri { get; set; }

        public string TargetContainerName { get; set; }

        public string TargetFilename { get; set; }
    }

    public class StorageBlobContainerInfo
    {
        public string Name { get; set; }

        public Uri Uri { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class StorageBlobInfo
    {
        public string AbsoluteUri { get; set; }

        public StorageBlobContainerInfo Container { get; set; }

        public string ContentType { get; set; }

        public string Filename { get; set; }

        public DateTime LastModified { get; set; }

        /// <summary>
        ///      Size in Bytes
        /// </summary>
        public long? Size { get; set; }

        public double SizeKb => (Size ?? 0) / 1024d;

        public double SizeMb => SizeKb / 1024d;

        public Uri Uri { get; set; }
    }
}
