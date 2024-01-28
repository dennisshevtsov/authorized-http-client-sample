// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace HttpClientAuthorizationSample;

public static class HttpClientAuthorizationSampleServices
{
  public static IServiceCollection AddHttp(this IServiceCollection services, string tokenBaseUrl, string getTokenUrl, string clientId, string clientSecret, string apiBaseUrl)
  {
    ArgumentNullException.ThrowIfNull(services);

    services.AddHttpClient
    (
      name: "token",
      configureClient: httpClient => httpClient.BaseAddress = new Uri(tokenBaseUrl)
    );

    IHttpClientBuilder builder = services.AddHttpClient
    (
      name: "api",
      configureClient: httpClient =>
      {
        httpClient.BaseAddress = new Uri(apiBaseUrl);
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
      }
    );

    builder.AddHttpMessageHandler((IServiceProvider provider) =>
    {
      HttpClient tokenHttpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient("token");
      AuthorizedHttpMessageHandler authorizedHttpMessageHandler = new(tokenHttpClient, getTokenUrl, clientId, clientSecret);

      return authorizedHttpMessageHandler;
    });

    return services;
  }
}
