// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace AuthorizedHttpClientSample.Cdek;

public sealed record class City
(
  [property: JsonPropertyName("code")]          int     Code,
  [property: JsonPropertyName("city")]          string  CityName,
  [property: JsonPropertyName("fias_guid")]     Guid    FiasGuid,
  [property: JsonPropertyName("city_uuid")]     Guid    CityUuid,
  [property: JsonPropertyName("country_code")]  string  CountryCode,
  [property: JsonPropertyName("country")]       string  Country,
  [property: JsonPropertyName("region")]        string  Region,
  [property: JsonPropertyName("region_code")]   int?    RegionCode,
  [property: JsonPropertyName("sub_region")]    string? SubRegion,
  [property: JsonPropertyName("longitude")]     float?  Longitude,
  [property: JsonPropertyName("latitude")]      float?  Latitude,
  [property: JsonPropertyName("time_zone")]     string? TimeZone,
  [property: JsonPropertyName("payment_limit")] float   PaymentLimit
);
