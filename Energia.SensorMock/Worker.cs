
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;

namespace Energia.SensorMock
{
    public class Worker : IHostedService, IDisposable
    {
        private readonly ILogger<Worker> _logger;
        private Timer? _timer = null;
        private HubConnection _connection;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7158/energiaHub")
                .WithAutomaticReconnect()
                .Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _connection.StartAsync(cancellationToken);
            _logger.LogInformation("Conexão com o Hub estabelecida.");

            _timer = new Timer(EnviarDados, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private async void EnviarDados(object? state)
        {
            var random = new Random();
            var consumo = new
            {
                Consumo = Math.Round(10 + random.NextDouble() * 100),
                Timestamp = DateTime.UtcNow
            };

            try
            {
                await _connection.InvokeAsync("EnviarConsumo", consumo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar dados de consumo de energia.");
            }

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço parado");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
