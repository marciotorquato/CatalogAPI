using CatalogAPI.Data;
using CatalogAPI.Domain.Enum;
using CatalogAPI.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatalogAPI.Domain.Consumers;

public class PaymentProcessedConsumer
{
    private readonly ILogger<PaymentProcessedConsumer> _logger;
    private readonly CatalogApiDbContext _context;

    public PaymentProcessedConsumer(
        ILogger<PaymentProcessedConsumer> logger,
        CatalogApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task ProcessAsync(PaymentProcessedEvent paymentEvent)
    {
        _logger.LogInformation("CONSUMER EXECUTADO | UsuarioId: {UsuarioId} | GameId: {GameId} | Status: {Status}", paymentEvent.UsuarioId, paymentEvent.GameId, paymentEvent.Status);

        try
        {
            // Buscar o registro na tabela UsuarioGameBiblioteca
            var usuarioGame = await _context.UsuarioGameBiblioteca.FirstOrDefaultAsync(ug => ug.UsuarioId == paymentEvent.UsuarioId && ug.GameId == paymentEvent.GameId);

            if (usuarioGame == null)
            {
                _logger.LogWarning("Registro não encontrado | UsuarioId: {UsuarioId} | GameId: {GameId}", paymentEvent.UsuarioId, paymentEvent.GameId);
                return;
            }

            // Verificar se o status é "Aprovado"
            if (paymentEvent.Status.Equals("Aprovado", StringComparison.OrdinalIgnoreCase))
            {
                // Atualizar Status para Concluido
                usuarioGame.Status = StatusCompra.Concluido;
                usuarioGame.DataAtualizacaoStatus = DateTimeOffset.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Status atualizado para Concluído | UsuarioId: {UsuarioId} | GameId: {GameId}", paymentEvent.UsuarioId, paymentEvent.GameId);
            }
            else
            {
                _logger.LogWarning("Status do pagamento não é 'Aprovado': {Status}", paymentEvent.Status);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar PaymentProcessed | UsuarioId: {UsuarioId} | GameId: {GameId}", paymentEvent.UsuarioId, paymentEvent.GameId);
            throw;
        }
    }
}