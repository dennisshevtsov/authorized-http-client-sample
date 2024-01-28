// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace HttpClientAuthorizationSample;

public sealed record class AuthorizedHttpClientSettings
{
  public required string TokenBaseUrl { get; init; }
  public required string GetTokenUrl { get; init; }
  public required string ClientId { get; init; }
  public required string ClientSecret { get; init; }

  public required string ApiBaseUrl { get; init; }
}
