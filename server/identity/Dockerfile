# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /app

# Copy property files
COPY ["server/identity/Directory.Build.props", "server/identity/"]
COPY ["server/identity/Directory.Packages.props", "server/identity/"]
COPY ["server/identity/src/Directory.Build.props", "server/identity/src/"]
COPY ["server/identity/src/persistence/Directory.Build.props", "server/identity/src/persistence/"]
COPY ["server/identity/src/presentation/Directory.Build.props", "server/identity/src/presentation/"]

COPY ["server/common/Directory.Build.props", "server/common/"]
COPY ["server/common/Directory.Packages.props", "server/common/"]
COPY ["server/common/src/persistence/Directory.Build.props", "server/common/src/persistence/"]
COPY ["server/common/src/presentation/Directory.Build.props", "server/common/src/presentation/"]

# Copy all project files
COPY ["server/identity/src/application/Application.csproj", "server/identity/src/application/"]
COPY ["server/identity/src/domain/Domain.csproj", "server/identity/src/domain/"]
COPY ["server/identity/src/persistence/mongodb/Persistence.MongoDb.csproj", "server/identity/src/persistence/mongodb/"]
COPY ["server/identity/src/presentation/graphql/Presentation.GraphQL.csproj", "server/identity/src/presentation/graphql/"]
COPY ["server/identity/src/presentation/rest/Presentation.Rest.csproj", "server/identity/src/presentation/rest/"]
COPY ["server/identity/src/root/Root.csproj", "server/identity/src/root/"]

COPY ["server/common/src/application/Application.csproj", "server/common/src/application/"]
COPY ["server/common/src/domain/Domain.csproj", "server/common/src/domain/"]
COPY ["server/common/src/expansions/Expansions.csproj", "server/common/src/expansions/"]
COPY ["server/common/src/infrastructure/Infrastructure.csproj", "server/common/src/infrastructure/"]
COPY ["server/common/src/persistence/mongodb/Persistence.MongoDb.csproj", "server/common/src/persistence/mongodb/"]
COPY ["server/common/src/presentation/graphql/Presentation.GraphQL.csproj", "server/common/src/presentation/graphql/"]
COPY ["server/common/src/tools/Tools.csproj", "server/common/src/tools/"]

RUN dotnet restore "server/identity/src/root/Root.csproj"

COPY server/common/. server/common/
COPY server/identity/. server/identity/
RUN dotnet build "server/identity/src/root/Root.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /app
RUN dotnet publish "server/identity/src/root/Root.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meets.Identity.Root.dll"]