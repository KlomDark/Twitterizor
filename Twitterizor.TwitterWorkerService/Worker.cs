using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Twitterizor.TwitterWorkerService
{
    public class Worker : BackgroundService
    {
        public static IConfigurationRoot Configuration { get; set; }
        private static Core.TwitterSecrets _secrets;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            CreateConfigurationBuilder();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Twitter Worker Service running at: {time}", DateTimeOffset.Now);

                await ProcessTweets();
            }
        }

        private static void CreateConfigurationBuilder()
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddUserSecrets<Core.TwitterSecrets>();

            Configuration = builder.Build();

            // Twitterizor API Keys should be stored in User Secrets. Ex:
            // {
            //    "TwitterSecrets": {
            //      "ApiKey": "API Key goes here",
            //      "ApiSecretKey": "API Secret Key goes here",
            //      "BearerToken": "Bearer Token goes heres"
            //    }
            // }

            var section = Configuration.GetSection("TwitterSecrets");

            IServiceCollection services = new ServiceCollection();

            services.Configure<Core.TwitterSecrets>(c => section.Bind(c))
                    .AddOptions()
                    .AddSingleton<Core.ISecretRevealer, Core.SecretRevealer>()
                    .BuildServiceProvider();

            var serviceProvider = services.BuildServiceProvider();

            var revealer = serviceProvider.GetService<Core.ISecretRevealer>();
            _secrets = revealer.GetTwitterSecrets();
        }

        private static async Task ProcessTweets()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                var requestUri = "https://api.twitter.com/2/tweets/sample/stream";

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _secrets.BearerToken);
                var stream = httpClient.GetStreamAsync(requestUri).Result;

                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        // Read the stream
                        var latestTweet = reader.ReadLine();

                        if (!string.IsNullOrEmpty(latestTweet))
                        {
                            var tweet = JsonSerializer.Deserialize<Core.Models.Tweet>(latestTweet);

                            // Make sure string is encoded with UTF8
                            //byte[] bytes = Encoding.Default.GetBytes(text);
                            //var utf8Text = Encoding.UTF8.GetString(bytes);

                            Core.TwitterData.Instance.AnalyzeTweet(tweet);
                        }
                    }

                    Console.WriteLine("End of stream encountered, stopping process...");
                }
            }

            await Task.Delay(0);
        }
    }
}
