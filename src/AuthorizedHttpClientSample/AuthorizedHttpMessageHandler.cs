// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AuthorizedHttpClientSample;

public sealed class AuthorizedHttpMessageHandler(HttpClient tokenHttpClient, string getTokenUrl, string clientId, string clientSecret) : DelegatingHandler
{
  private const int CacheGapInMilliseconds = 500;

  private readonly HttpClient _tokenHttpClient = tokenHttpClient ?? throw new ArgumentNullException(nameof(tokenHttpClient));

  private readonly string _getTokenUrl = getTokenUrl ?? throw new ArgumentNullException(nameof(getTokenUrl));
  private readonly string _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
  private readonly string _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));

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
      using FormUrlEncodedContent content = new(new Dictionary<string, string>
      {
        { "grant_type", "client_credentials" },
        { "client_id", _clientId },
        { "client_secret", _clientSecret },
      });
      using HttpResponseMessage message = await _tokenHttpClient.PostAsync(_getTokenUrl, content, cancellationToken);
      Token token = await message.Content.ReadFromJsonAsync<Token>(cancellationToken) ??
                    throw new Exception("No token retrieved");

      _cachedToken = token;
      _cacheExpiresAt = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn);
    }

    return _cachedToken;
  }
}
