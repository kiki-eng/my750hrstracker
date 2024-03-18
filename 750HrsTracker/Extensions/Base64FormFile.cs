namespace _750HrsTracker.Extensions
{
    public class Base64FormFile : IFormFile
    {
        private readonly string _fileName;
        private readonly string _contentType;
        private readonly byte[] _fileContent;

        public Base64FormFile()
        {
            
        }

        public Base64FormFile(string fileName, string contentType, byte[] fileContent)
        {
            _fileName = fileName;
            _contentType = contentType;
            _fileContent = fileContent;
        }

        public string ContentType => _contentType;

        public string ContentDisposition => $"form-data; name=\"file\"; filename=\"{_fileName}\"";

        public IHeaderDictionary Headers => throw new NotImplementedException();

        public long Length => _fileContent.Length;

        public string Name => "file";

        public string FileName => _fileName;

        public void CopyTo(Stream target)
        {
            target.Write(_fileContent, 0, _fileContent.Length);
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            return target.WriteAsync(_fileContent, 0, _fileContent.Length, cancellationToken);
        }

        public Stream OpenReadStream()
        {
            return new MemoryStream(_fileContent);
        }
    }
}
