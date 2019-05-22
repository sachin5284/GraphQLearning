using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace SerilogSample
{
    public class SerilogLoggerFactory : ILoggerFactory
    {
        private readonly SerilogLoggerProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogLoggerFactory"/> class.
        /// </summary>
        /// <param name="logger">The Serilog logger; if not supplied, the static <see cref="Serilog.Log"/> will be used.</param>
        /// <param name="dispose">When true, dispose <paramref name="logger"/> when the framework disposes the provider. If the
        /// logger is not specified but <paramref name="dispose"/> is true, the <see cref="Log.CloseAndFlush"/> method will be
        /// called on the static <see cref="Log"/> class instead.</param>
        public SerilogLoggerFactory(ILogger logger = null, bool dispose = false)
        {
            provider = new SerilogLoggerProvider(logger, dispose);
        }

        /// <summary>
        /// Disposes the provider.
        /// </summary>
        public void Dispose()
        {
            provider.Dispose();
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Extensions.Logging.ILogger" />.
        /// </returns>
        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return provider.CreateLogger(categoryName);
        }

        /// <summary>
        /// Adds an <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" /> to the logging system.
        /// </summary>
        /// <param name="provider">The <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" />.</param>
        public void AddProvider(ILoggerProvider provider)
        {
            SelfLog.WriteLine("Ignoring added logger provider {0}", provider);
        }
    }
}