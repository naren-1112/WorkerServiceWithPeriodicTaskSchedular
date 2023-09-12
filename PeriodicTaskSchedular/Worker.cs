using System.Xml.Linq;

namespace PeriodicTaskSchedular
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(10);
        string FileLocation;
        string FileName;


        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            FileLocation = _configuration.GetValue<string>("FilePath");
            FileName = _configuration.GetValue<string>("Filename");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Started");
            var Fname = FileLocation + "\\" + FileName;
            string text = "Service started" + "\n";
            File.AppendAllText(Fname, text);

            return base.StartAsync(cancellationToken);
        }
        private void CreateFile(string FileLocation, string FileName)
        {
            var Fname = FileLocation + "\\" + FileName;
            string text = "File created " + DateTime.Now.ToString() + " " + "\n";
            File.AppendAllText(Fname, text);
            _logger.LogInformation(Fname);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                CreateFile(FileLocation, FileName);
                _logger.LogInformation(DateTime.Now.ToString());
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Stopped");
            var Fname = FileLocation + "\\" + FileName;
            string text = "Service stopped";
            File.AppendAllText(Fname, text);
            return base.StopAsync(cancellationToken);
        }
        

    }
}