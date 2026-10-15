# Image of the API (PlaningPoker.Api), used to deploy it on Fly.io (see fly.toml).
# Build context: the root of the repository.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY PlaningPoker.Api/PlaningPoker.Api.csproj PlaningPoker.Api/
RUN dotnet restore PlaningPoker.Api/PlaningPoker.Api.csproj
COPY PlaningPoker.Api/ PlaningPoker.Api/
RUN dotnet publish PlaningPoker.Api/PlaningPoker.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

# With DATABASE_URL (Postgres, e.g. Neon on Render) the API uses it; otherwise it creates a SQLite database
# (./PlaningPoker.db) in the working directory when it starts.
# /data is where the persistent volume is mounted (see [[mounts]] in fly.toml).
RUN mkdir -p /data
WORKDIR /data

ENTRYPOINT ["dotnet", "/app/PlaningPoker.Api.dll"]
