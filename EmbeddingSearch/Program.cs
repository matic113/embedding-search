using Azure.AI.OpenAI;
using EmbeddingSearch.Data;
using EmbeddingSearch.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using System.ClientModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddOptions<AzureEmbeddingOptions>()
    .Bind(builder.Configuration.GetSection("AzureOpenAI:Embeddings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(sp =>
{
    // Resolve the IOptions instance from the DI container
    var embeddingOptions = sp.GetRequiredService<IOptions<AzureEmbeddingOptions>>().Value;

    // Use the resolved options to create and return the client
    return new AzureOpenAIClient(new Uri(embeddingOptions.Endpoint), new ApiKeyCredential(embeddingOptions.Key));
});

builder.Services.AddScoped<EmbeddingService>();

builder.Services.AddDbContext<ApplicationDbContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
    o=> o.UseVector())
);
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/seed-embeddings", (IServiceProvider serviceProvider) => 
    ProductEmbeddingsSeeder.SeedProductEmbeddings(serviceProvider)
);

app.Run();
