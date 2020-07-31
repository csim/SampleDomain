namespace Sample.Shared.Infrastructure.Data
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Sample.Shared.Abstractions;

    public class FileSystemBlobRepository : IBlobRepository
    {
        private string _basePath;

        public bool BlobExists(string containerName, string fileName)
        {
            return BlobExistsAsync(containerName, fileName).GetAwaiter().GetResult();
        }

        public Task<bool> BlobExistsAsync(string containerName, string fileName)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileName));

            var filePath = FilePath(containerName, fileName);
            var ret = !File.Exists(filePath);

            return Task.FromResult(ret);
        }

        public StorageBlobInfo BlobInfo(string containerName, string fileName, bool includeExtendedProperties = false)
        {
            throw new NotImplementedException();
        }

        public Task<StorageBlobInfo> BlobInfoAsync(string containerName, string fileName, bool includeExtendedProperties = false)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileName));

            throw new NotImplementedException();
        }

        public StorageBlobContainerInfo ContainerInfo(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            throw new NotImplementedException();
        }

        public void DeleteBlob(string containerName, string fileName)
        {
            DeleteBlobAsync(containerName, fileName).GetAwaiter().GetResult();
        }

        public Task DeleteBlobAsync(string containerName, string fileName)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileName));

            var filePath = FilePath(containerName, fileName);
            File.Delete(filePath);

            var parentPath = Directory.GetParent(filePath).FullName;

            while (true)
            {
                if (parentPath.EndsWith(containerName)) break;

                var fileCount = Directory.GetFiles(parentPath).Length;
                var dirCount = Directory.GetDirectories(parentPath).Length;
                if (fileCount == 0 && dirCount == 0) Directory.Delete(parentPath);

                parentPath = Directory.GetParent(parentPath).FullName;
            }

            return Task.FromResult(true);
        }

        public StorageBlobContainerInfo EnsureContainer(string containerName, bool setPublicAccessType = false)
        {
            return EnsureContainerAsync(containerName, setPublicAccessType).GetAwaiter().GetResult();
        }

        public Task<StorageBlobContainerInfo> EnsureContainerAsync(string containerName, bool setPublicAccessType = false)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));

            var path = Path.Combine(_basePath, containerName);
            Console.WriteLine(path);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var ret = new StorageBlobContainerInfo { Name = containerName, Uri = new Uri($"file://{containerName}") };

            return Task.FromResult(ret);
        }

        public byte[] ReadBlob(string containerName, string fileName)
        {
            throw new NotImplementedException();
        }

        public Stream ReadBlobAsStream(string containerName, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<MemoryStream> ReadBlobAsStreamAsync(string containerName, string fileName)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileName));

            throw new NotImplementedException();
        }

        public string ReadBlobAsString(string containerName, string fileName, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileName));

            throw new NotImplementedException();
        }

        public Task<string> ReadBlobAsStringAsync(string containerName, string fileName, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileName));

            var filePath = FilePath(containerName, fileName);
            if (!File.Exists(filePath)) throw new ApplicationException($"File does not exists ({fileName})");

            return Task.FromResult(File.ReadAllText(filePath));
        }

        public Task<byte[]> ReadBlobAsync(string containerName, string fileName)
        {
            throw new NotImplementedException();
        }

        public void SetBasePath(string basePath)
        {
            if (string.IsNullOrEmpty(basePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(basePath));

            _basePath = basePath;
        }

        public void WriteBlob(string containerName, string fileName, string content, bool overwrite = false)
        {
            throw new NotImplementedException();
        }

        public void WriteBlob(string containerName, string fileName, string content, Encoding encoding, bool overwrite = false)
        {
            throw new NotImplementedException();
        }

        public void WriteBlob(string containerName, string fileName, Stream stream, bool overwrite = false)
        {
            WriteBlobAsync(containerName, fileName, stream, overwrite).GetAwaiter().GetResult();
        }

        public void WriteBlob(string containerName, string fileName, byte[] content, bool overwrite = false)
        {
            WriteBlobAsync(containerName, fileName, content, overwrite).GetAwaiter().GetResult();
        }

        public Task WriteBlobAsync(string containerName, string fileName, string content, bool overwrite = false)
        {
            return WriteBlobAsync(containerName, fileName, content, Encoding.UTF8, overwrite);
        }

        public async Task WriteBlobAsync(string containerName, string fileName, string content, Encoding encoding, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileName));
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            var bcontent = encoding.GetBytes(content.ToCharArray());

            await WriteBlobAsync(containerName, fileName, bcontent, overwrite);
        }

        public Task WriteBlobAsync(string containerName, string fileName, Stream stream, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileName));

            var filePath = FilePath(containerName, fileName);

            if (File.Exists(filePath))
            {
                if (overwrite)
                {
                    File.Exists(filePath);
                }
                else
                {
                    throw new ApplicationException($"File already exists ({fileName})");
                }
            }

            EnsureFilePath(filePath);
            using var fileStream = File.Create(filePath);

            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, len);
            }

            return Task.CompletedTask;
        }

        public Task WriteBlobAsync(string containerName, string fileName, byte[] content, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentException("Value cannot be null or empty.", nameof(containerName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Value cannot be null or empty.", nameof(fileName));

            var filePath = FilePath(containerName, fileName);

            if (File.Exists(filePath))
            {
                if (overwrite)
                {
                    File.Exists(filePath);
                }
                else
                {
                    throw new ApplicationException($"File already exists ({fileName})");
                }
            }

            EnsureFilePath(filePath);
            File.WriteAllBytes(filePath, content);

            return Task.CompletedTask;
        }

        private void EnsureFilePath(string fileName)
        {
            var parentDir = Directory.GetParent(fileName).FullName;
            if (!Directory.Exists(parentDir)) Directory.CreateDirectory(parentDir);
        }

        private string FilePath(string containerName, string fileName)
        {
            var containerPath = Path.Combine(_basePath, containerName);
            if (!Directory.Exists(containerPath)) throw new ApplicationException($"Container does not exist ({containerName})");

            return Path.Combine(containerPath, fileName);
        }
    }
}
