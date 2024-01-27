// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HttpClientAuthorizationSample;

public sealed class AuthorizedHttpMessageHandler(HttpClient tokenHttpClient) : DelegatingHandler
{
  private readonly HttpClient _tokenHttpClient = tokenHttpClient ?? throw new ArgumentNullException(nameof(tokenHttpClient));

  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    Token token = await _tokenHttpClient.GetFromJsonAsync<Token>("", cancellationToken) ??
                  throw new Exception("No token retrieved");

    request.Headers.Authorization = new AuthenticationHeaderValue
    (
      scheme: token.TokenType,
      parameter: token.AccessToken
    );

    return await base.SendAsync(request, cancellationToken);
  }
}
