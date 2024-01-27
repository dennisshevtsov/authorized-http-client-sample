// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HttpClientAuthorizationSample;

public sealed class AuthorizedHttpMessageHandler(HttpClient tokenHttpClient) : DelegatingHandler
{
  private const int CacheGapInMilliseconds = 500;

  private readonly HttpClient _tokenHttpClient = tokenHttpClient ?? throw new ArgumentNullException(nameof(tokenHttpClient));

  private Token? _cachedToken;
  private DateTimeOffset _cacheExpiresAt;

  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    Token token = await RetrieveToken(cancellationToken);
    request.Headers.Authorization = new AuthenticationHeaderValue
    (
      scheme: token.TokenType,
      parameter: token.AccessToken
    );

    return await base.SendAsync(request, cancellationToken);
  }

  private async Task<Token> RetrieveToken(CancellationToken cancellationToken)
  {
    if (_cachedToken is null || _cacheExpiresAt < DateTimeOffset.UtcNow.AddMilliseconds(CacheGapInMilliseconds))
    {
      Token token = await _tokenHttpClient.GetFromJsonAsync<Token>("token", cancellationToken) ??
                    throw new Exception("No token retrieved");

      _cachedToken = token;
      _cacheExpiresAt = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn);
    }

    return _cachedToken;
  }
}
