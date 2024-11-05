using Microsoft.AspNetCore.SignalR;

namespace Energia.WebSocket
{
    public class EnergiaHub(EnergiaDbContext context) : Hub
    {
        private readonly EnergiaDbContext _context = context;

        public async Task EnviarConsumo(EnergiaDados dados)
        {
            try
            {
                _context.Consumos.Add(dados);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Dados Recebidos. Consumido {dados.Consumo} kWh em {dados.Timestamp}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro {ex.Message}");
            }            
        }
    }
}
