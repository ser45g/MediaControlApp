using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MediaControlApp.Interceptors
{
    public class TimingInterceptor : ICommandInterceptor
    {
        private Stopwatch? _stopwatch;

        public void Intercept(CommandContext context, CommandSettings settings)
        {
            // Runs before command execution
            _stopwatch = Stopwatch.StartNew();
            System.Console.WriteLine($"Starting command: {context.Name}");
        }

        public void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
        {
            // Runs after command execution
            _stopwatch?.Stop();
            System.Console.WriteLine($"Command completed in {_stopwatch?.ElapsedMilliseconds}ms (exit code: {result})");
        }
    }
}
