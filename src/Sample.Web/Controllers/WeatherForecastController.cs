namespace Sample.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample.Domain.Records;
    using Sample.Shared.Abstractions;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRecordRepository recordRepository)
        {
            _logger = logger;
            _recordRepository = recordRepository;
        }

        //private static readonly string[] _summaries =
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRecordRepository _recordRepository;

        [HttpGet]
        public async Task<object> Get()
        {
            await _recordRepository
                .AddAsync(new ToDoItemRecord
                {
                    PartitionKey = Guid.Empty.ToString(),
                    Description = $"test {DateTime.UtcNow}"
                });

            await _recordRepository
                .AddAsync(new ToDoItemRecord
                {
                    PartitionKey = Guid.Empty.ToString(),
                    Description = $"test {DateTime.UtcNow}"
                });

            await _recordRepository
                .AddAsync(new ToDoItemRecord
                {
                    PartitionKey = Guid.NewGuid().ToString(),
                    Description = $"test {DateTime.UtcNow}"
                });

            var items = _recordRepository
                .AsQueryable<ToDoItemRecord>()
                .Where(_ => _.PartitionKey != Guid.Empty.ToString())
                .OrderByDescending(_ => _.Description)
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
