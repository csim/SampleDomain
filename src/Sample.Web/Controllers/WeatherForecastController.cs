namespace Sample.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample.Abstractions;
    using Sample.Domain.Records;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IRecordRepository recordRepository,
            IBlobRepository blobRepository)
        {
            _logger = logger;
            _recordRepository = recordRepository;
            _blobRepository = blobRepository;
        }

        private readonly IBlobRepository _blobRepository;

        //private static readonly string[] _summaries =
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRecordRepository _recordRepository;

        [HttpGet]
        public async Task<object> Get()
        {
            //var containerName = "container2";
            //await _blobRepository.EnsureContainerAsync(containerName);

            //var g = Guid.NewGuid();

            //var filename = $"{g}/x1/File_{DateTime.UtcNow.Ticks}.txt";
            //var filename2 = $"{g}/x2/File_{DateTime.UtcNow.Ticks}.txt";

            ////var content = Encoding.UTF8.GetBytes($"testc: {DateTime.UtcNow}".ToCharArray());
            //var content = $"testc: {DateTime.UtcNow}";

            //await _blobRepository.WriteBlobAsync(containerName, filename, content, overwrite: true);
            //await _blobRepository.WriteBlobAsync(containerName, filename2, content, overwrite: true);

            ////await _blobRepository.ReadBlobAsStringAsync(containerName, filename);
            //await _blobRepository.DeleteBlobAsync(containerName, filename);

            //return true; 

            //return await _blobRepository.BlobInfoAsync(containerName, filename);


            await _recordRepository
                .AddAsync(new ToDoItemRecord { PartitionKey = "cbaeb852-449b-4619-9618-006b8a063634", Description = $"test {DateTime.UtcNow}" });


            //await _recordRepository
            //    .AddAsync(new ToDoItemRecord
            //    {
            //        PartitionKey = Guid.Empty.ToString(),
            //        Description = $"test {DateTime.UtcNow}"
            //    });

            //await _recordRepository
            //    .AddAsync(new ToDoItemRecord
            //    {
            //        PartitionKey = Guid.NewGuid().ToString(),
            //        Description = $"test {DateTime.UtcNow}"
            //    });

            var items = _recordRepository
                .AsQueryable<ToDoItemRecord>()
                .Where(_ => _.PartitionKey == "cbaeb852-449b-4619-9618-006b8a063634")
                .OrderByDescending(_ => _.CreatedOn)
                .ToList();


            return items;

            //var rng = new Random();
            //return Enumerable.Range(1, 5)
            //    .Select(
            //        index => new WeatherForecast
            //        {
            //            Date = DateTime.Now.AddDays(index), TemperatureC = rng.Next(-20, 55), Summary = Summaries[rng.Next(Summaries.Length)]
            //        })
            //    .ToArray();
        }
    }
}
