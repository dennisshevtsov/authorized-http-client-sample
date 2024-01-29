﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AuthorizedHttpClientSample;

public static class HttpClientAuthorizationSampleServices
{
  public static IHttpClientBuilder AddAuthorizedHttpClient(this IServiceCollection services, string sectionName = "")
  {
    ArgumentNullException.ThrowIfNull(services);

    services.AddOptions<AuthorizedHttpClientSettings>(sectionName)
            .ValidateOnStart();

    services.AddHttpClient
    (
      name: "token",
      configureClient: (IServiceProvider provider, HttpClient tokenHttpClient) =>
      {
        AuthorizedHttpClientSettings authorizedHttpClientSettings =
          provider.GetRequiredService<IOptions<AuthorizedHttpClientSettings>>().Value ??
          throw new Exception("No authorizated HTTP client settings retrieved");

        tokenHttpClient.BaseAddress = new Uri(authorizedHttpClientSettings.TokenBaseUrl);
      }
    );

    IHttpClientBuilder apiClientBuilder = services.AddHttpClient
    (
      name: "api",
      configureClient: (IServiceProvider provider, HttpClient tokenHttpClient) =>
      {
        AuthorizedHttpClientSettings authorizedHttpClientSettings =
          provider.GetRequiredService<IOptions<AuthorizedHttpClientSettings>>().Value ??
          throw new Exception("No authorizated HTTP client settings retrieved");

        tokenHttpClient.BaseAddress = new Uri(authorizedHttpClientSettings.ApiBaseUrl);
        tokenHttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        tokenHttpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
      }
    );

    apiClientBuilder.AddHttpMessageHandler((IServiceProvider provider) =>
    {
      AuthorizedHttpClientSettings authorizedHttpClientSettings =
          provider.GetRequiredService<IOptions<AuthorizedHttpClientSettings>>().Value ??
          throw new Exception("No authorizated HTTP client settings retrieved");

      HttpClient tokenHttpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient("token");
      AuthorizedHttpMessageHandler authorizedHttpMessageHandler = new
      (
        tokenHttpClient,
        authorizedHttpClientSettings.GetTokenUrl,
        authorizedHttpClientSettings.ClientId,
        authorizedHttpClientSettings.ClientSecret
      );

      return authorizedHttpMessageHandler;
    });

    return apiClientBuilder;
  }
}