FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AuthorizedHttpClientSample.Api/AuthorizedHttpClientSample.Api.csproj", "AuthorizedHttpClientSample.Api/"]
RUN dotnet restore "./AuthorizedHttpClientSample.Api/./AuthorizedHttpClientSample.Api.csproj"
COPY . .
WORKDIR "/src/AuthorizedHttpClientSample.Api"
RUN dotnet build "./AuthorizedHttpClientSample.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthorizedHttpClientSample.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizedHttpClientSample.Api.dll"]
