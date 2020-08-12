namespace SampleApp.Shared.Infrastructure.Blob
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using MimeMapping;
    using SampleApp.Shared.Abstractions;

    [ExcludeFromCodeCoverage]
    public class AzureStorageBlobRepository : IBlobRepository
    {
        private CloudBlobClient _client;
        private string _connectionString;

        private static HashSet<string> _knownContainers = new HashSet<string>();

        private CloudBlobClient Client
        {
            get
            {
                if (_client != null) return _client;

                var account = CloudStorageAccount.Parse(_connectionString);
                _client = account.CreateCloudBlobClient();

                return _client;
            }
        }

        private string ContainerNameFromPath(string filePath)
        {
            var containerName = Path.GetPathRoot(filePath);
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));

            return containerName;
        }

        private string EnsureContainerFromPath(string filePath)
        {
            var containerName = ContainerNameFromPath(filePath);

            if (!_knownContainers.Contains(containerName))
            {
                EnsureContainer(containerName);
                _knownContainers.Add(containerName);
            }

            return containerName;
        }

        private async Task<string> EnsureContainerFromPathAsync(string filePath)
        {
            var containerName = ContainerNameFromPath(filePath);

            if (!_knownContainers.Contains(containerName))
            {
                await EnsureContainerAsync(containerName);
                _knownContainers.Add(containerName);
            }

            return containerName;
        }

        public bool Exists(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var containerName = ContainerNameFromPath(filePath);

            var container = Client.GetContainerReference(containerName);
            if (!container.Exists()) return false;

            var blob = container.GetBlockBlobReference(filePath);
            return blob.Exists();
        }

        public async Task<bool> ExistsAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var containerName = ContainerNameFromPath(filePath);

            var container = Client.GetContainerReference(containerName);
            var cexists = await container.ExistsAsync();
            if (!cexists) return false;

            var blob = container.GetBlockBlobReference(filePath);
            return await blob.ExistsAsync();
        }

        public StorageBlobInfo Info(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var containerName = ContainerNameFromPath(filePath);

            var container = Client.GetContainerReference(containerName);
            if (container == null || !container.Exists()) return null;

            var blob = container.GetBlockBlobReference(filePath);
            if (blob == null || !blob.Exists()) return null;

            return BlobInfo(container, blob);
        }

        public async Task<StorageBlobInfo> InfoAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var containerName = ContainerNameFromPath(filePath);

            var container = Client.GetContainerReference(containerName);
            if (!await container.ExistsAsync()) return null;

            var blob = container.GetBlockBlobReference(filePath);
            if (!await blob.ExistsAsync()) return null;

            return BlobInfo(container, blob);
        }

        public StorageBlobContainerInfo ContainerInfo(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            var container = Client.GetContainerReference(name);
            return !container.Exists() ? null : ContainerInfo(container);
        }

        public void CopyBlob(StorageBlobCopyInfo copyInfo, CancellationToken? cancellationToken = null)
        {
            CopyBlob(new[] { copyInfo }, cancellationToken);
        }

        public void CopyBlob(IEnumerable<StorageBlobCopyInfo> copyInfos, CancellationToken? cancellationToken = null)
        {
            var waitTargetBlobs = new List<string>();

            foreach (var copyInfo in copyInfos)
            {
                var targetContainer = Client.GetContainerReference(copyInfo.TargetContainerName);
                var targetBlob = targetContainer.GetBlockBlobReference(copyInfo.TargetFilename);
                waitTargetBlobs.Add(targetBlob.Uri.ToString());

                targetBlob.StartCopy(new Uri($"{copyInfo.SourceUri}{copyInfo.SourceAccessToken}"));
                if (cancellationToken != null && cancellationToken.Value.IsCancellationRequested) throw new OperationCanceledException();
            }

            var waitCount = 1;
            var removeWait = new List<string>();

            while (waitCount < 200)
            {
                foreach (var waitTargetBlob in waitTargetBlobs)
                {
                    var targetBlob = Client.GetBlobReferenceFromServer(new Uri(waitTargetBlob));
                    targetBlob.FetchAttributes();

                    var status = targetBlob.CopyState.Status;
                    if (status == CopyStatus.Aborted || status == CopyStatus.Failed || status == CopyStatus.Invalid)
                    {
                        throw new ApplicationException($"Unable to copy blob ({status}) {targetBlob.Uri}");
                    }

                    if (status == CopyStatus.Success) removeWait.Add(waitTargetBlob);
                    if (status == CopyStatus.Pending) break; // exit check loop if there is at least one pending copy
                }

                foreach (var uri in removeWait) waitTargetBlobs.Remove(uri);
                removeWait.Clear();

                if (!waitTargetBlobs.Any()) break;

                var waitSeconds = waitCount <= 5 ? 2
                    : waitCount <= 10 ? 3
                    : waitCount <= 15 ? 4
                    : 10;

                if (cancellationToken != null && cancellationToken.Value.IsCancellationRequested) throw new OperationCanceledException();

                Thread.Sleep(waitSeconds * 1000);
                waitCount++;
            }
        }

        public void Delete(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var containerName = ContainerNameFromPath(filePath);

            var container = Client.GetContainerReference(containerName);
            if (!container.Exists()) return;

            var blob = container.GetBlockBlobReference(filePath);
            blob.Delete();
        }

        public async Task DeleteAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var containerName = ContainerNameFromPath(filePath);

            var container = Client.GetContainerReference(containerName);
            var cexists = await container.ExistsAsync();
            if (!cexists) return;

            var blob = container.GetBlockBlobReference(filePath);
            await blob.DeleteAsync();
        }

        public bool DeleteContainer(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            var container = Client.GetContainerReference(name);
            return container.DeleteIfExists();
        }

        public async Task<bool> DeleteContainerAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            var container = Client.GetContainerReference(name);
            return await container.DeleteIfExistsAsync();
        }

        public StorageBlobContainerInfo EnsureContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));

            var container = Client.GetContainerReference(containerName);

            container.CreateIfNotExists();

            return ContainerInfo(container);
        }

        public async Task<StorageBlobContainerInfo> EnsureContainerAsync(string containerName)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));

            var container = Client.GetContainerReference(containerName);

            await container.CreateIfNotExistsAsync();

            return ContainerInfo(container);
        }

        public StorageBlobQueryResponse<StorageBlobInfo> ListBlobs(
            string containerName,
            string prefix = null,
            int take = 1000,
            string continuationToken = null)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));
            if (take <= 0 || take > 1000) throw new ArgumentOutOfRangeException(nameof(take));

            var container = Client.GetContainerReference(containerName);

            BlobContinuationToken token = null;

            if (!string.IsNullOrEmpty(continuationToken))
            {
                token = new BlobContinuationToken { NextMarker = continuationToken };
            }

            var result = container.ListBlobsSegmented(prefix, true, BlobListingDetails.UncommittedBlobs, take, token, null, null);
            var results = result.Results?.Select(_ => BlobInfo(container, (CloudBlob)_)) ?? new List<StorageBlobInfo>();

            return new StorageBlobQueryResponse<StorageBlobInfo> { Items = results, ContinuationToken = result.ContinuationToken?.NextMarker };
        }

        /// <summary>
        ///     Segment a list of blob containers, returns a list and a continuation token. To begin, use a new ContinuationToken
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="take">Max 5000</param>
        /// <param name="continuationToken"></param>
        /// <returns></returns>
        public StorageBlobQueryResponse<StorageBlobContainerInfo> ListContainers(
            string prefix = null,
            int take = 1000,
            string continuationToken = null)
        {
            if (take <= 0 || take > 1000) throw new ArgumentOutOfRangeException(nameof(take));

            BlobContinuationToken token = null;

            if (!string.IsNullOrEmpty(continuationToken))
            {
                token = new BlobContinuationToken { NextMarker = continuationToken };
            }

            var result = Client.ListContainersSegmented(prefix, ContainerListingDetails.Metadata, take, token);
            var results = result.Results?.Select(ContainerInfo) ?? new List<StorageBlobContainerInfo>();

            return new StorageBlobQueryResponse<StorageBlobContainerInfo>
            {
                Items = results, ContinuationToken = result.ContinuationToken?.NextMarker
            };
        }

        public byte[] Read(string filePath)
        {
            using var stream = ReadAsStream(filePath);
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        public Stream ReadAsStream(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var containerName = ContainerNameFromPath(filePath);
            var container = Client.GetContainerReference(containerName);
            if (!container.Exists()) throw new FileNotFoundException($"Container not found. ({containerName})");

            var blob = container.GetBlockBlobReference(filePath);
            if (!blob.Exists()) throw new FileNotFoundException($"File not found. ({containerName}/{filePath})");

            return blob.OpenRead();
        }

        public async Task<Stream> ReadAsStreamAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var containerName = ContainerNameFromPath(filePath);
            var container = Client.GetContainerReference(containerName);
            var cexists = await container.ExistsAsync();
            if (!cexists) throw new FileNotFoundException($"Container not found. ({containerName})");

            var blob = container.GetBlockBlobReference(filePath);
            if (!await blob.ExistsAsync()) throw new FileNotFoundException($"File not found. ({containerName}/{filePath})");

            var stream = new MemoryStream();
            await blob.DownloadToStreamAsync(stream);
            return stream;
        }

        public string ReadAsString(string filePath, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var content = Read(filePath);
            return encoding.GetString(content);
        }

        public async Task<string> ReadAsStringAsync(string filePath, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var content = await ReadAsync(filePath);
            return encoding.GetString(content);
        }

        public async Task<byte[]> ReadAsync(string filePath)
        {
            await using var stream = await ReadAsStreamAsync(filePath);
            await using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

        public void SetConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("Value cannot be null or empty.", nameof(connectionString));

            _connectionString = connectionString;
            _client = null;
        }

        public string SharedReadAccessToken(string containerName, DateTime? startTime = null, TimeSpan? timeSpan = null)
        {
            var container = Client.GetContainerReference(containerName);
            if (!container.Exists()) return null;

            timeSpan ??= TimeSpan.FromHours(1);
            startTime ??= DateTime.UtcNow;
            var expiryTime = startTime + timeSpan;

            return container.GetSharedAccessSignature(
                new SharedAccessBlobPolicy
                {
                    Permissions = SharedAccessBlobPermissions.Read, SharedAccessExpiryTime = expiryTime, SharedAccessStartTime = startTime
                });
        }

        public void Write(string filePath, string content, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentException("Value cannot be null or empty.", nameof(content));

            Write(filePath, content, Encoding.UTF8, overwrite);
        }

        public void Write(string filePath, string content, Encoding encoding, bool overwrite = false)
        {
            Write(filePath, encoding.GetBytes(content), overwrite);
        }

        public void Write(string filePath, byte[] content, bool overwrite = false)
        {
            using var stream = new MemoryStream(content);
            Write(filePath, stream, overwrite);
        }

        public void Write(string filePath, Stream stream, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var containerName = ContainerNameFromPath(filePath);
            var container = Client.GetContainerReference(containerName);

            var blob = container.GetBlockBlobReference(filePath);
            if (!overwrite && blob.Exists()) throw new IOException($"File exists ({filePath})");

            stream.Position = 0;
            blob.UploadFromStream(stream);
            blob.Properties.ContentType = MimeUtility.GetMimeMapping(filePath);
            blob.SetProperties();
        }

        public async Task WriteAsync(string filePath, string content, bool overwrite = false)
        {
            await WriteAsync(filePath, content, Encoding.UTF8, overwrite);
        }

        public async Task WriteAsync(string filePath, string content, Encoding encoding, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentException("Value cannot be null or empty.", nameof(content));

            await WriteAsync(filePath, encoding.GetBytes(content), overwrite);
        }

        public async Task WriteAsync(string filePath, Stream stream, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var containerName = ContainerNameFromPath(filePath);
            var container = Client.GetContainerReference(containerName);

            var blob = container.GetBlockBlobReference(filePath);
            if (!overwrite && await blob.ExistsAsync()) throw new IOException($"File exists ({filePath})");

            stream.Position = 0;
            await blob.UploadFromStreamAsync(stream);
            blob.Properties.ContentType = MimeUtility.GetMimeMapping(filePath);
            await blob.SetPropertiesAsync();
        }

        public async Task WriteAsync(string filePath, byte[] content, bool overwrite = false)
        {
            await using var stream = new MemoryStream(content);
            await WriteAsync(filePath, stream, overwrite);
        }

        private static StorageBlobInfo BlobInfo(CloudBlobContainer container, CloudBlob blob)
        {
            var ret = new StorageBlobInfo
            {
                //Container = ContainerInfo(container),
                Filename = blob.Name,
                ContentType = blob.Properties.ContentType,
                Size = blob.Properties.Length,
                Uri = blob.Uri,
                //AbsoluteUri = blob.Uri.AbsoluteUri
            };

            return ret;
        }

        private static StorageBlobContainerInfo ContainerInfo(CloudBlobContainer container)
        {
            return new StorageBlobContainerInfo { Name = container.Name, Uri = container.Uri };
        }
    }

    
    public class StorageBlobQueryResponse<T>
    {
        public string ContinuationToken { get; set; }

        public IEnumerable<T> Items { get; set; }
    }

    public class StorageBlobContainerInfo
    {
        public string Name { get; set; }

        public Uri Uri { get; set; }
    }

    public class StorageBlobCopyInfo
    {
        public string SourceAccessToken { get; set; }

        public string SourceFilename { get; set; }

        public Uri SourceUri { get; set; }

        public string TargetContainerName { get; set; }

        public string TargetFilename { get; set; }
    }
}
