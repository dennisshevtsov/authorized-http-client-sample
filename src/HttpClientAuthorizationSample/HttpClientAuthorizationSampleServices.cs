// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace HttpClientAuthorizationSample;

public static class HttpClientAuthorizationSampleServices
{
  public static IServiceCollection AddHttp(this IServiceCollection services, string tokenBaseUrl, string apiBaseUrl)
  {
    ArgumentNullException.ThrowIfNull(services);

    services.AddHttpClient
    (
      name: "token",
      configureClient: httpClient => httpClient.BaseAddress = new Uri(tokenBaseUrl)
    );

    services.AddHttpClient
    (
      name: "api",
      configureClient: httpClient =>
      {
        httpClient.BaseAddress = new Uri(apiBaseUrl);
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
      }
    )
    .AddHttpMessageHandler((IServiceProvider provider) =>
    {
      HttpClient tokenHttpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient("token");
      AuthorizedHttpMessageHandler authorizedHttpMessageHandler = new(tokenHttpClient);

      return authorizedHttpMessageHandler;
    });

    return services;
  }
}
