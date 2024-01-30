// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using AuthorizedHttpClientSample.Cdek;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

ConfigurationBuilder builder = new();
builder.AddInMemoryCollection(new Dictionary<string, string?>
{
  ["TokenBaseUrl"] = "https://api.edu.cdek.ru",
  ["GetTokenUrl"]  = "v2/oauth/token?parameters",
  ["ClientId"]     = "EMscd6r9JnFiQ3bLoyjJY6eM78JrJceI",
  ["ClientSecret"] = "PjLZkKBHEiLK3YsjtNrt3TGNG0ahs3kG",
  ["ApiBaseUrl"]   = "https://api.edu.cdek.ru",
});

IConfiguration configuration = builder.Build();

ServiceCollection services = new();
services.AddSingleton(configuration);
services.AddAuthorizedHttpClient(tokenClientName: "cdek-token", apiClientName  : "cdek-api")
        .WithSettings(configuration);

using IServiceScope scope = services.BuildServiceProvider().CreateScope();

HttpClient http = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("cdek-api");
City[]? cities = await http.GetFromJsonAsync<City[]>("v2/location/cities");

if (cities is null)
{
  return;
}

Console.WriteLine("Top 10 cities:");
for (int i = 0;  i < (cities.Length < 10 ? cities.Length : 10); i++)
{
  Console.WriteLine($"City #{i}: {cities[i]}");
}
