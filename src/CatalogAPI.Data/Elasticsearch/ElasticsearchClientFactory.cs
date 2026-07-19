using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;

namespace CatalogAPI.Data.Elasticsearch;

public class ElasticsearchClientFactory
{
    public const string IndexName = "games";

    public ElasticsearchClient Client { get; }

    public ElasticsearchClientFactory(IConfiguration configuration)
    {
        var uri = configuration["Elasticsearch:Uri"] ?? "http://localhost:9200";

        var settings = new ElasticsearchClientSettings(new Uri(uri))
            .DefaultIndex(IndexName);

        Client = new ElasticsearchClient(settings);
    }
}
