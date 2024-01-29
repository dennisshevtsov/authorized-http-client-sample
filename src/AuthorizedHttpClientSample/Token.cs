// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace HttpClientAuthorizationSample;

public sealed record class Token
(
  [property: JsonPropertyName("access_token")] string AccessToken,
  [property: JsonPropertyName("token_type")]   string TokenType,
  [property: JsonPropertyName("expires_in")]   int    ExpiresIn,
  [property: JsonPropertyName("scope")]        string Scope,
  [property: JsonPropertyName("jti")]          string Jti
);
