
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MediaControlApp.Interceptors
{
    public class LoggingInterceptor : ICommandInterceptor
    {
        private Stopwatch? _stopwatch;
        private readonly ILogger _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger):base() {
            _logger = logger;
        }

        public void Intercept(CommandContext context, CommandSettings settings )
        {
            // Runs before command execution
            _stopwatch = Stopwatch.StartNew();
            _logger.LogInformation($"Starting command: {context.Name}");
           
        }

        public void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
        {
            // Runs after command execution
            _stopwatch?.Stop();
            _logger.LogInformation($"Command completed in {_stopwatch?.ElapsedMilliseconds}ms (exit code: {result})");

        }
    }
}
