using CatalogAPI.Data.Elasticsearch;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Repository;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport.Products.Elasticsearch;

namespace CatalogAPI.Data.Repositories.Elasticsearch;

public class SearchRepository : ISearchRepository
{
    private readonly ElasticsearchClient _client;

    public SearchRepository(ElasticsearchClientFactory factory)
        => _client = factory.Client;

    public async Task IndexAsync(Game game)
    {
        await EnsureIndexAsync();

        var document = new GameDocument
        {
            Id = game.Id,
            Nome = game.Nome,
            Descricao = game.Descricao,
            Genero = game.Genero,
            Desenvolvedor = game.Desenvolvedor,
            Preco = game.Preco,
            DataRelease = game.DataRelease
        };

        // Id do documento = Id do jogo: reindexar um jogo editado
        // sobrescreve o documento existente, sem duplicar no índice.
        var response = await _client.IndexAsync(document, ElasticsearchClientFactory.IndexName, document.Id.ToString());

        ThrowIfInvalid(response, $"Falha ao indexar o jogo {document.Id} no Elasticsearch");
    }

    public async Task<List<Game>> SearchAsync(string termo, int take = 20)
    {
        var response = await _client.SearchAsync<GameDocument>(s => s
            .Indices(ElasticsearchClientFactory.IndexName)
            .Size(take)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(termo)
                    .Fields(new[] { "nome", "descricao", "genero", "desenvolvedor" })
                    .Fuzziness(new Fuzziness("AUTO"))
                )
            )
        );

        ThrowIfInvalid(response, $"Falha ao buscar '{termo}' no Elasticsearch");

        // response.Documents já vem ordenado por _score (relevância) — é o
        // comportamento padrão do Elasticsearch pra uma query sem "sort"
        // explícito.
        return response.Documents.Select(d => new Game
        {
            Id = d.Id,
            Nome = d.Nome,
            Descricao = d.Descricao,
            Genero = d.Genero,
            Desenvolvedor = d.Desenvolvedor,
            Preco = d.Preco,
            DataRelease = d.DataRelease
        }).ToList();
    }

    private async Task EnsureIndexAsync()
    {
        var exists = await _client.Indices.ExistsAsync(ElasticsearchClientFactory.IndexName);
        if (exists.Exists)
        {
            return;
        }

        var response = await _client.Indices.CreateAsync(ElasticsearchClientFactory.IndexName, c => c
            .Mappings(m => m
                .Properties<GameDocument>(p => p
                    .Text(t => t.Nome)
                    .Text(t => t.Descricao)
                    .Text(t => t.Genero)
                    .Text(t => t.Desenvolvedor)
                )
            )
        );

        ThrowIfInvalid(response, "Falha ao criar o índice 'games' no Elasticsearch");
    }

    // O client oficial NÃO lança exceção por padrão em falhas de requisição
    // (conexão recusada, erro HTTP etc.) — ele só marca IsValidResponse
    // como false. Sem essa checagem, erros de indexação/busca ficam
    // completamente silenciosos.
    private static void ThrowIfInvalid(ElasticsearchResponse response, string message)
    {
        if (!response.IsValidResponse)
        {
            throw new InvalidOperationException($"{message}: {response.DebugInformation}");
        }
    }
}
