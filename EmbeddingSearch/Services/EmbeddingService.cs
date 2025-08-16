using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using OpenAI.Embeddings;
using Pgvector;
using System.ClientModel;

namespace EmbeddingSearch.Services
{
    public class EmbeddingService
    {
        private readonly AzureEmbeddingOptions _options;
        private readonly AzureOpenAIClient _azureOpenAIClient;
        private readonly ILogger<EmbeddingService> _logger;

        public EmbeddingService(IOptions<AzureEmbeddingOptions> options,
            AzureOpenAIClient azureOpenAIClient,
            ILogger<EmbeddingService> logger)
        {
            _options = options.Value;
            _azureOpenAIClient = azureOpenAIClient;
            _logger = logger;
        }

        public async Task<Vector?> GetEmbeddingsAsync(string text)
        {
            EmbeddingClient chatClient = _azureOpenAIClient.GetEmbeddingClient(_options.DeploymentName);

            ClientResult<OpenAIEmbeddingCollection>? response = 
                await chatClient.GenerateEmbeddingsAsync([text]);

            OpenAIEmbedding? embeddings = response.Value.FirstOrDefault();

            if (embeddings is not null)
            {
                return new Vector(embeddings.ToFloats());
            }

            _logger.LogWarning("No embeddings found for the provided text: {Text}", text);
            return null;
        }
    }
}
