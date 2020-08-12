namespace SampleApp.Shared.Infrastructure.Blob
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using MimeMapping;
    using SampleApp.Shared.Abstractions;

    public class FileSystemBlobRepository : IBlobRepository
    {
        private string _basePath;

        public void Delete(string fileName)
        {
            DeleteAsync(fileName).GetAwaiter().GetResult();
        }

        public Task DeleteAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            File.Delete(filePath);

            var dir = Directory.GetParent(filePath);
            var dirFullName = dir.FullName;

            while (true)
            {
                if (string.Equals(dir.FullName, _basePath, StringComparison.CurrentCultureIgnoreCase)) break;

                var fileCount = Directory.GetFiles(dirFullName).Length;
                var dirCount = Directory.GetDirectories(dirFullName).Length;
                if (fileCount == 0 && dirCount == 0) Directory.Delete(dirFullName);

                dir = Directory.GetParent(dir.FullName);
                dirFullName = dir.FullName;
            }

            return Task.FromResult(true);
        }

        public bool Exists(string fileName)
        {
            return ExistsAsync(fileName).GetAwaiter().GetResult();
        }

        public Task<bool> ExistsAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            filePath = ExpandFilePath(filePath);
            var ret = !File.Exists(filePath);

            return Task.FromResult(ret);
        }

        public StorageBlobInfo Info(string fileName)
        {
            return InfoAsync(fileName).GetAwaiter().GetResult();
        }

        public Task<StorageBlobInfo> InfoAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            filePath = ExpandFilePath(filePath);
            var fileInfo = new FileInfo(filePath);

            var ret = new StorageBlobInfo
            {
                ContentType = MimeUtility.GetMimeMapping(filePath), Filename = filePath, Size = fileInfo.Length, Uri = new Uri($"file://{filePath}"),
            };

            return Task.FromResult(ret);
        }

        public byte[] Read(string fileName)
        {
            return ReadAsync(fileName).GetAwaiter().GetResult();
        }

        public Stream ReadAsStream(string fileName)
        {
            return ReadAsStreamAsync(fileName).GetAwaiter().GetResult();
        }

        public Task<Stream> ReadAsStreamAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            filePath = ExpandFilePath(filePath);
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File does not exists ({filePath})");

            return Task.FromResult((Stream)File.OpenRead(filePath));
        }

        public string ReadAsString(string filePath, Encoding encoding = null)
        {
            return ReadAsStringAsync(filePath, encoding).GetAwaiter().GetResult();
        }

        public Task<string> ReadAsStringAsync(string filePath, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            filePath = ExpandFilePath(filePath);
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File does not exists ({filePath})");

            return Task.FromResult(File.ReadAllText(filePath, encoding));
        }

        public Task<byte[]> ReadAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            filePath = ExpandFilePath(filePath);
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File does not exists ({filePath})");

            return Task.FromResult(File.ReadAllBytes(filePath));
        }

        public void SetBasePath(string basePath)
        {
            if (string.IsNullOrEmpty(basePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(basePath));

            _basePath = basePath;
        }

        public void Write(string filePath, string content, bool overwrite = false)
        {
            WriteAsync(filePath, content, Encoding.UTF8, overwrite).GetAwaiter().GetResult();
        }

        public void Write(string filePath, string content, Encoding encoding, bool overwrite = false)
        {
            WriteAsync(filePath, content, encoding, overwrite).GetAwaiter().GetResult();
        }

        public void Write(string filePath, Stream stream, bool overwrite = false)
        {
            WriteAsync(filePath, stream, overwrite).GetAwaiter().GetResult();
        }

        public void Write(string filePath, byte[] content, bool overwrite = false)
        {
            WriteAsync(filePath, content, overwrite).GetAwaiter().GetResult();
        }

        public Task WriteAsync(string filePath, string content, bool overwrite = false)
        {
            return WriteAsync(filePath, content, Encoding.UTF8, overwrite);
        }

        public async Task WriteAsync(string filePath, string content, Encoding encoding, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));
            if (content == null) throw new ArgumentNullException(nameof(content));
            encoding ??= Encoding.UTF8;

            var bcontent = encoding.GetBytes(content.ToCharArray());

            await WriteAsync(filePath, bcontent, overwrite);
        }

        public Task WriteAsync(string filePath, Stream stream, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            if (File.Exists(filePath) && !overwrite) throw new ApplicationException($"File already exists ({filePath})");

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

        public Task WriteAsync(string filePath, byte[] content, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            filePath = ExpandFilePath(filePath);

            if (File.Exists(filePath) && !overwrite) throw new ApplicationException($"File already exists ({filePath})");

            EnsureFilePath(filePath);
            File.WriteAllBytes(filePath, content);

            return Task.CompletedTask;
        }

        private void EnsureFilePath(string fileName)
        {
            var parentDir = Directory.GetParent(fileName).FullName;
            if (!Directory.Exists(parentDir)) Directory.CreateDirectory(parentDir);
        }

        private string ExpandFilePath(string filePath)
        {
            return Path.Combine(_basePath, filePath);
        }
    }
}
