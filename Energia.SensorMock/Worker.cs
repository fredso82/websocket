
using Microsoft.AspNetCore.SignalR.Client;

namespace Energia.SensorMock
{
    public class Worker(ILogger<Worker> logger) : IHostedService, IDisposable
    {
        private readonly ILogger<Worker> _logger = logger;
        private Timer? _timer = null;
        private readonly HubConnection _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7158/energiaHub")
                .WithAutomaticReconnect()
                .Build();

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
                Consumo = random.Next(10, 100),
                Timestamp = DateTime.Now
            };

            try
            {
                await _connection.InvokeAsync("EnviarConsumo", consumo);                
                _logger.LogInformation($"Dados Enviados. Consumido {consumo.Consumo} kWh em {consumo.Timestamp}.");
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

        public void Dispose() => _timer?.Dispose();
    }
}
