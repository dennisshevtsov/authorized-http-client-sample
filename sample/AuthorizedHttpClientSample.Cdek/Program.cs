using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

ConfigurationBuilder builder = new();
builder.AddInMemoryCollection(new Dictionary<string, string?>
{
  { "TokenBaseUrl", "" },
  { "GetTokenUrl" , "" },
  { "ClientId"    , "" },
  { "ClientSecret", "" },
  { "ApiBaseUrl"  , "" },
});

IConfiguration configuration = builder.Build();

ServiceCollection services = new();
services.AddSingleton(configuration);
services.AddAuthorizedHttpClient();

using IServiceScope scope = services.BuildServiceProvider().CreateScope();

HttpClient http = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("api");
