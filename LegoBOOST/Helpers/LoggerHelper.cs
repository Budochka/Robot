using NLog;
using NLog.Config;
using NLog.Targets;

namespace LegoBOOST.Helpers
{
    internal static class LoggerHelper
    {
        public static Logger Instance { get; private set; }

        static LoggerHelper()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            // Step 3. Set target properties 
            consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";
            string tempPath = System.IO.Path.GetTempPath();
            fileTarget.FileName = tempPath + "log_legoboost.txt";
            fileTarget.Layout = @"${date:format=HH\:mm\:ss} ${message}";

            // Step 4. Define rules
            config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;

            Instance = LogManager.GetCurrentClassLogger();
        }
    }
}
