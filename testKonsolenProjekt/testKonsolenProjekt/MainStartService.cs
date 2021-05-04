using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace testKonsolenProjekt
{
    public class MainStartService : IMainStartService
    {
        private readonly ILogger<MainStartService> _logger;
        private readonly IConfiguration _config;

        public MainStartService(ILogger<MainStartService> logger, IConfiguration config)
        {
            this._logger = logger;
            this._config = config;
        }

        public void Run()
        {
            _logger.LogInformation("Started Main Service");

            for (int i = 0; i < _config.GetValue<int>("LogTimes"); i++)
            {
                _logger.LogInformation("Start the main logic for {runTime}", i);
            }
        }
    }
}
