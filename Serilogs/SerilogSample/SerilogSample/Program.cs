using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

namespace SerilogSample
{
    class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder ()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200")) // for the docker-compose implementation
                {
                    AutoRegisterTemplate = true,
                    //BufferBaseFilename = "./buffer",
                    RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway,
                    FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.WriteToFailureSink |
                                       EmitEventFailureHandling.RaiseCallback,
                    FailureSink = new FileSink("./fail-{Date}.txt", new JsonFormatter(), null, null)
                })
                .CreateLogger();
            

            int a = 10, b = 0;
            try
            {
                Log.Debug("Dividing {A} by {B}", a, b);
                Console.WriteLine(a / b);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong");
            }

            // Introduce a failure by storing a field as a different type
            Log.Debug("Reusing {A} by {B}", "string", true);

            Log.CloseAndFlush();
            Console.WriteLine("Press any key to continue...");
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(500);
            }
        }
    }
}