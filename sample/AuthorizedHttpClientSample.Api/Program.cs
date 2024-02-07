// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication()
                .AddOAuth2Introspection(options =>
                {
                  options.IntrospectionEndpoint = "http://host.docker.internal:5002/connect/introspect";
                });
builder.Services.AddAuthorization();

WebApplication app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!")
   .RequireAuthorization();

app.Run();
